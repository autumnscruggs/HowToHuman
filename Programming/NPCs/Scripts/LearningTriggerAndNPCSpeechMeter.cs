using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningTriggerAndNPCSpeechMeter : MonoBehaviour
{
    private ProgressiveAgent progression;
    private NPC parent;
    private NPCMovement movement;
    private PhoneSwitcher switcher;
    private BoxCollider parentTrigger;
    private SubtitleManager sManager_ref;
    public float MaxTimesSeen = 3;
    public float SeeBigfootDelay = 6;
    public float TimesSeen;
    public System.EventHandler SeenEvent;

    private bool canSeeBigfoot = false;
    private bool seeState = false;
    public bool startTimer = false;
    public float seeBigfootTimer = 0;

    public bool inSpaceTrigger { get; set; }

    public Transform beginningNPCSpot;

    private void Awake()
    {
        parent = this.transform.parent.GetComponentInChildren<NPC>();
        switcher = parent.GetComponentInChildren<PhoneSwitcher>();
        parent.SawDisguisedBigfootEvent += AddTimesSeen;
        SeenEvent += ReactToBigfootMinor;
        movement = parent.GetComponent<NPCMovement>();
        BoxCollider[] colls = parent.GetComponents<BoxCollider>();
        parentTrigger = System.Array.Find(colls, x => x.isTrigger);
        progression = this.GetComponent<ProgressiveAgent>();
        sManager_ref = FindObjectOfType<SubtitleManager>().GetComponent <SubtitleManager>();
    }

    private void OnDisable()
    {
        parent.SawDisguisedBigfootEvent -= AddTimesSeen;
        SeenEvent -= ReactToBigfootMinor;
    }

    private void Update()
    {
        this.transform.position = parent.transform.position;

        if(startTimer) { ViewBigfootTimer(); }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();

            float distance = Vector3.Distance(parentTrigger.transform.position, pm.transform.position);
            int testOffset = 2;

            if (distance < (parentTrigger.size.x - testOffset))
            {
                inSpaceTrigger = true;
                //if (canSeeBigfoot && movement.MoveState == MovementStates.Moving) { ViewBigfootTimer();  }
                if (canSeeBigfoot) { progression.TestProgression(pm); }
            }
            else
            {
                inSpaceTrigger = false;
                if (canSeeBigfoot) { progression.TestProgression(pm);  }
            }
        }
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            inSpaceTrigger = false;
            //startTimer = false;
        }
    }


    private void ViewBigfootTimer()
    {
        if (!seeState)
        {
            AddTimesSeen();
            seeState = true;
        }
        else
        {
            seeBigfootTimer += Time.deltaTime;
            if (seeBigfootTimer >= SeeBigfootDelay)
            {
                seeBigfootTimer = 0;
                seeState = false;
                startTimer = false;
            }
        }
    }


    private void AddTimesSeen()
    {
        if(!seeState)
        {
            if (TimesSeen < MaxTimesSeen)
            {
                TimesSeen++;
                if(TimesSeen >= MaxTimesSeen)
                {
                    parent.TriggerDetection(); progression.fillSpeed = 0;
                }
            }
            else { parent.TriggerDetection(); progression.fillSpeed = 0; }
            SeenEvent(this, System.EventArgs.Empty);
        }
    }

    private void AddTimesSeen(object sender, System.EventArgs e)
    {
        //Debug.Log("Saw the foots // CanSee - " + canSeeBigfoot);
        if (TimesSeen <= MaxTimesSeen && canSeeBigfoot) { startTimer = true; }
    }

    public void StartMiniGame()
    {
        if (!canSeeBigfoot) { StartCoroutine(StartDelay()); }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);
        canSeeBigfoot = true;
    }

    IEnumerator StopDelay()
    {
        canSeeBigfoot = false;

        if (movement != null)
        {
            //Debug.Log("In coroutine");

            movement.Positions.Clear();
            movement.RandomizePositions = false;
            movement.RotateAtStoppingPoint = false;
            movement.Positions.Add(beginningNPCSpot);
            //yield return new WaitForEndOfFrame();
            //movement.StopAtTheLastPoint = true;

            while(true)
            {
                float dist = Vector3.Distance(movement.gameObject.transform.position, beginningNPCSpot.gameObject.transform.position);
                if (dist <= 2.3f)
                {
                    movement.StopAtTheLastPoint = true;
                    movement.CanMove = false;
                    //movement.PauseMovement(true);
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void StopAtLastPoint()
    {
        if (canSeeBigfoot) { StartCoroutine(StopDelay()); }
    }

    public void ValPhoneCallEnd()
    {
        sManager_ref.TurnOffSubtitleImmediately();
        sManager_ref.PlaySubtitle(32);
    }

    public void ReactToBigfootMinor(object Sender, System.EventArgs e)
    {
        sManager_ref.TurnOffSubtitleImmediately();
        sManager_ref.PlaySubtitle(29);
    }
}
