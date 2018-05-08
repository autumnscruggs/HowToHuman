using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnBoolChanged(bool newValue);
public delegate void OnFloatChanged(float newValue);

public class PauseData 
{
    public static event OnBoolChanged ShowSubtitlesChanged;
    public static event OnBoolChanged ShowTutorialsChanged;
    public static event OnBoolChanged ManualCameraChanged;

    public static event OnFloatChanged MusicVolumeChanged;
    public static event OnFloatChanged DialogueVolumeChanged;
    public static event OnFloatChanged SFXVolumeChanged;

    private static bool showSubtitles = true;
    public static bool ShowSubtitles
    {
        get { return showSubtitles; }
        set
        {
            showSubtitles = value;
            if(ShowSubtitlesChanged != null) { ShowSubtitlesChanged(showSubtitles); }
        }
    }

    private static bool showTutorials = true;
    public static bool ShowTutorials
    {
        get { return showTutorials; }
        set
        {
            showTutorials = value;
            if (ShowTutorialsChanged != null) { ShowTutorialsChanged(showTutorials); }
        }
    }

    private static bool manualCamera = true;
    public static bool ManualCamera
    {
        get { return manualCamera; }
        set
        {
            manualCamera = value;
            if (ManualCameraChanged != null) { ManualCameraChanged(manualCamera); }
        }
    }


    private static float musicVolume = 0.3f;
    public static float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            if (MusicVolumeChanged != null) { MusicVolumeChanged(musicVolume); }
        }
    }

    private static float dialogueVolume = 0.9f;
    public static float DialogueVolume
    {
        get { return dialogueVolume; }
        set
        {
            dialogueVolume = value;
            if (DialogueVolumeChanged != null) { DialogueVolumeChanged(dialogueVolume); }
        }
    }

    private static float sFXVolume = 1f;
    public static float SFXVolume
    {
        get { return sFXVolume; }
        set
        {
            sFXVolume = value;
            if (SFXVolumeChanged != null) { SFXVolumeChanged(sFXVolume); }
        }
    }
}
