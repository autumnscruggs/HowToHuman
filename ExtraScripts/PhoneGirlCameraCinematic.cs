using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneGirlCameraCinematic : MonoBehaviour
{
    public GameObject BigfootCam, ValCam;

    public void ActivateValCam()
    {
        BigfootCam.SetActive(false);
        ValCam.SetActive(true);
    }

    public void DeActivateValCam()
    {
        BigfootCam.SetActive(true);
        ValCam.SetActive(false);
    }
}
