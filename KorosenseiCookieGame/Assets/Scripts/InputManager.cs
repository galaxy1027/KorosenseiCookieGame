using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public PlayerInputActions inputActions;

    public InputAction move;
    public InputAction jump;
    public InputAction crouch;
    public InputAction pause;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            inputActions = new PlayerInputActions();
        }
    }

    void OnEnable()
    {
        move = inputActions.Player.Move;
        jump = inputActions.Player.Jump;
        crouch = inputActions.Player.Crouch;
        pause = inputActions.UI.Pause;
        move.Enable();
        jump.Enable();
        crouch.Enable();
        pause.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();
        crouch.Disable();
        pause.Disable();
    }

}
