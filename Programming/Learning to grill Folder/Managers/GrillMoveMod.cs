using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GrillMoveMod : MonoBehaviour
{

	// Use this for initialization
	[HideInInspector]
	public GameObject player;
    public GameObject Lights;
	private UpdateObjective updateObj;

    private SubtitleManager subtitleNarr;

    public ProgressBar progressBar;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        progressBar = GameObject.FindObjectOfType<ProgressBar>();
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    void Start ()
	{
        player = FindObjectOfType<Bigfoot>().gameObject;
		updateObj = GameObject.FindObjectOfType<UpdateObjective>();
        subtitleNarr = FindObjectOfType<SubtitleManager>().GetComponent<SubtitleManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
	   if (other.gameObject.GetComponent<Bigfoot>() != null)
		{
            //Spawn the game screen
            //On Confirmation Teleport Bigfoot to one of the bushes
            //Profit
            //Lights.SetActive(true);

			if (updateObj.CurrentObjective == GameState.FindFood)
			{
                // StartCoroutine(NotificationDelay());
                subtitleNarr.PlaySubtitle(37);
                if (!PauseData.ShowTutorials) { progressBar.gameObject.SetActive(true); }
                updateObj.ChangeState(GameState.LearnToGrill);
            }
        }
	}
}
