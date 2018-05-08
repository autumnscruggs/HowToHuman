using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class TutorialForSpeech : MonoBehaviour
{
    public UnityEvent AfterTutorial;
    private Bigfoot bigfoot;
    public NPC npc;

    public float tutorialDelayTime = 5f;

    private SubtitleManager sManager_ref;
    private PhoneGirlCameraCinematic pgc_Ref;

    private void Awake()
    {
        bigfoot = GameObject.FindObjectOfType<Bigfoot>();
        sManager_ref = FindObjectOfType<SubtitleManager>().GetComponent<SubtitleManager>();
        pgc_Ref = FindObjectOfType<PhoneGirlCameraCinematic>();
    }

    public void StartTutorial()
    {
        if(PauseData.ShowTutorials)
        {
            pgc_Ref.ActivateValCam();
            CutsceneManagement.InCutscene = true;
            //TODO: Play phone girl's dialogue and then the narrator's
            sManager_ref.PlaySubtitle(20);
            StartCoroutine(TutorialDelay());
            //Stop movement for bigfoot but start the girl's movement
            npc.GetComponent<NPCMovement>().StartMove();
            bigfoot.GetComponent<PlayerMovement>().CanMove = false;
        }
        else
        {
            AfterTutorial.Invoke();
        }
    }

    IEnumerator TutorialDelay()
    {
        yield return new WaitForSeconds(tutorialDelayTime);
        CutsceneManagement.InCutscene = false;
        pgc_Ref.DeActivateValCam();
        //Allow movement
        bigfoot.GetComponent<PlayerMovement>().CanMove = true;
        //Invoke after tutorial stuff
        AfterTutorial.Invoke();
        StartConversation();

    }

    public void StartConversation()
    {
        sManager_ref.PlaySubtitle(22);
    }

    public void EndConversation()
    {
        sManager_ref.TurnOffSubtitleImmediately();
    }
}
