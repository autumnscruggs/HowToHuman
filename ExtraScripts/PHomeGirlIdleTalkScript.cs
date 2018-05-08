using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHomeGirlIdleTalkScript : MonoBehaviour
{
    private SubtitleManager sManager_Ref;
    private UpdateObjective uObjective_Ref;
    public int IdleTalk1, IdleTalk2, IdleTalk3, IdleTalk4, IdleTalk5;
    private int ScriptID;
	// Use this for initialization
	void Start ()
    {
        sManager_Ref = FindObjectOfType<SubtitleManager>();
        uObjective_Ref = FindObjectOfType<UpdateObjective>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void SelectScript()
    {
        ScriptID = Random.Range(0, 6);

        switch (ScriptID)
        {
            case 0:
            sManager_Ref.PlaySubtitle(IdleTalk1);
                break;
            case 1:
                sManager_Ref.PlaySubtitle(IdleTalk2);
                break;
            case 2:
                sManager_Ref.PlaySubtitle(IdleTalk3);
                break;
            case 3:
                sManager_Ref.PlaySubtitle(IdleTalk4);
                break;
            case 4:
                sManager_Ref.PlaySubtitle(IdleTalk5);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            if (uObjective_Ref.gameState == GameState.LearnToSpeak)
            {
                Destroy(this.gameObject);
            }
            else
            {
                SelectScript();
            }
            
        }
    }

}
