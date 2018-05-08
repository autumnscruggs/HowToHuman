using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Lazy Observer
public delegate void MovementStateHandler(MovementStateArgs e);
public delegate void WalkStateHandler(WalkStateArgs e);
public class MovementStateArgs : EventArgs
{
    private MovementStates moveState;
    public MovementStateArgs(MovementStates _moveState)
    {
        moveState = _moveState;
    } // eo ctor

    public MovementStates MovementState {get { return moveState; } }
} // eo class MyEventArgs
public class WalkStateArgs : EventArgs
{
    private WalkStates walkState;
    public WalkStateArgs(WalkStates _walkState)
    {
        walkState = _walkState;
    } // eo ctor

    public WalkStates WalkState { get { return walkState; } }
} // eo class MyEventArgs
#endregion

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    //Event
    public event MovementStateHandler MovementStateChanged;
    public event WalkStateHandler WalkStateChanged; 

    //Inspector Bits
    [Header("--- Movement Speed ---")]
    public float baseSpeed = 50f;
    public float walkSpeedModifier = 2f;
    public float crouchSpeedModifier = 1f;
    public float sprintSpeedModifier = 4f;
    [Header("--- Movement State ---")]
    [SerializeField] private MovementStates moveState;
    [SerializeField] private WalkStates walkState;
    public MovementStates MovementState
    {
        get
        {
            return moveState;
        }
        set
        {
            if (moveState != value)
            {
                moveState = value;
                if (MovementStateChanged != null) { MovementStateChanged(new MovementStateArgs(moveState)); }
            }
        }
    }
    public WalkStates WalkState
    {
        get
        {
            return walkState;
        }
        set
        {
            if (walkState != value)
            {
                walkState = value;
                if (WalkStateChanged != null) { WalkStateChanged(new WalkStateArgs(walkState)); }
            }
        }
    }
    public bool CanMove = true;
    public bool CanRun = true;
    [Header("--- Sliding ---")]
    public float drag = 4f;

    //Non-Inspector Bits
    private float speed = 0;

    public float TotalSpeed { get { return speed; } }

    private Vector3 velocity;
    [HideInInspector] public float maxVelocityChange = 50f;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Collider pCollider;

    private Vector3 targetDirection;
    private Vector3 targetVelocity;

    //Dependencies
    private PlayerInput playerInput;
    public PlayerInput Input { get { return playerInput; } }

    private void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        //rigidbody
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        rigidBody.useGravity = true;
        rigidBody.drag = drag;
        //collider info
        pCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        SetMoveState();
        SetWalkState();
    }

    private void FixedUpdate()
    {
        if (CanMove && playerInput.MovementInputPushed && Time.timeScale > 0)
        {
            SetSpeed();
            UpdateMovement();
        }
    }

    private void SetWalkState()
    {
        if (playerInput.WalkState == WalkStates.Crouch)
        {
            if (WalkState != WalkStates.Crouch) { WalkState = WalkStates.Crouch; }
        }
        else if(playerInput.WalkState == WalkStates.Sprint)
        {
            if (WalkState != WalkStates.Sprint) { WalkState = WalkStates.Sprint; }
        }
        else
        {
            if (WalkState != WalkStates.Normal) { WalkState = WalkStates.Normal; }
        }
    }

    private void SetMoveState()
    {
        MovementState = playerInput.MovementState; 
    }

    private void SetSpeed()
    {
        switch (WalkState)
        {
            case WalkStates.Normal:
                speed = baseSpeed * walkSpeedModifier;
                break;
            case WalkStates.Crouch:
                speed = baseSpeed * crouchSpeedModifier;
                break;
            case WalkStates.Sprint:
                speed = baseSpeed * sprintSpeedModifier;
                break;
        }
    }

    private void UpdateMovement()
    {
        RigidbodyMovement();
    }

    private void RigidbodyMovement()
    {
        if(playerInput.InputDirection.y >= 0) { targetDirection = this.transform.forward; }
        else { targetDirection = -this.transform.forward; }

        targetVelocity = targetDirection;

        targetVelocity *= Time.deltaTime * speed;

        var velocity = rigidBody.velocity;
        var velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
    }
}

