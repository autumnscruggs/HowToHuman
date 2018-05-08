using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpeech : NPCDialogue
{
    //[SerializeField] private float learningSpeechTriggerRadius = 6f;
    private UpdateObjective updateObj;

    protected override void Awake()
    {
        base.Awake();
        updateObj = GameObject.FindObjectOfType<UpdateObjective>();
        this.npcMovement.CanMove = false;
    }

    protected override void Start()
    {
        base.Start();

       // BoxCollider collider = this.gameObject.transform.parent.GetChild(1).GetComponent<BoxCollider>();
       // collider.size = new Vector3(this.learningSpeechTriggerRadius, collider.size.y, this.learningSpeechTriggerRadius);
    }

    public override void PlayDialogue()
    {
        if (updateObj.CurrentObjective == GameState.LearnToSpeak)
        {
            this.npcMovement.StartMove();
            this.CanBeInteractedWith = false;

        }
        //base.PlayDialogue();
    }

    public override void PlayInteractionActions()
    {
        if(updateObj.CurrentObjective == GameState.LearnToSpeak)
        {
            base.PlayInteractionActions();
        }
    }

    protected override void LookAtPlayer()
    {
        //base.LookAtPlayer();
    }

   


}
