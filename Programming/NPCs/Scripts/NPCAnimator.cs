using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPCAnimator : MonoBehaviour
{
    //References
    private Animator animator;
    private NPC npc;
    public NPCMovement NpcMove { get; private set; }

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        npc = this.GetComponentInParent<NPC>();
        NpcMove = this.GetComponentInParent<NPCMovement>();
        //if (npc != null) { NPC.DetectedBigfootEvent += PlayReactionAnimation; }
        if (NpcMove != null) { NpcMove.MoveStateChanged += MoveAnimationStateChanged; }
    }

    private void OnEnable()
    {
        NPC.DetectedBigfootEvent += PlayReactionAnimation; 
    }

    private void OnDisable()
    {
        NPC.DetectedBigfootEvent -= PlayReactionAnimation;
    }

    private void Start()
    {
        //Set default animation

        animator.Play("Idle");
        MoveAnimationStateChanged(new MovementStateArgs(MovementStates.Idle));

        //if(npcMove.CanMove)
        //{
        //    animator.Play("Idle");
        //    MoveAnimationStateChanged(new MovementStateArgs(MovementStates.Idle));
        //}
        //else
        //{
        //    animator.Play("Moving");
        //    MoveAnimationStateChanged(new MovementStateArgs(MovementStates.Moving));
        //}
    }
    
    private void PlayReactionAnimation(object sender, EventArgs e)
    {
        if((NPC)sender == npc)
        {
            animator.Play("Reaction");
        }
    }

    private void MoveAnimationStateChanged(MovementStateArgs e)
    {
        switch (e.MovementState)
        {
            case MovementStates.Idle:
                animator.SetInteger("Speed", 0);
                break;
            case MovementStates.Moving:
                animator.SetInteger("Speed", 1); 
                break;
            default:
                break;
        }
    }
}
