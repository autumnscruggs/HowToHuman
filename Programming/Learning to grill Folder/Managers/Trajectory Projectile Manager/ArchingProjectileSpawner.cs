using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchingProjectileSpawner : MonoBehaviour
{
    public SpriteRenderer detectionSprite;
    private float meterMaxHeight = 0.94f;
    public float meterFillAmount;
    public bool debugFinish = false;
    public int SliderValue { get; set; }

    public float arcSpeed;
    public float verticalArcSpeed;
    public float spawnerRotateSpeed = 12;

    [HideInInspector]
    public List<TargetPoint> TargetPoints;
    [HideInInspector]
    public LearningGrillManager LGM;
    private PlayerInput tpI;
    public List<GameObject> ProjectilePrefabs;
    public enum SpawningStatus { Idle, Spawnning }
    [HideInInspector]
    public SpawningStatus spawningStatus;
    public float SetSpawnTimer;
    private float spawnTimer;
    private int SelectedTarget;
    private int prevTarget = -1;

    private Transform projectileHolder;
    private AudioSource SwooshPlayer;
    public AudioClip Toss1, Toss2;

    private SubtitleManager sManager_Ref;
    private int WordsNumber;
    private int prevVoiceLine = -1;

    public List<ArchingProjectile> spawnedProjectiles = new List<ArchingProjectile>();

    public static System.EventHandler SayingVoiceLine;

    private void Awake()
    {
        projectileHolder = this.transform;
        spawnTimer = 0;
        TargetPoints.AddRange(FindObjectsOfType<TargetPoint>());
        meterFillAmount = meterMaxHeight / 3;
        LGM = FindObjectOfType<LearningGrillManager>();
        sManager_Ref = FindObjectOfType<SubtitleManager>();
    }

    public void PreventSpawning()
    {
        spawningStatus = SpawningStatus.Idle;
    }

    public void StopAllThings()
    {
        //sManager_Ref.PlaySubtitle(53);
        spawningStatus = SpawningStatus.Idle;

        //sManager_Ref.TurnOffSubtitleImmediately();

        int count = spawnedProjectiles.Count;
        for(int x = 0; x < count; x++)
        {
            GameObject gO = spawnedProjectiles[x].gameObject;
            Destroy(gO.gameObject, 0.1f);
            spawnedProjectiles.Remove(gO.GetComponent<ArchingProjectile>());
        }
    }

    public void FillMeter()
    {
        SliderValue++;
        detectionSprite.transform.localScale += new Vector3(0, meterFillAmount, 0);
    }

    void Start()
    {
        SwooshPlayer = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (debugFinish) { SliderValue = 3; }

        switch (spawningStatus)
        {
            case SpawningStatus.Idle:
                SwitchColors();
                HideTargets();
                break;
            case SpawningStatus.Spawnning:
                SwitchColors();
                LaunchProjectile();
                break;
        }

    }

    private void SwitchColors()
    {
        detectionSprite.color = Color.red;
    }

    private void LaunchProjectile()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }

        else if (spawnTimer <= 0)
        {
            spawnTimer = SetSpawnTimer;

            int TargetNum = 0;
            do
            {
                TargetNum = Random.Range(0, TargetPoints.Count);
            }
            while (TargetNum == prevTarget);

            int PrefabNum = Random.Range(0, ProjectilePrefabs.Count);

            SelectedTarget = TargetNum;
            prevTarget = SelectedTarget;

            ActivateSelectedTarget();
            SpawnerRotateTowards(SelectedTarget);

            SayLines();
            SpawnProjectile(PrefabNum);
        }
    }

    private void SayLines()
    {
        if (SayingVoiceLine != null) { SayingVoiceLine(this, System.EventArgs.Empty); }

        do { WordsNumber = Random.Range(0, 5); }
        while (WordsNumber == prevVoiceLine);
        prevVoiceLine = WordsNumber;

        switch (WordsNumber)
        {
            case 0:
                sManager_Ref.PlaySubtitle(40);
                break;
            case 1:
                sManager_Ref.PlaySubtitle(41);
                break;
            case 2:
                sManager_Ref.PlaySubtitle(42);
                break;
            case 3:
                sManager_Ref.PlaySubtitle(43);
                break;
            case 4:
                sManager_Ref.PlaySubtitle(44);
                break;
        }
    }

    private void ActivateSelectedTarget()
    {
        //for (int i = 0; i < TargetPoints.Count; i++)
        //{
        //    if ( SelectedTarget == i)
        //    {
        //        TargetPoints[i].gameObject.SetActive(true);
        //    }
        //    //else
        //    //{
        //    //    TargetPoints[i].gameObject.SetActive(false);
        //    //}
        //}

        TargetPoints[SelectedTarget].gameObject.SetActive(true);
    }

    private void HideTargets()
    {
        for (int i = 0; i < TargetPoints.Count; i++)
        {
            TargetPoints[i].gameObject.SetActive(false);
        }
    }

    public void SpawnProjectile(int prefabNum)
    {
        PlaySound();
        GameObject spawnedProjectile;
        spawnedProjectile = Instantiate(ProjectilePrefabs[prefabNum], this.transform.position, Quaternion.identity);
        spawnedProjectile.transform.parent = projectileHolder;
        spawnedProjectile.GetComponent<ArchingProjectile>().Target = TargetPoints[SelectedTarget].transform.position;
        spawnedProjectile.GetComponent<ArchingProjectile>().arcSpeed = arcSpeed;
        spawnedProjectile.GetComponent<ArchingProjectile>().trajectoryHeight = verticalArcSpeed;

        spawnedProjectiles.Add(spawnedProjectile.GetComponent<ArchingProjectile>());
    }

    public void SpawnerRotateTowards(int selectedBush)
    {
        this.transform.LookAt(TargetPoints[SelectedTarget].transform);
    }

    public void PlaySound()
    {
        int audioChoice = 0;

        audioChoice = Random.Range(0, 2);

        if (audioChoice == 1)
        {
            SwooshPlayer.PlayOneShot(Toss1);
        }
        else
        {
            SwooshPlayer.PlayOneShot(Toss2);
        }

    }

}
