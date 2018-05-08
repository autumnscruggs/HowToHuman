using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    private enum CameraAnimState { None, AnimatingInFrontOfBigfoot, AnimatingBehindBigfoot }
    private CameraAnimState state;

    public Bigfoot bigfoot;
    public Transform animationCameraPos;
    private Camera thisCam;
    public ThirdPersonCamera tpCam;

    public CameraTransition ToIdle;
    public CameraTransition ToMoving;

    private float cTime = 0;

    private bool setCameraBackToMain = false;

    private Vector3 newRotation;

    private float distance = 0;
    private Vector3 startPos;
    private Quaternion startRotation;
    public Vector3 rotationOffset = new Vector3(0, 5, 0);

    private void OnEnable()
    {
        thisCam = this.GetComponent<Camera>();
        newRotation = new Vector3(0, 180, 0);
        SetCam(0);
    }

    private void Start()
    {
        this.transform.position = tpCam.transform.position;
    }

    private void SetCam(int cam)
    {
        if(cam == 0)
        {
            thisCam.enabled = false;
            tpCam.GetComponent<Camera>().enabled = true;
            tpCam.CanFollow = true;
        }
        else
        {
            thisCam.enabled = true;
            tpCam.GetComponent<Camera>().enabled = false;
            tpCam.CanFollow = false;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case CameraAnimState.None:
                break;
            case CameraAnimState.AnimatingInFrontOfBigfoot:
                distance = Vector3.Distance(this.transform.position, animationCameraPos.transform.position);
                MoveTowards(ToIdle, animationCameraPos);
                RotateTowards();
                break;
            case CameraAnimState.AnimatingBehindBigfoot:
                distance = Vector3.Distance(this.transform.position, tpCam.transform.position);
                MoveTowards(ToMoving, tpCam.transform);
                RotateTowards();
                break;
            default:
                break;
        }
    }

    public void AnimateInFrontOfBigfoot()
    {
        distance = Vector3.Distance(tpCam.transform.position, animationCameraPos.transform.position);
        this.transform.position = tpCam.transform.position;
        startPos = tpCam.transform.position;
        startRotation = this.transform.rotation;
        SetCam(1);
        setCameraBackToMain = false;
        state = CameraAnimState.AnimatingInFrontOfBigfoot;
    }

    public void ReturnToBigfootBack()
    {
        distance = Vector3.Distance(this.transform.position, tpCam.transform.position);
        startPos = this.transform.position;
        startRotation = this.transform.rotation;
        setCameraBackToMain = true;
        state = CameraAnimState.AnimatingBehindBigfoot;
    }

    public void MoveTowards(CameraTransition transition, Transform pos)
    { 
        if (distance > 1)
        {
            // calculate current time within our lerping time range
            cTime += Time.deltaTime * transition.Speed;
            // calculate straight-line lerp position:
            Vector3 currentPos = Vector3.Lerp(startPos, pos.transform.position, cTime);
            // add a value to Y, using Sine to give a curved trajectory in the Y direction
            currentPos.x += transition.XArc * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);
            currentPos.z += transition.ZArc * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);
            // finally assign the computed position to our gameObject:
            transform.position = currentPos;
        }
        else
        {
            cTime = 0;
        }
    }

    public void RotateTowards()
    {
        if (distance > 1)
        {
            Vector3 lookAtPos = bigfoot.transform.position + rotationOffset;
            this.transform.LookAt(lookAtPos);
        }
        else
        {
            if(setCameraBackToMain) { SetCam(0); }
            state = CameraAnimState.None;
        }
    }
}

[System.Serializable]
public class CameraTransition
{
    public float Speed;
    public float XArc;
    public float ZArc;
}
