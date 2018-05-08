using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCameraScript : MonoBehaviour
{
    //Fix the 'Why Bigfoot can't move when you turn off his movement' glitch

    public GameObject PlayerCamera, IntroCamera, Bigfoot, SkipText;
    public Animation CameraAnimation { get; set; }
    private bool isCutscenePlaying, shitdone, PlayerTurnedOn;
    public bool IsCutscenePlaying { get { return isCutscenePlaying; } }

    private PlayerMovement playermovement_Ref;
    private FadeInGame fadeIn_Ref;
    private SubtitleManager subtitlesRef;
    private CheckpointValues cpvRef;
    //private bool CutsceneTriggered;

    public static System.EventHandler CutsceneComplete;

    // Use this for initialization
    void Start()
    {
        //Bigfoot = GameObject.Find("Bigfoot");
        playermovement_Ref = Bigfoot.GetComponent<PlayerMovement>();
        CameraAnimation = IntroCamera.GetComponent<Animation>();
        fadeIn_Ref = GameObject.Find("Cutscenes Manager").GetComponent<FadeInGame>();
        subtitlesRef = GameObject.FindObjectOfType<SubtitleManager>().GetComponent<SubtitleManager>();
        cpvRef = GameObject.FindObjectOfType<CheckpointValues>().GetComponent<CheckpointValues>();
        IntroCamera.SetActive(false);
    }

    private void Update()
    {
        PlayIntroScene();


        //if (CutsceneTriggered == true)
        //{
        //TurnOffPlayer();
        //InteruptCutscene();
        //}
    }


    public void PlayIntroScene()
    {
        if (CheckpointValues.CurrentState <= GameState.FindClothes)
        {
            if (shitdone == false)
            {
                if (isCutscenePlaying == false)
                {
                    isCutscenePlaying = true;
                    CutsceneManagement.InCutscene = true;
                    StartCoroutine(PlayCutscene());
                }
                else
                {
                    fadeIn_Ref.FadeIn(0.8f);
                    InteruptCutscene();
                }
            }
        }
        else
        {
            if (PlayerTurnedOn == false)
            {
                TurnOnPlayer();
                CutsceneManagement.InCutscene = false;
                fadeIn_Ref.FadeIn(255);
                PlayerTurnedOn = true;
            }

        }
    }



    void TurnOffPlayer()
    {
        //Bigfoot.SetActive(false);
        PlayerCamera.SetActive(false);
        playermovement_Ref.enabled = false;
        IntroCamera.SetActive(true);
        //UI.SetActive(false);
        SkipText.SetActive(true);
    }

    void TurnOnPlayer()
    {
        //Bigfoot.SetActive(true);
        PlayerCamera.SetActive(true);
        IntroCamera.SetActive(false);
        playermovement_Ref.enabled = true;
        //UI.SetActive(true);
        SkipText.SetActive(false);
    }

    IEnumerator PlayCutscene()
    {
        TurnOffPlayer();
        subtitlesRef.PlaySubtitle(0);
        yield return new WaitForSeconds(CameraAnimation.clip.length);
        TurnOnPlayer();
        shitdone = true;
        isCutscenePlaying = false;

        CutsceneManagement.InCutscene = false;
        if (CutsceneComplete != null) { CutsceneComplete(this, System.EventArgs.Empty); }
    }


    void InteruptCutscene()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            StopAllCoroutines();
            fadeIn_Ref.FadeIn(255);
            subtitlesRef.TurnOffSubtitleImmediately();
            TurnOnPlayer();
            shitdone = true;
            isCutscenePlaying = false;

            CutsceneManagement.InCutscene = false;
            if (CutsceneComplete != null) { CutsceneComplete(this, System.EventArgs.Empty); }
        }
    }
}
