using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionCameraManager : MonoBehaviour
{

    public static EventHandler DetectionStarted;
    public static EventHandler DetectionEnded;

    private bool startEventTriggered = false;
    private bool endEventTriggered = false;

    // Use this for initialization
    public GameObject PlayerCamera, DetectionCamera, Bigfoot;
    [HideInInspector]
    public GameObject CameraStart, CameraEnd;
    private LearningTriggerAndNPCSpeechMeter LearningAndSpeechManager;
    public enum DetectionCameraState { Inactive, Active };
    public DetectionCameraState CameraState;

    public float RotateSpeed;
    public float speed;
    public float CameraStayTimer;
    private float CameraStayTimerSet;

    private bool CameraStartSet;
    private bool CameraEndSet;
    [HideInInspector]
    public NPC[] NPCS;
    [HideInInspector]
    public List<NPC> ListofNPCS;
    public GameObject CameraPositionPrefab;

    private List<GameObject> ListofCameraPositions;
    private bool facingBigfoot = false;

    void Start()
    {
        CameraStartSet = false;
        CameraEndSet = false;
        LearningAndSpeechManager = FindObjectOfType<LearningTriggerAndNPCSpeechMeter>();
        DetectionCamera.gameObject.SetActive(false);
        CameraState = DetectionCameraState.Inactive;
        NPCS = FindObjectsOfType<NPC>();
        ListofNPCS.AddRange(NPCS);
        AddCameraPositions();
    }

    // Update is called once per frame
    void Update()
    {
        switch (CameraState)
        {
            case DetectionCameraState.Inactive:

                CameraStartSet = false;
                CameraEndSet = false;

                CameraStayTimerSet = CameraStayTimer;
                if (!facingBigfoot)
                {
                    DeactivateDetectionCamera();
                    CheckDetectionState();
                }
                else
                {
                    CameraStart = Bigfoot.GetComponentInChildren<CameraStart>().gameObject;
                    CameraEnd = Bigfoot.GetComponentInChildren<CameraEnd>().gameObject;
                    CameraState = DetectionCameraState.Active;
                }
                break;
            case DetectionCameraState.Active:
                Bigfoot.GetComponent<PlayerMovement>().CanMove = false;
                ActivateDetectionCamera();
                CameraMove();
                break;
        }
    }

    public void ManualActivation(GameObject npc)
    {
        this.gameObject.transform.parent = npc.transform;
        CameraStart = npc.GetComponentInChildren<CameraStart>().gameObject;
        CameraEnd = npc.GetComponentInChildren<CameraEnd>().gameObject;
        NPC.DetectedBigfootEvent(npc.GetComponent<NPC>(), EventArgs.Empty);
        CameraState = DetectionCameraState.Active;
    }

    public void CheckDetectionState()
    {
        for (int i = 0; i < ListofNPCS.Count; i++)
        {
            if (ListofNPCS[i].TriggerSawNakedBigfoot == true)
            {
                this.gameObject.transform.parent = ListofNPCS[i].gameObject.transform;

                CameraStart = ListofCameraPositions[i].GetComponentInChildren<CameraStart>().gameObject;
                CameraEnd = ListofCameraPositions[i].GetComponentInChildren<CameraEnd>().gameObject;

                NPC.DetectedBigfootEvent(ListofNPCS[i], EventArgs.Empty);

                CameraState = DetectionCameraState.Active;
            }
        }

    }

    private void AddCameraPositions()
    {
        ListofCameraPositions = new List<GameObject>();

        for (int i = 0; i < ListofNPCS.Count; i++)
        {
            GameObject CameraPosition = Instantiate(CameraPositionPrefab, ListofNPCS[i].transform);
            CameraPosition.transform.parent = ListofNPCS[i].transform;
            ListofCameraPositions.Add(CameraPosition);
        }

    }

    private void ActivateDetectionCamera()
    {
        if (DetectionStarted != null && !startEventTriggered)
        {
            DetectionStarted(this, EventArgs.Empty);
            startEventTriggered = true;
            CutsceneManagement.InCutscene = true;
        }
        //Debug.Log("He knows the way!");
        PlayerCamera.SetActive(false);
        //FakePlayerCamera.SetActive(false);
        DetectionCamera.SetActive(true);
        CameraState = DetectionCameraState.Active;
    }

    private void DeactivateDetectionCamera()
    {
        //Debug.Log("Action is coming!");
        PlayerCamera.SetActive(true);
        //FakePlayerCamera.SetActive(false);
        DetectionCamera.SetActive(false);
        CameraState = DetectionCameraState.Inactive;
    }

    private void CameraMove()
    {
        SetCameraStart();

        //Initiate Camera Movement
        if (CameraEndSet != true)
        {
            float Dist = Vector3.Distance(transform.position, CameraEnd.transform.position);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, CameraEnd.transform.position, step);

            if (Dist <= 0.1)
            {
                CameraEndSet = true;
            }
        }

        else if (CameraEndSet == true)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            CameraStayTimerSet -= Time.deltaTime;

            if (CameraStayTimerSet <= 0)
            {
                if (!facingBigfoot) { facingBigfoot = true; }
                else
                {
                    if (DetectionEnded != null && !endEventTriggered)
                    {
                        DetectionEnded(this, System.EventArgs.Empty);
                        CutsceneManagement.InCutscene = false;
                        endEventTriggered = true;
                        startEventTriggered = false;
                    }
                    //CheckPointManager.Instance.ResetScene();
                }

                CameraState = DetectionCameraState.Inactive;
            }
        }

    }

    public void RotateTowards(Transform target)
    {
        if (CameraState == DetectionCameraState.Active)
        {
            this.transform.LookAt(CameraEnd.transform);
        }

    }
    public void SetCameraStart()
    {
        //Set Camera Starting Location
        if (CameraStartSet == false)
        {
            this.gameObject.transform.position = this.CameraStart.transform.position;
            this.gameObject.transform.parent = this.gameObject.transform;
            this.RotateTowards(CameraStart.transform);
            this.CameraStartSet = true;
        }
    }
    public void SetCameraEnd()
    {
        //Set Camera Starting Location
        if (CameraEndSet == false)
        {
            this.gameObject.transform.position = this.gameObject.transform.position;
            this.RotateTowards(CameraEnd.transform);
            this.CameraEndSet = true;
        }
    }
}
