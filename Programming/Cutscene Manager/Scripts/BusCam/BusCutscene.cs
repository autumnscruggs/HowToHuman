using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusCutscene : MonoBehaviour
{
    public GameObject PlayerCamera, IntroCamera, Bigfoot, BusBoy, UI;
    public Transform BigfootPosition;

    private PlayerMovement playermovement_Ref;
    private SubtitleManager subtitlesRef;
    private BusBoyTracker bbTracker_ref;

    private bool IsHappening;

    private float cutsceneTimer = 0;

    // Use this for initialization
    void Start()
    {
        playermovement_Ref = Bigfoot.GetComponent<PlayerMovement>();
        subtitlesRef = GameObject.FindObjectOfType<SubtitleManager>();
        bbTracker_ref = GameObject.FindObjectOfType<BusBoyTracker>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TurnOffPlayer()
    {
        Bigfoot.transform.position = BigfootPosition.transform.position;
        Bigfoot.transform.LookAt(BusBoy.transform.position);
        PlayerCamera.SetActive(false);
        playermovement_Ref.CanMove = false;
        IntroCamera.SetActive(true);
        UI.SetActive(false);
        IsHappening = true;
    }

    void TurnOnPlayer()
    {
        PlayerCamera.SetActive(true);
        IntroCamera.SetActive(false);
        playermovement_Ref.CanMove = true;
        UI.SetActive(true);
        IsHappening = false;
    }

    public void PlayCutscene(int SubtitleNum, float CutsceneTimeDuration, int Value)
    {
        if (IsHappening == false)
        {
            CutsceneManagement.InCutscene = true;
            TurnOffPlayer();
            subtitlesRef.PlaySubtitle(SubtitleNum);
        }
        else
        {
            cutsceneTimer += Time.deltaTime;
            if (cutsceneTimer >= CutsceneTimeDuration)
            {
                cutsceneTimer = 0;
                SetUpObjective(Value);
                TurnOnPlayer();
                CutsceneManagement.InCutscene = false;
                bbTracker_ref.CanPlayCutscene = false;

            }
        }
    }

    public void SetUpObjective(int Value)
    {
        if (Value == 1)
        {
            bbTracker_ref.updateobj_Ref.ChangeState(GameState.LearnToSpeak);
        }
        else if (Value == 2)
        {
            bbTracker_ref.updateobj_Ref.ChangeState(GameState.FindFood);
        }
        else if (Value == 3)
        {
            bbTracker_ref.updateobj_Ref.ChangeState(GameState.GetOnBus);
        }
    }
}
