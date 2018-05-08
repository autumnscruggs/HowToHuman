using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour {

    public int bushNumber = 0;
    public LearningGrillManager LGM;
    private UpdateObjective updateObj;
    private ControllerSwitch controlSwitch;

    public GameObject Lights;

    

    void Start()
    {
        updateObj = GameObject.FindObjectOfType<UpdateObjective>();
        controlSwitch = GameObject.FindObjectOfType<ControllerSwitch>();
        LGM = FindObjectOfType<LearningGrillManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            LGM.InBush = true;
            Lights.SetActive(false);

            //if (LGM.ProgressStatus == LearningGrillManager.ProgressionStatus.Idle)
            //{
            //    LGM.MiniGameUI.SetActive(true);
            //}

            if (updateObj.CurrentObjective == GameState.LearnToGrill && controlSwitch.CanSwitch)
            {
                //Any minigame start functions here   
                controlSwitch.SwitchToGrillBigfoot();
                controlSwitch.GrillController.SetStartingBush(bushNumber);
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            LGM.InBush = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            LGM.InBush = false;
        }
    }
}
