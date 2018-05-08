using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusBoyTracker : MonoBehaviour
{
    [HideInInspector] public UpdateObjective updateobj_Ref;
    private BusCutscene b_cutsceneRef;
    public bool CanPlayCutscene = false;
    //private bool playingCutscene = false;

    private void Awake()
    {
        updateobj_Ref = GameObject.FindObjectOfType<UpdateObjective>();
        b_cutsceneRef = GameObject.FindObjectOfType<BusCutscene>();
    }

    public void Update()
    {
        if (CanPlayCutscene)
        {
            PlayCutsceneBasedOnObjective();
        }
    }

    public void BusDriverInteraction()
    {
        CanPlayCutscene = true;
        //playingCutscene = true;
    }

    private void PlayCutsceneBasedOnObjective()
    {
        if (updateobj_Ref.CurrentObjective == GameState.GoToBusStation)
        {
            b_cutsceneRef.PlayCutscene(11, 16.5f, 1);
            //updateobj_Ref.ChangeState(GameState.LearnToSpeak);

        }
        else if (updateobj_Ref.CurrentObjective == GameState.GoToDriverAfterSpeech)
        {
            b_cutsceneRef.PlayCutscene(33, 29, 2);
            //updateobj_Ref.ChangeState(GameState.FindFood);

        }
        else if (updateobj_Ref.CurrentObjective == GameState.GoToDriverAfterBurger)
        {
            b_cutsceneRef.PlayCutscene(55, 16.5f, 3);
            //updateobj_Ref.ChangeState(GameState.GetOnBus);

        }
    }
}
