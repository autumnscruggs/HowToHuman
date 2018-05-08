using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsQuit : MonoBehaviour
{
    private FadeInGame fadeRef;
    public float FadeValue;

	// Use this for initialization
	void Start ()
    {
        fadeRef = GetComponent<FadeInGame>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        fadeRef.FadeIn(FadeValue);

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(0);
        }
	}


}
