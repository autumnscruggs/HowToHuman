using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer : MonoBehaviour
{
    private PauseMenu pauseMenu;

    public List<AudioSource> MusicSources = new List<AudioSource>();
    public List<AudioSource> SFXSources = new List<AudioSource>();
    public List<AudioSource> DialogueSources = new List<AudioSource>();

    [SerializeField] private float MusicVolume;
    [SerializeField] private float SFXVolume;
    [SerializeField] private float DialogueVolume;

    private void Update()
    {
        MusicVolume = PauseData.MusicVolume;
        SFXVolume = PauseData.SFXVolume;
        DialogueVolume = PauseData.DialogueVolume;
    }


    private void Awake()
    {
        pauseMenu = this.GetComponent<PauseMenu>();
        pauseMenu.music.onValueChanged.AddListener(delegate { MusicValueChanged(); });
        pauseMenu.sFX.onValueChanged.AddListener(delegate { SFXValueChanged(); });
        pauseMenu.dialogue.onValueChanged.AddListener(delegate { DialogueValueChanged(); });
    }


    private void Start()
    {
        pauseMenu.music.value = PauseData.MusicVolume;
        pauseMenu.sFX.value = PauseData.SFXVolume;
        pauseMenu.dialogue.value = PauseData.DialogueVolume;

        AudioSource[] sources = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in sources)
        {
            if (source.gameObject.name == "Music Player")
            {
                MusicSources.Add(source);
            }
            else
            {
                if (source.gameObject.GetComponent<SubtitleManager>())
                {
                    DialogueSources.Add(source);
                }
                else
                {
                    SFXSources.Add(source);
                }
            }
        }

        if (MusicSources.Count > 0) { pauseMenu.music.value = MusicSources[0].volume; }
        if (SFXSources.Count > 0) { pauseMenu.sFX.value = SFXSources[0].volume; }
        if (DialogueSources.Count > 0) { pauseMenu.dialogue.value = DialogueSources[0].volume; }
    }

    private void MusicValueChanged()
    {
        PauseData.MusicVolume = pauseMenu.music.value;

        if (MusicSources.Count > 0)
        {
            foreach (AudioSource source in MusicSources)
            {
                source.volume = PauseData.MusicVolume;
            }
        } 
    }

    private void SFXValueChanged()
    {
        PauseData.SFXVolume = pauseMenu.sFX.value;

        if (SFXSources.Count > 0)
        {
            foreach (AudioSource source in SFXSources)
            {
                source.volume = PauseData.SFXVolume;
            }
        }  
    }

    private void DialogueValueChanged()
    {
        PauseData.DialogueVolume = pauseMenu.dialogue.value;

        if (DialogueSources.Count > 0)
        {
            foreach (AudioSource source in DialogueSources)
            {
                source.volume = PauseData.DialogueVolume;
            }
        }  
    }
}
