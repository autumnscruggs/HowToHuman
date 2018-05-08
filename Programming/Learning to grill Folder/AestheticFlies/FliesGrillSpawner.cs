using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FliesGrillSpawner : MonoBehaviour
{
    private GameObject flyHolder;
    private float randomFlyTimer = 0;
    private int prevFly = -1;
    private int randomFly = -1;

    private bool fliesInAction = false;
    public float timeBetweenFlies = 50;

    private List<GrillFly> ContinuousFlies = new List<GrillFly>();
    private List<GrillFly> NonContinuousFlies = new List<GrillFly>();

    private AudioSource source;

    private void Awake()
    {
        source = this.GetComponent<AudioSource>();

        flyHolder = this.transform.GetChild(0).gameObject;
        GrillFly[] flies = this.transform.GetComponentsInChildren<GrillFly>();
        foreach(GrillFly gf in flies)
        {
            if(gf.ContinuousFly)
            {
                if (!ContinuousFlies.Contains(gf)) { ContinuousFlies.Add(gf); }
            }
            else
            {
                if (!NonContinuousFlies.Contains(gf)) { NonContinuousFlies.Add(gf); }
            }
        }
        StopFlies();
    }

    public void StartFlies()
    {
        source.Play();
        fliesInAction = true;
        flyHolder.gameObject.SetActive(true);
        StartRandomFly();
    }

    public void StopFlies()
    {
        source.Stop();
        fliesInAction = false;
        flyHolder.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(fliesInAction)
        {
            randomFlyTimer += Time.deltaTime;
            if(randomFlyTimer >= timeBetweenFlies)
            {
                randomFlyTimer = 0;
                StartRandomFly();
            }
        }
    }

    private void StartRandomFly()
    {
        do
        {
            randomFly = Random.Range(0, NonContinuousFlies.Count);
        }
        while (randomFly == prevFly);

        NonContinuousFlies.ForEach(x => x.gameObject.SetActive(false));
        NonContinuousFlies[randomFly].gameObject.SetActive(true);
    }
}
