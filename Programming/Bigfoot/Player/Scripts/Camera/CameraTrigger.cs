using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private AutoCamera autoCam;
    private CameraStyleSwitcher camSwitch;

    public float newXRot = 20;
    public float newDistance = 3.5f;
    public float newHeight = 1.7f;
    public bool resetIfObjectiveCompleted = false;
    public bool onlyActivateOnCertainObjective = false;
    public GameState objectiveToActivateTrigger;
    public GameState objectiveToComplete;

    private bool objectiveCanComplete = false;

    private void Awake()
    {
        camSwitch = GameObject.FindObjectOfType<CameraStyleSwitcher>();
        autoCam = GameObject.FindObjectOfType<AutoCamera>();
    }

    private void OnEnable()
    {
        UpdateObjective.ObjectiveChangedEvent += ObjectiveChanged;
    }

    private void OnDisable()
    {
        UpdateObjective.ObjectiveChangedEvent -= ObjectiveChanged;
    }

    private void ObjectiveChanged(object sender, System.EventArgs e)
    {
        if (camSwitch.CamStyle == CameraStyle.Auto)
        {
            if (objectiveCanComplete)
            {
                autoCam.ReturnToDefaultValues();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(camSwitch.CamStyle == CameraStyle.Auto)
        {
            if (other.gameObject.GetComponent<Bigfoot>() != null)
            {
                if (onlyActivateOnCertainObjective)
                {
                    if (CheckpointValues.CurrentState != objectiveToActivateTrigger ||
                        CheckpointValues.CurrentState != objectiveToComplete)
                    {
                        return;
                    }
                }

                autoCam.SetCameraValues(newXRot, newDistance, newHeight);
            }
        }
            
    }

    private void OnTriggerStay(Collider other)
    {
        if (camSwitch.CamStyle == CameraStyle.Auto)
        {
            if (other.gameObject.GetComponent<Bigfoot>() != null)
            {
                if (resetIfObjectiveCompleted)
                {
                    if (CheckpointValues.CurrentState == objectiveToComplete && !objectiveCanComplete)
                    {
                        objectiveCanComplete = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (camSwitch.CamStyle == CameraStyle.Auto)
        {
            if (other.gameObject.GetComponent<Bigfoot>() != null)
            {
                autoCam.ReturnToDefaultValues();
            }
        }
    }
}
