using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchColliderFix : MonoBehaviour
{
    private Vector3 originalCenter;
    private float originalHeight;

    public Vector3 newCenter;
    public float newHeight;

    private void OnEnable()
    {
        originalCenter = this.GetComponent<CapsuleCollider>().center;
        originalHeight = this.GetComponent<CapsuleCollider>().height;
        this.GetComponent<PlayerMovement>().WalkStateChanged += OnCrouch;
    }

    private void OnDisable()
    {
        this.GetComponent<PlayerMovement>().WalkStateChanged -= OnCrouch;
    }

    private void OnCrouch(WalkStateArgs e)
    {
        if(e.WalkState == WalkStates.Crouch)
        {
            this.GetComponent<CapsuleCollider>().center = newCenter;
            this.GetComponent<CapsuleCollider>().height = newHeight;
        }
        else
        {
            this.GetComponent<CapsuleCollider>().center = originalCenter;
            this.GetComponent<CapsuleCollider>().height = originalHeight;
        }
    }
}
