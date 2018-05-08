using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BigfootAnimations : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public PlayerMovement Movement { get; private set; }
    private MovementStates previousState;

    private bool canCrouch = true;

    private void Awake()
    {
        Animator = this.GetComponent<Animator>();
        Movement = this.transform.parent.GetComponent<PlayerMovement>();
        previousState = Movement.MovementState;
    }

    private void OnEnable()
    {
        NPC.DetectedBigfootEvent += PlayBigfootReact;
    }

    private void OnDisable()
    {
        NPC.DetectedBigfootEvent -= PlayBigfootReact;
    }

    private void Start()
    {
        Movement.MovementStateChanged += ActivateAnimation;
        Movement.WalkStateChanged += ActivateAnimation;
    }

    private void Update()
    {
        SetMoveAnimations(Movement.MovementState);
        SetWalkAnimations(Movement.WalkState);
        ForwardAndBackwardsAnimationSpeeds();
    }

    private void ForwardAndBackwardsAnimationSpeeds()
    {
        if (Movement.Input.InputDirection.y < 0) { Animator.SetFloat("WalkSpeed", -1); Animator.SetFloat("SprintSpeed", -1); Animator.SetFloat("CrouchSpeed", -1f); }
        else { Animator.SetFloat("WalkSpeed", 1); Animator.SetFloat("SprintSpeed", 1); Animator.SetFloat("CrouchSpeed", 1f); }
    }

    public void PlayBigfootReact(object sender, System.EventArgs e)
    {
        Animator.Play("React");
        canCrouch = false;
    }

    public void ActivateAnimation(MovementStateArgs e)
    {
        //Debug.Log("NEW Movement State + " + e.MovementState + " // OLD Walk State + " + movement.WalkState);
        //SetMoveAnimations(e.MovementState);
    }

    public void ActivateAnimation(WalkStateArgs e)
    {
       //Debug.Log("OLD Movement State + " + movement.MovementState + " // NEW Walk State + " + e.WalkState);
       //SetWalkAnimations(e.WalkState);
    }

    public void SetMoveAnimations(MovementStates moveState)
    {
        switch (moveState)
        {
            case MovementStates.Idle:
                Animator.SetInteger("Speed", 0);
                break;
            case MovementStates.Moving:
                if (!CutsceneManagement.InCutscene)
                {
                    if (Movement.WalkState != WalkStates.Sprint) { Animator.SetInteger("Speed", 1); }
                    else { Animator.SetInteger("Speed", 2); }
                }
                break;
            default:
                break;
        }
    }

    public void SetWalkAnimations(WalkStates walkState)
    {
        switch (walkState)
        {
            case WalkStates.Normal:
                Animator.SetInteger("Crouch", 0);
                break;
            case WalkStates.Crouch:
                if (!CutsceneManagement.InCutscene)
                {
                    if (canCrouch) { Animator.SetInteger("Crouch", 1); }
                    else { Animator.SetInteger("Crouch", 0); }
                }
                break;
            default:
                break;
        }
    }

}