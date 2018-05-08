using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    private AudioSource Musicplayer;
    public AudioClip NewMusic;

    public bool PlayOnce;
    private bool IsPlayed;

	// Use this for initialization
	void Start ()
    {
        Musicplayer = GameObject.Find("Music Player").GetComponent<AudioSource>();

    }

    public void ChangeAudio()
    {
        if (IsPlayed == false)
        {
            Musicplayer.Stop();
            Musicplayer.clip = NewMusic;
            Musicplayer.Play();
            IsPlayed = true;
        }
        else
        {
            if (PlayOnce == false)
            {
                Musicplayer.Stop();
                Musicplayer.clip = NewMusic;
                Musicplayer.Play();
            }
        }
        
    }
}
