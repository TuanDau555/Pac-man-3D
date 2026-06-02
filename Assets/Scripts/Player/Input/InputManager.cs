using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private InputSystem_Actions inputSystemAction;

    #region Execute
    protected override void Awake()
    {
        base.Awake();
        inputSystemAction = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputSystemAction.Enable();
    }

    void OnDisable()
    {
        inputSystemAction.Disable();
    }
    #endregion
    
    #region Player Input Method
    
    public void DisableGameInput() => inputSystemAction.Player.Disable();
    public void EnableGameInput() => inputSystemAction.Player.Enable();
    
    public Vector2 GetPlayerMovement() => inputSystemAction.Player.Move.ReadValue<Vector2>();
    public Vector2 GetMouseDelta() => inputSystemAction.Player.Look.ReadValue<Vector2>();

    public bool IsFiringPressed() => inputSystemAction.Player.Attack.WasPressedThisFrame(); 

    public bool IsSprintedPressed() => inputSystemAction.Player.Sprint.ReadValue<float>() > 0f;
    
    #endregion

    #region UI Input Method

    public bool IsPauseGame() => inputSystemAction.UI.Pause.WasPressedThisFrame();
    
    #endregion
}