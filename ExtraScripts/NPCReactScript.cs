using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReactScript : MonoBehaviour {

	public int ScriptID1, ScriptID2;
	private SubtitleManager subtitles;
	private bool DialogueHappening;
	private NPC npc;
    private Bigfoot bigfoot_Ref;

	// Use this for initialization
	void Start ()
	{
		subtitles = FindObjectOfType<SubtitleManager>();
        bigfoot_Ref = FindObjectOfType<Bigfoot>();
		npc = this.GetComponent<NPC>();

	}

	private void OnEnable()
	{
		NPC.DetectedBigfootEvent += ReacttoNaked;
	}

	private void OnDisable()
	{
		NPC.DetectedBigfootEvent -= ReacttoNaked;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void ReacttoNaked(object sender, System.EventArgs e)
	{
		if((NPC)sender == npc)
		{
            if (bigfoot_Ref.DisguiseState == BigfootDisguiseState.Naked)
            {
                subtitles.PlaySubtitle(ScriptID1);
            }
            else
            {
                subtitles.PlaySubtitle(ScriptID2);
            }

            
		}
	}
}
