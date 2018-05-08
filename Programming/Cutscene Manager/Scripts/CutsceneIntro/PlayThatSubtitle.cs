using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayThatSubtitle : MonoBehaviour
{
    private SubtitleManager sManager_ref;
    public float TimeDelay;
    private float timer;
    private bool StartPlaying = false;
    private bool SubtitleStarting = false;

    // Use this for initialization
    void Start ()
    {
        timer = 0;
        StartPlaying = false;
        SubtitleStarting = false;
        sManager_ref = FindObjectOfType<SubtitleManager>().GetComponent<SubtitleManager>();
    }
    
    // Update is called once per frame
    void Update ()
    {
        RunSubtitles();
    }

    public void RunSubtitles()
    {
        if (StartPlaying == false)
        {
            timer += Time.deltaTime;

            if (timer >= TimeDelay)
            {
                StartPlaying = true;
                timer = TimeDelay;
            }
        }
        else
        {
            if (SubtitleStarting == false)
            {
                sManager_ref.PlaySubtitle(0);
                SubtitleStarting = true;
            }
            
        }
    }

}
