using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DadScriptedEvent : MonoBehaviour
{
    private NPCMovement npc;
    private ToggleDad dad;

    public Transform grillTransform;
    public Transform grillTransform2;
    public Transform awayFromGrillTransform;
    private bool moved = false;


    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(dad == null) { dad = this.GetComponent<ToggleDad>(); }
        if (npc == null) { npc = this.gameObject.GetComponent<NPCMovement>(); }

        if (CheckpointValues.CurrentState > GameState.FindClothes /*&& CheckpointValues.CurrentState < GameState.GoToDriverAfterBurger*/)
        {
            npc.Positions.Clear();
            npc.transform.position = grillTransform2.transform.position;
            PutOnProperSpotHack(grillTransform2);
            npc.transform.localEulerAngles = new Vector3(0, -90, 0);
            dad.TurnOnGrillDad();
        }

        //else if (CheckpointValues.CurrentState > GameState.GoToDriverAfterBurger)
        //{
        //    dad.TurnOnNormalDad();
        //    npc.Positions.Clear();
        //    npc.transform.position = awayFromGrillTransform.transform.position;
        //    PutOnProperSpotHack(awayFromGrillTransform);
        //    npc.transform.localEulerAngles = new Vector3(0, -90, 0);
        //}

    }

    private void OnEnable()
    {
        UpdateObjective.ObjectiveChangedEvent += MoveToGrill;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        UpdateObjective.ObjectiveChangedEvent -= MoveToGrill;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void LateUpdate()
    {
        if (npc.CanMove)
        {
            int pos = this.GetComponent<NPCMovement>().Positions.FindIndex(x => x.position == grillTransform2.position);
            if (this.GetComponent<NPCMovement>().CurrentPosition == pos)
            {
                float distance = Vector3.Distance(this.transform.position, this.GetComponent<NPCMovement>().Positions[this.GetComponent<NPCMovement>().CurrentPosition].position);
                if (distance <= 0.9f)
                {
                    //Debug.Log("Wooop");
                    //npc.CanMove = false;
                    //npc.PauseMovement(true);
                    dad.TurnOnGrillDad();
                    //PutOnProperSpotHack(grillTransform2);
                }
            }
        }
    }

    public void MoveAwayFromGrill()
    {
        npc.PauseMovement(false);
        npc.CanMove = true;
        if (!npc.Positions.Contains(awayFromGrillTransform)){ npc.Positions.Add(awayFromGrillTransform);}
        npc.StopAtTheLastPoint = true;
    }

    public void MoveToGrill(object sender, EventArgs e)
    {
        if (CheckpointValues.CurrentState > GameState.FindClothes && !moved)
        {
            moved = true;
            MoveToGrill();
        }
    }
    public void MoveToGrill()
    {
        moved = true;
        if(!npc.Positions.Contains(grillTransform)) {npc.Positions.Add(grillTransform); }
        if (!npc.Positions.Contains(grillTransform2)) { npc.Positions.Add(grillTransform2); }
        npc.IdleTime = 0.2f;
        npc.StopAtTheLastPoint = true;
    }

    private void PutOnProperSpotHack(Transform spot)
    {
        npc.Positions.Add(spot);
        npc.IdleTime = 0.1f;
        npc.InitialDestination(0);
        npc.StopAtTheLastPoint = true;
    }
}
