using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadResponse : MonoBehaviour
{
    public int ScriptID1,ScriptID2,ScriptID3;
    private int scriptID;
    private SubtitleManager subtitles;
    private UpdateObjective objectiveRef;

    // Use this for initialization
    void Start () {
        subtitles = FindObjectOfType<SubtitleManager>().GetComponent<SubtitleManager>();
        objectiveRef = FindObjectOfType<UpdateObjective>();
    }
	
	// Update is called once per frame
	void Update () {
        SetDialogueUpdate();
	}

    public void SetDialogueUpdate()
    {
        if (objectiveRef.CurrentObjective == GameState.GoToBusStation)
        {
            scriptID = 1;
        }
        else if (objectiveRef.CurrentObjective == GameState.FindFood)
        {
            scriptID = 2;
        }
        else if (objectiveRef.CurrentObjective == GameState.GoToDriverAfterBurger)
        {
            scriptID = 3;
        }

        
    }

    public void TalkToBigfoot()
    {
        switch (scriptID)
        {
            case 1:
                subtitles.PlaySubtitle(ScriptID1);
                break;
            case 2:
                subtitles.PlaySubtitle(ScriptID2);
                break;
            case 3:
                subtitles.PlaySubtitle(ScriptID3);
                break;
        }
    }
}
