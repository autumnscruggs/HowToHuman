using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReactSound : MonoBehaviour
{
    private AudioSource NPCAudioSource;
    public AudioClip NPCSoundClip;

	// Use this for initialization
	void Start ()
    {
        NPCAudioSource = GetComponent<AudioSource>();
        //this.GetComponent<NPC>().SawDisguisedBigfootEvent += PlaySound; //This keeps playing way too much
	}

    private void OnEnable()
    {
        NPC.DetectedBigfootEvent += PlaySound;
    }

    private void OnDisable()
    {
        NPC.DetectedBigfootEvent -= PlaySound;
    }


    // Update is called once per frame
    void Update () {
		
	}

    void PlaySound(object sender, System.EventArgs e)
    {
        NPCAudioSource.PlayOneShot(NPCSoundClip);
    }
}
