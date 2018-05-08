using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    private CheckPointManager cpManager_Ref;

    public int CheckpointNumber;
    private bool IsTriggered;

    public Transform checkpointSpot;
    private Vector3 checkpointObject;

    private void Start()
    {
        cpManager_Ref = FindObjectOfType<CheckPointManager>();
        checkpointObject = checkpointSpot.GetComponent<Transform>().position;
    }

    private void Update()
    {
        DeleteLastCheckPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == cpManager_Ref.Bigfoot)
        {
            print("Bigfoot has entered the trigger.");

            if (IsTriggered == false)
            {
                IsTriggered = true;
                cpManager_Ref.CurrentCheckpoint = CheckpointNumber;
                cpManager_Ref.checkpointValues_ref.ValuesForSpawn = checkpointObject;
                //cpManager_Ref.checkpointValues_ref.OutofStart = true;

                if (CheckpointNumber > 1)
                {
                    cpManager_Ref.checkpointValues_ref.GotClothing = true;
                }
            }
        }
    }

    void DeleteLastCheckPoint()
    {
        //When Bigfoot enters the next checkpoint, the one he's triggered last will be removed from play.
        if (CheckpointNumber < cpManager_Ref.CurrentCheckpoint)
        {
            Destroy(this.gameObject);
        }
    }


}
