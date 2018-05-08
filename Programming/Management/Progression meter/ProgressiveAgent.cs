using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressiveAgent : MonoBehaviour {

    public float fillSpeed = 10;
    public bool CrouchingIsRequired = true;
    private ProgressionMeterManager pMM;
    private bool meterComplete;
    public List<UnityEvent> OnProgressionMeterCompletionActions = new List<UnityEvent>();

    void Start ()
    {
        pMM = GameObject.FindObjectOfType<ProgressionMeterManager>();
    }

    public void TestProgression(PlayerMovement pMovement)
    {
        if(CrouchingIsRequired && pMovement.WalkState != WalkStates.Crouch) { return; }
        if (pMM.ProgressValue >= 100)
        {
            if (!meterComplete)
            {
                meterComplete = true;
                OnProgressionMeterCompletionActions.ForEach(x => x.Invoke());
            }
        }
        else
        {
            pMM.UpdateProgressionBar(fillSpeed);
        }
    }
	
}
