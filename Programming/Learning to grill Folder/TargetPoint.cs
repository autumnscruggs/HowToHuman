using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour {

    // Use this for initialization
    private LearningGrillManager LGM;
    private AudioSource Splatplayer;
    private GameObject ActiveProjectile;
    public enum OccupiedState { Empty, Occupied}
    public OccupiedState occupiedState;
    void Awake()
    {
        LGM = FindObjectOfType<LearningGrillManager>();
        Splatplayer = GameObject.Find("Target").GetComponent<AudioSource>();
    }

    private void Update()
    {
        SearchForProjectile();
    }
    private void SearchForProjectile()
    {
        try
        {
            if (ActiveProjectile == null)
            {
                ActiveProjectile = FindObjectOfType<ArchingProjectile>().gameObject;
            }
        }
        catch(System.NullReferenceException)
        {
            Debug.LogWarning("No need to worry - but the SearchForProjectile function " +
                "threw a null for the ActiveProjectile");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "IgnoreProjectileCollision")
        {
            if (other.gameObject.GetComponent<Bigfoot>() != null)
            {
                occupiedState = OccupiedState.Occupied;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Bigfoot>() != null)
        {
            occupiedState = OccupiedState.Empty;
        }
    }
}
