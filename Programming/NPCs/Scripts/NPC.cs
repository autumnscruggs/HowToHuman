using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class NPC : MonoBehaviour
{
    //Inspector
    [SerializeField] private bool canDetectBigfoot = false;
    [Header("SphereCast Fields")]
    [SerializeField]
    private float spherecastDistance = 20;
    [SerializeField] private float spherecastRadius = 1;
    [SerializeField] private float eyeOffset = 0.5f;
    [SerializeField] private bool drawRaycastInScene = true;

    private Vector3 origin;
    private Vector3 direction;

    [Header("Hit Information")]
    [SerializeField]
    private bool displayHitInConsole = false;
    [SerializeField] private GameObject hitObject;
    [SerializeField] private float hitDistance;

    private bool triggerSawNakedBigfoot = false;

    public bool TriggerSawNakedBigfoot { get { return triggerSawNakedBigfoot; } }
    public void TriggerDetection() { triggerSawNakedBigfoot = true; }

    //Properties/Events
    public bool CanDetectBigfoot { get { return canDetectBigfoot; } private set { canDetectBigfoot = value; } }
    public static EventHandler DetectedBigfootEvent;
    public EventHandler SawDisguisedBigfootEvent;


    private void Update()
    {
        origin = new Vector3(this.transform.position.x, this.transform.position.y + eyeOffset, this.transform.position.z);
        direction = this.transform.forward;
        if (CanDetectBigfoot) { DetectionRaycast(); }
    }

    private void OnDrawGizmos()
    {

#if UNITY_EDITOR
        //The following code will run in edit mode
        if (!Application.isPlaying)
        {
            origin = new Vector3(this.transform.position.x, this.transform.position.y + eyeOffset, this.transform.position.z);
            direction = this.transform.forward;
            hitDistance = spherecastDistance;
        }
#endif

        if (drawRaycastInScene)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(origin, origin + direction * hitDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin + direction * hitDistance, spherecastRadius);
        }
    }


    private void DetectionRaycast()
    {
        RaycastHit hit;

        if (Physics.SphereCast(origin, spherecastRadius, direction, out hit, spherecastDistance))
        {
            hitObject = hit.collider.gameObject;
            hitDistance = hit.distance;

            if (displayHitInConsole) { Debug.Log("NPC: " + this.gameObject.name + " // Just Hit: " + hit.collider.gameObject.name); }

            Bigfoot bigfoot = hit.transform.gameObject.GetComponent<Bigfoot>();
            if (bigfoot != null) //if hit bigfoot
            {
                if (bigfoot.DisguiseState == BigfootDisguiseState.Naked)
                {
                    //CheckPointManager.Instance.ResetScene();
                    if (DetectedBigfootEvent != null && !triggerSawNakedBigfoot) { DetectedBigfootEvent(this, EventArgs.Empty); triggerSawNakedBigfoot = true; }
                }
                else
                {
                    if (SawDisguisedBigfootEvent != null)
                    { SawDisguisedBigfootEvent(this, EventArgs.Empty);}
                }
            }
        }
        else
        {
            hitDistance = spherecastDistance;
            hitObject = null;
        }
    }
}
