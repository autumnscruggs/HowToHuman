using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerTutorialScript : MonoBehaviour
{
    public GameObject BigfootCam, ValCam;
    private bool isDone;

    public void ActivateBurCam()
    {
        if (isDone == false)
        {
            isDone = true;
            BigfootCam.SetActive(false);
            ValCam.SetActive(true);
            CutsceneManagement.InCutscene = true;
        }
        
    }

    public void DeActivateBurCam()
    {
        BigfootCam.SetActive(true);
        ValCam.SetActive(false);
        CutsceneManagement.InCutscene = false;
    }
}
