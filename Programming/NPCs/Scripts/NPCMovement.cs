using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{
    //Inspector Values
    [Header("Nav Mesh Values")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stoppingDistance = 0.3f;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] private float acceleration = 4;
    [Header("Movement Values")]
    public List<Transform> Positions = new List<Transform>();
    public bool RandomizePositions = false;
    [SerializeField] private float idleTime = 2f;
    [SerializeField] private MovementStates moveState;
    [SerializeField] private bool canMove = true;
    public bool StopAtTheLastPoint = true;

    public bool RotateAtStoppingPoint = false;

    public float IdleTime { get { return idleTime; } set { idleTime = value; } }

    private MovementStates prevState;

    private enum RotateState { TurningAround, Chilling, TurningBackAround, Done }
    private RotateState rotateState;
    private RotateState RotationState
    { 
        get { return rotateState; }
        set
        {
            rotateState = value;
            if(value == RotateState.TurningAround || value == RotateState.TurningBackAround)
            {
                currentRotation = this.transform.rotation;
                //currentRotation = this.transform.eulerAngles;
                //currentRotation = this.transform.forward;
            }
        }
    }
    private bool temporaryStop = false;

    //Properties/Events
    public MovementStates MoveState
    {
        get { return moveState; }
        private set
        {
            if (moveState != value)
            {
                if (value == MovementStates.Idle)
                {
                    if(CanMove && !canStop) { return; }

                    //if (this.gameObject.name == "SpeechNPC") { Debug.Log("Ahhh // " + canStop); }
                    moveState = value;
                    if (MoveStateChanged != null)
                    {
                        //if (this.gameObject.name == "SpeechNPC") { Debug.Log("Ahhh // " + canStop); }
                        MoveStateChanged(new MovementStateArgs(MovementStates.Idle));
                    }
                }
                else if(value == MovementStates.Moving)
                {
                    if (CanMove)
                    {
                        moveState = value;
                        if (MoveStateChanged != null)
                        {
                           MoveStateChanged(new MovementStateArgs(MovementStates.Moving));
                        }
                    }
                }
            }
        }
    }
    public bool CanMove
    {
        get { return canMove; }
        set
        {
            canMove = value;

            if (canMove == false)
            {
                if(!waiting && Agent != null) { Agent.updateRotation = true; }

                if (MoveState == MovementStates.Moving)
                {
                    MoveState = MovementStates.Idle;
                }
            }
            else
            {
                Agent.isStopped = false;
                Agent.updateRotation = false;

                if (MoveState == MovementStates.Idle)
                {
                    MoveState = MovementStates.Moving;
                }
            }
        }
    }
    public MovementStateHandler MoveStateChanged;

    //Non-inspector Values
    [SerializeField] private int currentPosition;
    public int CurrentPosition { get { return currentPosition; } private set { currentPosition = value; } }
    private bool canIncrement = true;

    //References
    private NPC npc;
    public NavMeshAgent Agent { get; private set; }

    private bool waiting = false;
    [SerializeField] private float idleTimer = 0;
    private float rotateTime = 0;
    private float rotateTimer = 0;

    private Quaternion currentRotation;
    private int prevPosition = 0;
    [SerializeField] private Vector3 target;
    [SerializeField] private float distToTarget;

    private bool canStop = false;

    private void Awake()
    {
        npc = this.GetComponent<NPC>();
        MoveStateChanged += StateChanged;
        Agent = this.GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        NPC.DetectedBigfootEvent += StopMoving;
    }

    private void OnDisable()
    {
        NPC.DetectedBigfootEvent -= StopMoving;
    }

    private void StopMoving(object sender, System.EventArgs e)
    {
        this.CanMove = false;
        this.Agent.isStopped = true;
    }

    public void InitialDestination(int destination)
    {
        InitialDestination(Positions[destination]);
    }
    public void InitialDestination(Transform destination)
    {
        Agent.destination = destination.position;
        target = destination.position;
    }

    private void Start()
    {
        Agent.speed = speed;
        Agent.stoppingDistance = stoppingDistance;
        Agent.angularSpeed = rotationSpeed;
        Agent.acceleration = acceleration;
        Agent.updateRotation = true;
        if (Positions.Count > 0) { InitialDestination(0); }
        if (!CanMove) { Agent.isStopped = true; Agent.updateRotation = false; }
        else if(CanMove && Positions.Count > 0) { InitiateMovement(); }
    }

    private void Update()
    {
        if (CanMove) { MoveCharacter(); }
        IdleDelay();
    }

    private void MoveCharacter()
    {
        distToTarget = Vector3.Distance(this.transform.position, target);

        bool atPoint = Agent.remainingDistance <= 0f && Agent.remainingDistance != Mathf.Infinity
            && Agent.remainingDistance != Mathf.NegativeInfinity;
        bool correctPoint = distToTarget <= 0.8f;
        canStop = atPoint && correctPoint;
        if (canStop)
        {
            StartIdle();
        }

        if(RotateAtStoppingPoint)
        {
            if (waiting)
            {
                rotateTime = idleTime / 4;
                rotateTimer += Time.deltaTime;
                if (rotateTimer >= rotateTime) { RotationState++; rotateTimer = 0; }

                switch (rotateState)
                {
                    case RotateState.TurningAround:
                        Agent.updateRotation = false;
                        TurnAround(rotateTime);
                        break;
                    case RotateState.Chilling:
                        break;
                    case RotateState.TurningBackAround:
                        TurnAround(rotateTime);
                        break;
                    case RotateState.Done:
                        rotateTimer = 0;
                        Agent.updateRotation = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void StartIdle()
    {
        MoveState = MovementStates.Idle;
        currentRotation = this.transform.rotation;
        prevPosition = CurrentPosition;
        waiting = true;
    }

    private void IdleDelay()
    {
        if(waiting)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer >= idleTime)
            {
                idleTimer = 0;
                StopIdle();
            }
        }
    }

    private void StopIdle()
    {
        waiting = false;
        if (!temporaryStop) { MoveState = MovementStates.Moving; }
        canIncrement = true;
        RotationState = RotateState.Done;
        IncrementPosition();
        if (Positions.Count > 0 && CurrentPosition < Positions.Count && CanMove)
        { Agent.SetDestination(Positions[CurrentPosition].position); target = Positions[CurrentPosition].position; }
    }

    private void ResetPosition()
    {
        CurrentPosition = 0;
    }

    private void IncrementPosition()
    {
        if(RandomizePositions)
        {
            int random = 0;
            do
            {
                random = Random.Range(0, Positions.Count);
            }
            while (random == CurrentPosition);

            CurrentPosition = random;
        }
        else
        {
            if (CurrentPosition < Positions.Count)
            {
                CurrentPosition++;
            }
            else
            {
                if (!StopAtTheLastPoint) { ResetPosition(); }
                else
                {
                    CurrentPosition = (Positions.Count - 1); //Keep it at last position
                    CanMove = false;
                    canStop = true;
                    MoveState = MovementStates.Idle;
                }
            }


            //CurrentPosition++;
            //if (CurrentPosition >= Positions.Count)
            //{
            //    if (!StopAtTheLastPoint) { ResetPosition(); }
            //    else
            //    {
            //        CanMove = false;
            //        MoveState = MovementStates.Idle;
            //        CurrentPosition = (Positions.Count - 1); //Keep it at last position
            //    }
            //}
        }

        //Debug.Log("NPC // " + this.gameObject.name + "// currentPosition // " + currentPosition + " // Positions.Count // " + Positions.Count);

        RotationState = RotateState.TurningAround;
    }

    public void PauseMovement(bool pause)
    {
        if(pause) { prevState = this.MoveState; if (prevState != MovementStates.Idle) { MoveState = MovementStates.Idle; } }
        else { if (MoveState != prevState) { MoveState = prevState; } }
        if(!pause && !CanMove) { pause = true; }
        Agent.updateRotation = !pause;
        Agent.isStopped = pause;
        temporaryStop = pause;
    }

    private void StateChanged(MovementStateArgs e)
    {
        switch (e.MovementState)
        {
            case MovementStates.Idle:
                Agent.isStopped = true;
                break;
            case MovementStates.Moving:
                Agent.isStopped = false;
                break;
            default:
                break;
        }
    }

    public void TurnAround(float speed)
    {
        //var lookPos = currentRotation + 180f * Vector3.up;
        ////Debug.Log("CurrentRotation " + currentRotation + " // LookPos " + lookPos);
        //this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, lookPos, speed * Time.deltaTime); // lerp to new angles

        var targetRotation = currentRotation * Quaternion.Euler(0, 180, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

        //this.transform.Rotate(0, Time.deltaTime * 30, 0, Space.Self);

    }

    public void StartMove()
    {
        InitiateMovement();
    }

    private void InitiateMovement()
    {
        CanMove = true;
        Agent.updateRotation = true;
        MoveState = MovementStates.Moving;
        MoveStateChanged(new MovementStateArgs(MoveState));
    }
}
