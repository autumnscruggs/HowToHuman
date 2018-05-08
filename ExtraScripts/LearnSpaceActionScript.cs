using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnSpaceActionScript : MonoBehaviour
{
    public GameObject LearningSpace;

	// Use this for initialization
	void Start ()
    {
        LearningSpace.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateSpace()
    {
        LearningSpace.SetActive(true);
    }

    public void DeactivateSpace()
    {
        LearningSpace.SetActive(false);
    }
}
