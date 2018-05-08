using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disguise : MonoBehaviour
{
    public delegate void DisguiseTakenDelegate();
    public event DisguiseTakenDelegate DisguiseTaken;

    [SerializeField] private DisguiseTypes disguiseType = DisguiseTypes.Hunter;
    [SerializeField] protected bool canTakeDisguise = true;

    public void OnInteraction()
    {
        PressedEInTrigger();
        if (DisguiseTaken != null) { DisguiseTaken(); }
    }

    protected virtual void PressedEInTrigger()
    {
        DisguiseManager.Instance.PickUpDisguise(disguiseType);
        this.gameObject.SetActive(false);
    }

}
