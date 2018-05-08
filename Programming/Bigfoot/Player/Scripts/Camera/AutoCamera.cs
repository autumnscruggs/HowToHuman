using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCamera : MonoBehaviour
{
    public Transform Container;
    public PlayerInput input;
    public float rotateSpeedModifier = 1;
    public bool CanRotate = false;

    public float newDistance = 3.5f;
    public float newHeight = 1.7f;

    [SerializeField] private float originalDist;
    [SerializeField] private float originalHeight;

    private Vector2 rotateDirection;
    private Quaternion original;

    private ThirdPersonCamera tpCam;
    private PlayerMovement movement;
    private Transform player;

    private float rotateSpeed = 0;



    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        tpCam = this.gameObject.GetComponent<ThirdPersonCamera>();

        originalDist = tpCam.defaultDistance;
        originalHeight = tpCam.height;

        player = input.transform;
        original = player.rotation;
        movement = input.gameObject.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        rotateSpeed = movement.TotalSpeed * rotateSpeedModifier;

        if (CanRotate)
        {
            GetRotateDirection();
            //this.transform.rotation = Quaternion.Euler(original.x, this.transform.rotation.y, original.z);
            player.Rotate(new Vector3(0, rotateDirection.x, 0) * rotateSpeed * Time.deltaTime);
            //player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.y, 0);
            //Container.localEulerAngles = new Vector3(input.transform.root.localEulerAngles.x,
            //    input.transform.localEulerAngles.y, input.transform.root.localEulerAngles.z);

        }
    }

    private void LateUpdate()
    {
        if(player.localEulerAngles.x != 0) { player.localEulerAngles = new Vector3(0, player.localEulerAngles.y, player.localEulerAngles.z); }
        if (player.localEulerAngles.z != 0) { player.localEulerAngles = new Vector3(player.localEulerAngles.x, player.localEulerAngles.y, 0); }
    }

    public void SetCameraValues(float xRot, float dist, float height)
    {
        tpCam.XValue = xRot;
        tpCam.defaultDistance = dist;
        tpCam.height = height;
    }

    public void ReturnToDefaultValues()
    {
        tpCam.XValue = 0;
        tpCam.defaultDistance = newDistance;
        tpCam.height = newHeight;
    }

    public void ResetContainer(bool on)
    {
        if(player == null || tpCam == null) { Setup(); }

        if(on)
        {
            //Container.rotation = input.transform.rotation;
            //this.transform.rotation = Quaternion.Euler(0,0,0);
            //p[.rotation = Quaternion.Euler(0, 0, 0);
            //player.rotation = this.transform.rotation;
            tpCam.XValue = 10;
            tpCam.defaultDistance = newDistance;
            tpCam.height = newHeight;
        }
        else
        {
            //input.transform.rotation = Container.rotation;
            tpCam.XValue = 0;
            player.rotation = original;
            tpCam.defaultDistance = originalDist;
            tpCam.height = originalHeight;
        }
    }

    private void GetRotateDirection()
    {
        if(input.LeftOrRightMovementInputPushed)
        {
            rotateDirection.x = input.InputDirection.x;
        }
        else
        {
            rotateDirection.x = 0;
        }

        if (input.ForwardOrBackwardMovementInputPushed)
        {
            rotateDirection.y = input.InputDirection.y;
        }
        else
        {
            rotateDirection.y = 0;
        }

    }
}
