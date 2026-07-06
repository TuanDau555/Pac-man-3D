using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bộ điều khiển camera duy nhất trong toàn hệ thống.
/// Nhận dữ liệu từ PlayerController, cập nhật tất cả module,
/// tổng hợp offset và apply lên Camera.
/// KHÔNG module nào được phép ghi trực tiếp lên camera.localPosition / localRotation.
/// </summary>
public class CameraMotionController : MonoBehaviour
{
    [Header("Target Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Player Body (để xoay Yaw)")]
    [SerializeField] private Transform playerBody;

    [Header("Configure")]
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private PlayerCamStatsSO playerCamStatsSO;

    // Modules
    private LookSmoothingModule _lookSmoothing;
    private HeadBobModule _headBob;
    private CameraSpringModule _cameraSpring;
    private AccelerationModule _acceleration;
    private IdleBreathingModule _idleBreathing;

    private List<ICameraMotionModule> _modules;

    // Data từ PlayerController
    private CameraMotionData _data;

    private Vector3 _basePosition;

    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        _basePosition = cameraTransform.localPosition;

        // Khởi tạo modules
        _lookSmoothing = new LookSmoothingModule(playerStatsSO);
        _headBob = new HeadBobModule(playerCamStatsSO);
        _cameraSpring = new CameraSpringModule(playerCamStatsSO);
        _acceleration = new AccelerationModule(playerCamStatsSO);
        _idleBreathing = new IdleBreathingModule();

        // Danh sách các module đóng góp offset (không tính LookSmoothing — xử lý riêng rotation)
        _modules = new List<ICameraMotionModule>
        {
            _headBob,
            _cameraSpring,
            _acceleration,
            _idleBreathing,
        };
    }

    /// <summary>
    /// PlayerController gọi mỗi frame để cung cấp dữ liệu.
    /// </summary>
    public void ReceiveData(CameraMotionData data)
    {
        _data = data;
    }

    private void LateUpdate()
    {
        float dt = Time.deltaTime;

        // 1. Cập nhật LookSmoothing
        _lookSmoothing.Tick(dt, _data);

        // 2. Tách Yaw và Pitch ra riêng:
        //    - Yaw  → xoay body của player (để WASD luôn đúng hướng)
        //    - Pitch → chỉ xoay camera (nhìn lên/xuống)
        float currentYaw = _lookSmoothing.GetCurrentYaw();
        Quaternion pitchOnly = _lookSmoothing._lookRotation;

        if (playerBody != null)
            playerBody.localRotation = Quaternion.Euler(0f, currentYaw, 0f);

        // 3. Cập nhật tất cả modules còn lại
        foreach (var module in _modules)
            module.Tick(dt, _data);

        // 4. Tổng hợp offset
        Vector3 finalPosition = _basePosition;
        Vector3 finalRotationEuler = Vector3.zero;

        foreach (var module in _modules)
        {
            finalPosition += module.GetPositionOffset();
            finalRotationEuler += module.GetRotationOffset();
        }

        // 5. Apply lên Camera
        //    localRotation = pitchOnly (đã có cả yaw lẫn pitch từ LookSmoothing)
        //    nhưng vì playerBody đã xoay yaw, camera chỉ cần pitch trong local space
        cameraTransform.localPosition = finalPosition;
        cameraTransform.localRotation = Quaternion.Euler(-_lookSmoothing.GetCurrentPitch(), 0f, 0f)
                                        * Quaternion.Euler(finalRotationEuler);
    }
}