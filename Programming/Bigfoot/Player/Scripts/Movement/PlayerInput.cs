using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputStyle { Hold, Toggle }
public enum MovementStates { Idle, Moving }
public enum WalkStates { Normal, Sprint, Crouch }

public class PlayerInput : MonoBehaviour
{
    //Inspector Values
    [Header("Input Style Options")]
    public InputStyle CrouchStyle;
    public InputStyle SprintStyle;

    //[Header("States")]
    private MovementStates moveState;
    [SerializeField]private WalkStates walkState;

    [Header("Input Direction")]
    [SerializeField] private Vector2 inputDirection;

    //General Properties & Dependencies
    public MovementStates MovementState
    {
        get
        {
            return moveState;
        }
        private set
        {
            moveState = value;
        }
    }
    public WalkStates WalkState
    {
        get
        {
            return walkState;
        }
        private set
        {
            walkState = value;
        }
    }
    public Vector2 InputDirection
    {
        get { return inputDirection; }
    }
    public Gamepad Gamepad { get; private set; }

    //Private Instance Members
    private bool sprint = false;
    private bool crouch = false;

    [SerializeField] private float gamepadX;
    [SerializeField] private float gamepadY;

    //Input Tests
    public bool InteractionInputHeld
    {
        get
        {
            return (Gamepad != null && Gamepad.IsConnected && Gamepad.IsHoldingButtonFromList(InputManager.Gamepad.InteractionInputs))
            || KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.InteractionInputs);
        }
    }
    public bool InteractionInputPushed
    {
        get
        {
            return (Gamepad != null && Gamepad.IsConnected && (Gamepad.IsButtonDownFromList(InputManager.Gamepad.InteractionInputs) || Gamepad.IsButtonUpFromList(InputManager.Gamepad.InteractionInputs) || Gamepad.IsHoldingButtonFromList(InputManager.Gamepad.InteractionInputs)))
            || KeyboardInput.IsKeyDownFromList(InputManager.Keyboard.InteractionInputs);
        }
    }
    public bool MovementInputPushed
    {
        get
        {
            return (Gamepad != null && Gamepad.IsConnected && Gamepad.IsMovingLeftStick) ||
            KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.LeftInputs) ||
            KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.RightInputs) ||
            KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.UpInputs) ||
            KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.DownInputs);
        }
    }
    public bool ForwardOrBackwardMovementInputPushed
    {
       get
        {
            return (Gamepad != null && Gamepad.IsConnected && (Gamepad.GetStick_L().Y > InputManager.GamepadDeadSpace || Gamepad.GetStick_L().Y < -InputManager.GamepadDeadSpace)) ||
           KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.UpInputs) ||
           KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.DownInputs);
        }
    }
    public bool LeftOrRightMovementInputPushed
    {
        get
        {
            return (Gamepad != null && Gamepad.IsConnected && (Gamepad.GetStick_L().X > InputManager.GamepadDeadSpace || Gamepad.GetStick_L().X < -InputManager.GamepadDeadSpace)) ||
          KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.LeftInputs) ||
          KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.RightInputs);
        }
    }

    private void Awake()
    {
        if (GamepadManager.Instance != null)
        {
            Gamepad = GamepadManager.Instance.GetGamepad(1);
        }
    }

    private void Start()
    {
        MovementState = MovementStates.Idle;
        WalkState = WalkStates.Normal;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        GetMovementDirection();
        if (!crouch) { GetSprintInput(); }
        GetCrouchInput();
        SetStates();
    }

    private void GetMovementDirection()
    {
        if (Gamepad != null && Gamepad.IsConnected && Gamepad.IsMovingLeftStick)
        {
            inputDirection.x = Gamepad.GetStick_L().X;
            inputDirection.y = Gamepad.GetStick_L().Y;

            gamepadX = Gamepad.GetStick_L().X;
            gamepadY = Gamepad.GetStick_L().Y;

            //if (Gamepad.GetStick_L().X < InputManager.GamepadDeadSpace)
            //{
            //    inputDirection.x = -1;
            //}
            //else if (Gamepad.GetStick_L().X > InputManager.GamepadDeadSpace)
            //{
            //    inputDirection.x = 1;
            //}
            //else
            //{
            //    inputDirection.x = 0;
            //}

            //if (Gamepad.GetStick_L().Y < InputManager.GamepadDeadSpace)
            //{
            //    inputDirection.y = -1;
            //}
            //else if (Gamepad.GetStick_L().Y > InputManager.GamepadDeadSpace)
            //{
            //    inputDirection.y = 1;
            //}
            //else
            //{
            //    inputDirection.y = 0;
            //}
        }
        if (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.LeftInputs) || KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.UpInputs) ||
            KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.RightInputs) || KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.DownInputs))
        {
            if (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.LeftInputs))
            {
                inputDirection.x = -1;
            }
            else if (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.RightInputs))
            {
                inputDirection.x = 1;
            }
            else
            {
                inputDirection.x = 0;
            }

            if (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.DownInputs))
            {
                inputDirection.y = -1;
            }
            else if (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.UpInputs))
            {
                inputDirection.y = 1;
            }
            else
            {
                inputDirection.y = 0;
            }
        }
    }
    private void GetSprintInput()
    {
        if(SprintStyle == InputStyle.Toggle)
        {
            if (Gamepad != null && Gamepad.IsConnected)
            {
                if (Gamepad.IsButtonDownFromList(InputManager.Gamepad.SprintInputs)) { sprint = !sprint; }
            }

            if (KeyboardInput.IsKeyDownFromList(InputManager.Keyboard.SprintInputs)) { sprint = !sprint; }
        }
        else
        {
            if (Gamepad != null && Gamepad.IsConnected)
            {
                if (Gamepad.IsHoldingButtonFromList(InputManager.Gamepad.SprintInputs) ||
                    (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.SprintInputs))) { sprint = true; }
                else { sprint = false; }
            }
            else
            {
                if (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.SprintInputs)) { sprint = true; }
                else { sprint = false; }
            }
        }
    }
    private void GetCrouchInput()
    {
        if (CrouchStyle == InputStyle.Toggle)
        {
            if (Gamepad != null && Gamepad.IsConnected)
            {
                if (Gamepad.IsButtonDownFromList(InputManager.Gamepad.CrouchInputs)) { crouch = !crouch; }
            }

            if (KeyboardInput.IsKeyDownFromList(InputManager.Keyboard.CrouchInputs)) { crouch = !crouch; }
        }
        else
        {
            if (Gamepad != null && Gamepad.IsConnected)
            {
                if (Gamepad.IsHoldingButtonFromList(InputManager.Gamepad.CrouchInputs) ||
                    (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.CrouchInputs))) { crouch = true; }
                else { crouch = false; }
            }
            else
            {
                if (KeyboardInput.IsHoldingKeyFromList(InputManager.Keyboard.CrouchInputs)) { crouch = true; }
                else { crouch = false; }
            }
        }
    }
    private void SetStates()
    {
        //Set movement state
        if (!MovementInputPushed)
        {
            if (MovementState != MovementStates.Idle) { MovementState = (MovementStates.Idle); }
        }
        else
        {
            if (MovementState != MovementStates.Moving) { MovementState = (MovementStates.Moving); }
        }
        //Set walk state
        if (crouch)
        {
            WalkState = WalkStates.Crouch;
        }
        else if (sprint)
        {
            WalkState = WalkStates.Sprint;
        }
        else
        {
            WalkState = WalkStates.Normal;
        }
    }
}
