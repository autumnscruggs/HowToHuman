using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class RotateWithCamera : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private float prevAngle;

    [HideInInspector] public Vector3 targetDirection;
    protected Quaternion targetRotation;
    [HideInInspector] public Quaternion freeRotation;
    [HideInInspector] public bool keepDirection;
    [HideInInspector] public bool horizontalDirectionKeep;

    [HideInInspector] public float speed;

    private CameraStyleSwitcher cameraSwitcher;
    private ThirdPersonCamera tpCamera;
    private PlayerInput tpInput;
    private PlayerMovement tpMovement;


    public GameObject model;

    private void Start()
    {
        tpCamera = GameObject.FindObjectOfType<ThirdPersonCamera>();
        cameraSwitcher = tpCamera.gameObject.GetComponent<CameraStyleSwitcher>();
        if (tpCamera) tpCamera.SetMainTarget(this.transform);

        tpInput =this.GetComponent<PlayerInput>();
        tpMovement = this.GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        if (Time.timeScale > 0 && tpMovement.CanMove)
        {
            if(cameraSwitcher.CamStyle == CameraStyle.Manual)
            {
                if (InputManager.ControllerType == ControlType.Gamepad) { keepDirection = tpInput.ForwardOrBackwardMovementInputPushed; }
                else { keepDirection = tpInput.ForwardOrBackwardMovementInputPushed && !tpInput.LeftOrRightMovementInputPushed; }
                //if (tpMovement.CanMove && tpInput.MovementInputPushed) { RotateModelInDirectionOfKeyPushed(); }

                horizontalDirectionKeep = tpInput.LeftOrRightMovementInputPushed;
            }
            else
            {
                //RotateModelWithAnotherTransform(tpCamera.transform);
            }
            
        }
    }


    protected virtual void FixedUpdate()
    {
        CameraInput();
    }


    private void LateUpdate()
    {

    }

    protected virtual void CameraInput()
    {
        if (tpCamera == null)
            return;

        bool gamepad = false;
        float Y = 0;
        float X = 0;

        if (tpInput.Gamepad != null && tpInput.Gamepad.IsConnected && tpInput.Gamepad.IsMovingEitherThumbStick)
        {
            gamepad = true;
            Y += tpInput.Gamepad.GetStick_R().Y;
            X += tpInput.Gamepad.GetStick_R().X;
        }
        else
        {
            Y = MouseInput.MouseY();
            X = MouseInput.MouseX();
        }

        tpCamera.RotateCamera(X, Y, gamepad);

        // tranform Character direction from camera if not KeepDirection
        if (keepDirection)
        {
            //Debug.Log("Keep Direction");
            // rotate the character with the camera while strafing        
            RotateModelWithAnotherTransform(tpCamera.transform);
            RotateObjectWithAnotherTransform(tpCamera.transform);
        }
        else 
        {
            if(horizontalDirectionKeep)
            {
                if(tpInput.ForwardOrBackwardMovementInputPushed)
                {
                    if(tpInput.InputDirection.y > 0)
                    {
                        if (tpInput.InputDirection.x > 0)
                        {
                            SidewaysFacing(tpCamera.transform, true, true);
                        }
                        else if (tpInput.InputDirection.x < 0)
                        {
                            SidewaysFacing(tpCamera.transform, false, true);
                        }
                    }
                    else
                    {
                        if (tpInput.InputDirection.x > 0)
                        {
                            SidewaysFacing(tpCamera.transform, true, true, true);
                        }
                        else if (tpInput.InputDirection.x < 0)
                        {
                            SidewaysFacing(tpCamera.transform, false, true, true);
                        }
                    }

                }
                else
                {
                    if (tpInput.InputDirection.x > 0)
                    {
                        SidewaysFacing(tpCamera.transform, true, false);
                    }
                    else if (tpInput.InputDirection.x < 0)
                    {
                        SidewaysFacing(tpCamera.transform, false, false);
                    }
                }
            }
        }
    }

    public virtual void RotateObjectWithAnotherTransform(Transform referenceTransform)
    {
        var newRotation = new Vector3(this.transform.eulerAngles.x, referenceTransform.eulerAngles.y, this.transform.eulerAngles.z);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.fixedDeltaTime);
        //targetRotation = this.transform.rotation;
    }

    public virtual void RotateModelWithAnotherTransform(Transform referenceTransform)
    {
        prevAngle = model.transform.eulerAngles.y;
        var newRotation = new Vector3(model.transform.eulerAngles.x, referenceTransform.eulerAngles.y, model.transform.eulerAngles.z);
        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.fixedDeltaTime);
        targetRotation = model.transform.rotation;
    }

    public virtual void SidewaysFacing(Transform referenceTransform, bool left, bool diagonal, bool backwards = false)
    {
        float angle = 0;
        if (diagonal) { if (!backwards) { angle = left ? 40 : -40; } else { angle = !left ? 40 : -40; } }
        else
        {
             angle = left ? 90 : -90; 
        }

        var newRotation = new Vector3(this.transform.eulerAngles.x, referenceTransform.eulerAngles.y + angle, this.transform.eulerAngles.z);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.fixedDeltaTime);
        //targetRotation = model.transform.rotation;
    }

    //public virtual void RotateToTarget(Transform target)
    //{
    //    if (target)
    //    {
    //        Quaternion rot = Quaternion.LookRotation(target.position - transform.position);
    //        var newPos = new Vector3(transform.eulerAngles.x, rot.eulerAngles.y, transform.eulerAngles.z);
    //        targetRotation = Quaternion.Euler(newPos);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newPos), rotationSpeed * Time.deltaTime);
    //    }
    //}

    public void RotateModelInDirectionOfKeyPushed()
    {
        if(!keepDirection)
        {
           // Debug.Log("NOT Keep Direction");

            float angle = 0;
            if (tpInput.InputDirection.x >= 0.5f && tpInput.InputDirection.y == 0) { angle = 90; }
            else if (tpInput.InputDirection.x <= -0.5f && tpInput.InputDirection.y == 0) { angle = -90; }
            else if (tpInput.InputDirection.x >= 0.5f && tpInput.InputDirection.y >= 0.5f) { angle = 40; }
            else if (tpInput.InputDirection.x <= -0.5f && tpInput.InputDirection.y >= 0.5f) { angle = -40; }
            else if (tpInput.InputDirection.x <= -0.5f && tpInput.InputDirection.y <= -0.5f) { angle = 40; }
            else if (tpInput.InputDirection.x >= 0.5f && tpInput.InputDirection.y <= -0.5f) { angle = -40; }
            else if(tpInput.InputDirection.x == 0 && tpInput.InputDirection.y != 0) { angle = 0; }

            var newRotation = new Vector3(model.transform.eulerAngles.x, prevAngle + angle, model.transform.eulerAngles.z);
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.fixedDeltaTime);
            targetRotation = model.transform.rotation;
        }
    }
}

