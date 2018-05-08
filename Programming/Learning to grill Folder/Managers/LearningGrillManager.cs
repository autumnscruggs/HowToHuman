using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LearningGrillManager : MonoBehaviour {

    [HideInInspector]
    public GameObject player;
    private Transform RespawnPoint;
    private ProgressionMeterManager pMM;
    private ArchingProjectileSpawner aPS;
    private NPC_Detection nPC_Detection;

    [HideInInspector] public float progressValue;
    [HideInInspector] public float progressFillAmount;
    public float fillSpeed;

    public float TutorialLength = 2f;
    public UnityEvent OnTutorialStart;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameComplete;

    public bool InBush { get; set; }
    public bool InStart { get; set; }

    [SerializeField] private int ingredientsRequired;
    public int IngredientsRequired { get { return ingredientsRequired; } }

    [SerializeField] private int ingredientsAcquired;
    public int IngredientsAcquired { get { return ingredientsAcquired; } }

    [SerializeField] private int strikes = 0;
    public int Strikes { get { return strikes; } }

    [SerializeField] private int maxStrikes = 3;
    public int MaxStrikes { get { return maxStrikes; } }


    public static System.EventHandler MissedIngredientEvent;

    public enum ProgressionStatus { Idle, Progressing, Completed, Failed }
    private ProgressionStatus theStatus;
    public ProgressionStatus ProgressStatus
    {
        get { return theStatus; }
        set
        {
            if(theStatus != value)
            {
                theStatus = value;

                if (theStatus == ProgressionStatus.Progressing)
                {
                    pMM.ShowProgressionMeter();
                }
                else if (theStatus == ProgressionStatus.Idle)
                {
                    pMM.HideProgressionMeter();
                }
                else if (theStatus == ProgressionStatus.Completed)
                {
                    pMM.HideProgressionMeter();
                }
                else
                {
                    pMM.ShowProgressionMeter();
                }
            }
        }
    }


    public enum GrillAbilities { Ignorant, Knowledgeable}
    public GrillAbilities grillAbilities;

    private float InitialPlayerBaseSpeed;
    private bool GameInProgress;

    private void Awake()
    {
        progressFillAmount = 100 / IngredientsRequired;
        player = FindObjectOfType<Bigfoot>().gameObject;
    }

    void Start()
    {
        pMM = FindObjectOfType<ProgressionMeterManager>();
        aPS = FindObjectOfType<ArchingProjectileSpawner>();
        RespawnPoint = GameObject.Find("RespawnPoint").GetComponent<Transform>();
        progressValue = 0;
        nPC_Detection = FindObjectOfType<NPC_Detection>();
    }

    void Update()
    {
        switch (ProgressStatus)
        {
            case ProgressionStatus.Idle:
                CheckGameStatus();
                break;
            case ProgressionStatus.Progressing:
                CheckGameStatus();
                break;
            case ProgressionStatus.Completed:
                ShowWinScreen();
                grillAbilities = GrillAbilities.Knowledgeable;
                ResetMinigame();
                break;
            case ProgressionStatus.Failed:
                aPS.PreventSpawning();
                GameInProgress = false;
                //RespawnBigFoot();
                break;
        }
    }
    public void MissedIngredient()
    {
        strikes++;

        MissedIngredientEvent(this, System.EventArgs.Empty);

        if (strikes >= maxStrikes)
        {
            aPS.StopAllThings();
            ProgressStatus = ProgressionStatus.Failed;
            pMM.ShowProgressionMeter();
            nPC_Detection.DetectPlayer();
        }
    }

    public void PickUpIngredient()
    {
        pMM.PlaySoundOneShot();
        FillProgressMeter();
        ingredientsAcquired++;
    }

    private void FillProgressMeter()
    {
        if (progressValue < 100)
        {
            progressValue += progressFillAmount;
            pMM.ProgressSlider.value = progressValue;
        }
    }

    private void CheckGameStatus()
    {
        if (progressValue >= 100)
        {
            ProgressStatus = ProgressionStatus.Completed;
        }

        if (aPS.SliderValue >= 3)
        {
            ProgressStatus = ProgressionStatus.Failed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null && ProgressStatus == ProgressionStatus.Progressing)
        {

            if (ProgressStatus == ProgressionStatus.Progressing)
            {
                aPS.spawningStatus = ArchingProjectileSpawner.SpawningStatus.Spawnning;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        aPS.spawningStatus = ArchingProjectileSpawner.SpawningStatus.Idle;
    }

    private void RespawnBigFoot()
    {
        if (ProgressStatus == ProgressionStatus.Failed && GameInProgress == true)
        {
            StopAllCoroutines();
            aPS.StopAllThings();
            nPC_Detection.DetectPlayer();
            ProgressStatus = ProgressionStatus.Idle;
            GameInProgress = false;
        }

        else if (ProgressStatus == ProgressionStatus.Completed && GameInProgress == false)
        {
            ProgressStatus = ProgressionStatus.Idle;
        }
    }

    private void ResetProjectileSpawner()
    {
        if (aPS.spawningStatus != ArchingProjectileSpawner.SpawningStatus.Idle)
        {
            
            aPS.spawningStatus = ArchingProjectileSpawner.SpawningStatus.Idle;

            List<ArchingProjectile> ListofProjectiles = new List<ArchingProjectile>();
            ListofProjectiles.AddRange(FindObjectsOfType<ArchingProjectile>());

            if (ListofProjectiles.Count > 0 && ListofProjectiles[0] != null)
            {
                for (int i = 0; i < ListofProjectiles.Count; i++)
                {
                    Destroy(ListofProjectiles[i].gameObject);
                }
            }

            aPS.SliderValue = 0;
        }
    }

    public void StartMiniGame()
    {
        StartCoroutine(StartDelay());
    }

    public void ResetMinigame()
    {
        aPS.SliderValue = 0;
        progressValue = 0;
        pMM.ResetProgress();
        pMM.ProgressSlider.value = progressValue;
        ResetProjectileSpawner();
        RespawnBigFoot();
        GameInProgress = false;
    }

    public void ConfirmEnd()
    {
        GameInProgress = false;
        //MiniGameWinScreen.gameObject.SetActive(false);
    }

    public void ShowWinScreen()
    {
        if (ProgressStatus == ProgressionStatus.Completed && GameInProgress == true 
            && grillAbilities != GrillAbilities.Knowledgeable)
        {
            //Debug.Log("On Game Complete");
            GameInProgress = false;
            OnGameComplete.Invoke();
            aPS.StopAllThings();
        }
    }

    public void HideDetectionSlider()
    {
        aPS.detectionSprite.gameObject.SetActive(false);
    }

    public void ShowDetectionMeter()
    {
        aPS.detectionSprite.gameObject.SetActive(true);
    }

    IEnumerator StartDelay()
    {
        if(PauseData.ShowTutorials)
        {
            OnTutorialStart.Invoke();

            player.GetComponent<PlayerMovement>().CanMove = false;

            yield return new WaitForSeconds(TutorialLength);
        }

        yield return null;


        player.GetComponent<PlayerMovement>().CanMove = true;

        GameInProgress = true;
        ProgressStatus = ProgressionStatus.Progressing;

        int StartPositionNum;
        StartPositionNum = Random.Range(0, 3);

        OnGameStart.Invoke();
        pMM.ResetProgress();
        aPS.spawningStatus = ArchingProjectileSpawner.SpawningStatus.Spawnning;
    }
}
