using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnterActions;
    public UnityEvent OnTriggerExitActions;
    public bool CanOnlyInvokeOnEnterOnce = true;
    public bool CanOnlyInvokeOnExitOnce = true;

    private bool onEnter = true;
    private bool onExit = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            if (onEnter) { OnTriggerEnterActions.Invoke(); }
            if (CanOnlyInvokeOnEnterOnce) { onEnter = false; }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            if (onExit) { OnTriggerExitActions.Invoke(); }
            if (CanOnlyInvokeOnExitOnce) { onExit = false; }
        }
    }
}
