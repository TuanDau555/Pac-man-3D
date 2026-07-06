using UnityEngine;

public class IdleBreathingModule : ICameraMotionModule
{
    #region Parameter
    // --- Settings ---
    // Tốc độ thay đổi của noise (nhỏ = chậm, mượt)
    private const float PositionNoiseSpeed = 0.4f;
    private const float RotationNoiseSpeed = 0.3f;

    // Biên độ dao động
    private const float PositionAmplitude = 0.012f;  // meters
    private const float RotationAmplitude = 0.1f;     // degrees

    // Tốc độ blend in/out giữa idle và đang đi
    private const float BlendSpeed = 3f;

    // Ngưỡng velocity để coi là "đứng yên"
    private const float IdleThreshold = 0.05f;

    // --- Noise offsets (seed): mỗi trục dùng sample khác nhau ---
    // Dùng prime numbers để các trục noise không tương quan nhau
    private readonly float _seedPX;
    private readonly float _seedPY;
    private readonly float _seedPZ;
    private readonly float _seedRX;
    private readonly float _seedRY;
    private readonly float _seedRZ;

    // --- State ---
    private float _noiseTime = 0f;
    private float _idleBlend = 0f;

    private Vector3 _positionOffset = Vector3.zero;
    private Vector3 _rotationOffset = Vector3.zero;
    #endregion

    public IdleBreathingModule()
    {
        // Random seed lúc khởi tạo để mỗi lần chơi khác nhau
        _seedPX = Random.Range(0f, 100f);
        _seedPY = Random.Range(100f, 200f);
        _seedPZ = Random.Range(200f, 300f);
        _seedRX = Random.Range(300f, 400f);
        _seedRY = Random.Range(400f, 500f);
        _seedRZ = Random.Range(500f, 600f);
    }

    public void Tick(float deltaTime, CameraMotionData data)
    {
        // Kiểm tra player có đứng yên không (chỉ xét horizontal)
        float horizontalSpeed = new Vector2(data.Velocity.x, data.Velocity.z).magnitude;
        bool isIdle = horizontalSpeed < IdleThreshold;

        // Blend idle effect vào khi đứng yên, blend ra khi di chuyển
        float targetBlend = isIdle ? 1f : 0f;
        _idleBlend = Mathf.Lerp(_idleBlend, targetBlend, BlendSpeed * deltaTime);

        if (_idleBlend < 0.001f)
        {
            _positionOffset = Vector3.zero;
            _rotationOffset = Vector3.zero;
            return;
        }

        // Tăng noise time
        _noiseTime += deltaTime;

        // Sample Perlin Noise cho từng trục
        // Mathf.PerlinNoise trả về [0,1] → remap sang [-1,1]
        float px = SampleNoise(_seedPX, _noiseTime * PositionNoiseSpeed) * PositionAmplitude;
        float py = SampleNoise(_seedPY, _noiseTime * PositionNoiseSpeed * 0.7f) * PositionAmplitude;
        float pz = SampleNoise(_seedPZ, _noiseTime * PositionNoiseSpeed * 0.5f) * PositionAmplitude * 0.3f;

        float rx = SampleNoise(_seedRX, _noiseTime * RotationNoiseSpeed) * RotationAmplitude;
        float ry = SampleNoise(_seedRY, _noiseTime * RotationNoiseSpeed * 0.6f) * RotationAmplitude * 0.5f;
        float rz = SampleNoise(_seedRZ, _noiseTime * RotationNoiseSpeed * 0.8f) * RotationAmplitude * 0.4f;

        _positionOffset = new Vector3(px, py, pz) * _idleBlend;
        _rotationOffset = new Vector3(rx, ry, rz) * _idleBlend;
    }

    /// <summary>Sample Perlin Noise và remap [0,1] → [-1,1].</summary>
    private static float SampleNoise(float seed, float time)
    {
        return (Mathf.PerlinNoise(seed, time) - 0.5f) * 2f;
    }

    public Vector3 GetPositionOffset() => _positionOffset;
    public Vector3 GetRotationOffset() => _rotationOffset;
}