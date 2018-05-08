using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public GameState NewGameState;
    private UpdateObjective updateObj;
    public List<StateLocation> Locations = new List<StateLocation>();

    private Bigfoot bigfoot;
    private DisguiseManager disguiseManager;

    public StartSpeech speechNPC;

    private void Awake()
    {
        updateObj = GameObject.FindObjectOfType<UpdateObjective>();
        bigfoot = GameObject.FindObjectOfType<Bigfoot>();
        disguiseManager = GameObject.FindObjectOfType<DisguiseManager>();
    }

    public void SetState()
    {
        StateLocation loc = Locations.Find(x => x.state == NewGameState);
        bigfoot.transform.position = loc.transform.position;

        updateObj.ChangeState(NewGameState);

        if (updateObj.CurrentObjective != GameState.FindClothes)
        {
            disguiseManager.PickUpDisguise(DisguiseTypes.Hunter);
        }
        else
        {
            disguiseManager.LoseDisguise();
        }

        if (updateObj.CurrentObjective > GameState.GoToDriverAfterSpeech)
        {
            speechNPC.CanBeInteractedWith = false;
        }
        else
        {
            speechNPC.CanBeInteractedWith = true;
        }
    }
}

[System.Serializable]
public class StateLocation
{
    public GameState state;
    public Transform transform;
}

