using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BusEscape : MonoBehaviour
{
    public BusGoneSceneManager busgoneRef;

    private void Start()
    {
        //busgoneRef = FindObjectOfType<BusGoneSceneManager>().GetComponent<BusGoneSceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bigfoot>())
        {
            busgoneRef.DoIt = true;
        }
    }



}
