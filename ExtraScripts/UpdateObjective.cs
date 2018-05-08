using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    FindClothes, GoToBusStation, LearnToSpeak, FollowGirl, GoToDriverAfterSpeech,
    FindFood, LearnToGrill, GoToDriverAfterBurger, GetOnBus
}

public class UpdateObjective : MonoBehaviour
{
    private SubtitleManager subRef;

    public AudioClip ObjectiveUpdate;
    public GameState gameState;
    public GameState CurrentObjective
    {
        get { return CheckpointValues.CurrentState; }
        private set { CheckpointValues.CurrentState = value; if (ObjectiveChangedEvent != null)
            { ObjectiveChangedEvent(this, EventArgs.Empty); } }
    }

    public static EventHandler ObjectiveChangedEvent;

    private ObjectiveUI objUI;
    private CheckpointValues cp_Ref;
    public AudioSource ObjectivePlayer;

    #region On Scene Load
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        gameState = CheckpointValues.CurrentState;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
    #endregion


    // Use this for initialization
    void Start ()
    {
        cp_Ref = GameObject.FindObjectOfType<CheckpointValues>();
        //ObjectivePlayer = GetComponent<AudioSource>();
        subRef = this.GetComponentInChildren<SubtitleManager>();
        //SetState_CLOTHES();
    }


    public void SetUIForState(GameState state)
    {
        if (subRef == null) { subRef = this.GetComponentInChildren<SubtitleManager>(); }

        //switch (state)
        //{
        //    //case GameState.MakeBurger:
        //    //    subRef.PlaySubtitle(49);
        //    //    break;
        //    case GameState.GoToDriverAfterBurger:
        //        subRef.PlaySubtitle(53);
        //        break;
        //}
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
        CurrentObjective = gameState;
        SetUIForState(gameState);
        ObjectivePlayer.PlayOneShot(ObjectiveUpdate);
    }
    
    public void FindClothes()
    {
        ChangeState(GameState.FindClothes);
    }

    public void GoToBusStation()
    {
        ChangeState(GameState.GoToBusStation);
    }

    public void LearnToSpeak()
    {
        ChangeState(GameState.LearnToSpeak);
    }

    public void FollowGirl()
    {
        ChangeState(GameState.FollowGirl);
    }

    public void GoToDriverAfterSpeech()
    {
        ChangeState(GameState.GoToDriverAfterSpeech);
    }

    public void FindFood()
    {
        ChangeState(GameState.FindFood);
    }

    public void LearnToGrill()
    {
        ChangeState(GameState.LearnToGrill);
    }

    public void MakeBurger()
    {
        //ChangeState(GameState.MakeBurger);
    }

    public void GoToDriverAfterBurger()
    {
        ChangeState(GameState.GoToDriverAfterBurger);
        StartCoroutine(PlaySubtitle53());
    }

    public void GetOnBus()
    {
        ChangeState(GameState.GetOnBus);
    }

    IEnumerator PlaySubtitle53()
    {
        yield return new WaitForEndOfFrame();
        subRef.PlaySubtitle(53);
    }
}
