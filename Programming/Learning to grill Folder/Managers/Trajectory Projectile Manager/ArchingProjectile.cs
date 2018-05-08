
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchingProjectile : MonoBehaviour
{
    // Use this for initialization
    private ArchingProjectileSpawner APS;
    private LearningGrillManager LGM;
    //public int DetectionPenalty;
    private bool hasSpawned;
    public float DespawnTimer;

    private bool hasHitBigfoot = false;
    private AudioSource Splatplayer;

    public Vector3 Target;
    public float trajectoryHeight = 5;
    public float arcSpeed = 0.04f;

    Vector3 startPos = new Vector3(0, 0, 0);
    private float cTime = 0f;

    private void Awake()
    {
        Splatplayer = GameObject.Find("Target").GetComponent<AudioSource>();
    }

    void Start()
    {
        APS = FindObjectOfType<ArchingProjectileSpawner>();
        LGM = FindObjectOfType<LearningGrillManager>();
        startPos = this.transform.position;
    }

    void FixedUpdate()
    {
        // calculate current time within our lerping time range
        cTime += arcSpeed;
        // calculate straight-line lerp position:
        Vector3 currentPos = Vector3.Lerp(startPos, Target, cTime);
        // add a value to Y, using Sine to give a curved trajectory in the Y direction
        currentPos.y += trajectoryHeight * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);
        // finally assign the computed position to our gameObject:
        transform.position = currentPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision // " + collision.gameObject.name);

        if (collision.gameObject.tag != "IgnoreProjectileCollision")
        {
            if (!hasHitBigfoot)
            {
                APS.spawnedProjectiles.Remove(this);
                Destroy(this.gameObject, DespawnTimer);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Collision - " + other.gameObject.name + " // Tag -" + other.gameObject.tag);

        if (other.gameObject.tag != "IgnoreProjectileCollision")
        {

            if (other.gameObject.GetComponent<Bigfoot>() != null)
            {
                if (!hasHitBigfoot) { hasHitBigfoot = true; }
                LGM.PickUpIngredient();
                Splatplayer.Play();

                for (int i = 0; i < APS.TargetPoints.Count; i++)
                {
                    if (APS.TargetPoints[i].occupiedState == TargetPoint.OccupiedState.Occupied)
                    {
                        APS.TargetPoints[i].occupiedState = TargetPoint.OccupiedState.Empty;
                        APS.TargetPoints[i].gameObject.SetActive(false);
                    }
                }

                APS.spawnedProjectiles.Remove(this);
                Destroy(this.gameObject);
            }

            else if (other.gameObject.GetComponent<TargetPoint>() != null)
            {
                for (int i = 0; i < APS.TargetPoints.Count; i++)
                {
                    if (APS.TargetPoints[i].occupiedState == TargetPoint.OccupiedState.Occupied)
                    {
                        hasHitBigfoot = true;
                        LGM.PickUpIngredient();
                        Splatplayer.Play();
                        APS.TargetPoints[i].occupiedState = TargetPoint.OccupiedState.Empty;
                    }
                }
                
                if (!hasHitBigfoot)
                {
                    Splatplayer.Play();
                    LGM.MissedIngredient();
                }

                other.gameObject.SetActive(false);
                APS.spawnedProjectiles.Remove(this);
                Destroy(this.gameObject);
            }


            if (!hasHitBigfoot)
            {
                APS.spawnedProjectiles.Remove(this);
                Destroy(this.gameObject, DespawnTimer);
            }
        }
    }
}
