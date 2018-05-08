using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RemoveCloths : MonoBehaviour
{
    public GameObject Shirt, Ring, Pants, Disguise;
    public AudioClip OutfitOnSound;
    private AudioSource ClothesSoundPlayer;

    private bool SoundPlayed;

    private SubtitleManager subtitles;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(CheckpointValues.CurrentState > GameState.FindClothes)
        {
            //Ring.SetActive(false);
            Destroy(Ring);
            Destroy(Shirt);
            Destroy(Pants);
            Disguise.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void Start()
    {
        ClothesSoundPlayer = GetComponent<AudioSource>();
        subtitles = GameObject.FindObjectOfType<SubtitleManager>().GetComponent<SubtitleManager>();
    }

    public void RemoveClothing()
    {
        Destroy(Ring);
        Destroy(Shirt);
        Destroy(Pants);
        PlayAudio();

    }

    public void PlayAudio()
    {
        if (SoundPlayed == false)
        {
            subtitles.PlaySubtitle(6);
            ClothesSoundPlayer.PlayOneShot(OutfitOnSound);
            SoundPlayed = true;
        }
        
    }


}
