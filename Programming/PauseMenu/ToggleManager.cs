using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleManager : MonoBehaviour
{
    private PauseMenu pauseMenu;
    private void Awake()
    {
        pauseMenu = this.GetComponent<PauseMenu>();
        pauseMenu.subtitle.onValueChanged.AddListener(delegate { SubtitleChanged(); });
        pauseMenu.tutorial.onValueChanged.AddListener(delegate { TutorialChanged(); });
        pauseMenu.camera.onValueChanged.AddListener(delegate { CameraChanged(); });

        pauseMenu.subtitle.isOn = PauseData.ShowSubtitles;
        pauseMenu.tutorial.isOn = PauseData.ShowTutorials;
        pauseMenu.camera.isOn = PauseData.ManualCamera;
    }

    public void SubtitleChanged()
    {
        PauseData.ShowSubtitles = pauseMenu.subtitle.isOn;
    }

    public void TutorialChanged()
    {
        PauseData.ShowTutorials = pauseMenu.tutorial.isOn;
    }

    public void CameraChanged()
    {
        PauseData.ManualCamera = pauseMenu.camera.isOn;
    }
}
