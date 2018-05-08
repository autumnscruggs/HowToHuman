using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStyle { Manual, Auto }

public class CameraStyleSwitcher : MonoBehaviour
{
    private CameraStyle camStyle;
    public CameraStyle CamStyle
    {
         get { return camStyle; }
         private set { camStyle = value; }
    }

    private ThirdPersonCamera tpCam;
    private AutoCamera autoCam;



    private void Awake()
    {
        tpCam = this.GetComponent<ThirdPersonCamera>();
        autoCam = this.GetComponent<AutoCamera>();
    }

    private void Start()
    {
        OnBoolChanged(PauseData.ManualCamera);
        autoCam.ResetContainer(false);
    }

    private void OnEnable()
    {
        PauseData.ManualCameraChanged += OnBoolChanged;
    }
    private void OnDisable()
    {
        PauseData.ManualCameraChanged -= OnBoolChanged;
    }

    public void OnBoolChanged(bool manual)
    {
        CamStyle = manual ? CameraStyle.Manual : CameraStyle.Auto;
        OnStyleChanged();
    }

    private void OnStyleChanged()
    {
        switch (CamStyle)
        {
            case CameraStyle.Manual:
                tpCam.ResetCamera();
                autoCam.ResetContainer(false);
                tpCam.lockCamera = false;
                autoCam.CanRotate = false;
                break;
            case CameraStyle.Auto:
                tpCam.ResetCamera();
                autoCam.ReturnToDefaultValues();
                autoCam.ResetContainer(true);
                tpCam.lockCamera = true;
                autoCam.CanRotate = true;
                break;
            default:
                break;
        }
    }
}
