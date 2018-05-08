using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Detection : MonoBehaviour {

    // Use this for initialization
    private LearningGrillManager LGM;
    private ArchingProjectileSpawner aPS;
    private DetectionCameraManager detection;
    private ControllerSwitch controller;
    public NPC dadNPC;

    private bool detected = false;

    void Start ()
    {
        if(dadNPC == null) { dadNPC = GameObject.FindObjectOfType<DadScriptedEvent>().gameObject.GetComponent<NPC>(); }

        LGM = FindObjectOfType<LearningGrillManager>();
        aPS = FindObjectOfType<ArchingProjectileSpawner>();
        detection = GameObject.FindObjectOfType<DetectionCameraManager>();
        controller = GameObject.FindObjectOfType<ControllerSwitch>();
    }

    public void DetectPlayer()
    {
        if(!detected)
        {
            dadNPC.GetComponent<NPCMovement>().Agent.updateRotation = false;
            dadNPC.transform.LookAt(LGM.player.transform.position);
            detection.ManualActivation(dadNPC.gameObject);
            //controller.SwitchToOriginal();
            detected = true;
        }
    }
}
