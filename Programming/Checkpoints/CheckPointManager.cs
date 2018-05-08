using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
	public List<GameObject> Checkpoints; //Tracks how many check points we'll have in level.

	//[HideInInspector]
	public int CurrentCheckpoint; //Lets the manager know which checkpoint we're on now.

	[HideInInspector]
	public CheckpointValues checkpointValues_ref;

	[HideInInspector]
	public GameObject Bigfoot;

	

	#region Singleton
	private static CheckPointManager instance;
	private CheckPointManager() { }
	public static CheckPointManager Instance
	{
		get
		{
			return instance;
		}
		private set
		{
			if(instance == null)
			{
				instance = value;
			}
		}
	}
	#endregion

	private void OnEnable()
	{
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		Bigfoot = GameObject.FindObjectOfType<Bigfoot>().gameObject;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
	}

	private void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		//Bigfoot = GameObject.Find("Bigfoot");
		checkpointValues_ref = GameObject.Find("GameManager").GetComponent<CheckpointValues>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		TestReset(); //TODO: Debug for testing. DELETE AFTER
	}


	public void ResetScene()
	{
		//Scene should reset and Bigfoot will appear at a new location.

		if (CheckpointValues.CurrentState == GameState.FollowGirl) { CheckpointValues.CurrentState = GameState.LearnToSpeak; }
        if (CheckpointValues.CurrentState == GameState.LearnToGrill) { CheckpointValues.CurrentState = GameState.FindFood; }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void TestReset()
	{
		if (Input.GetKeyDown(KeyCode.Z)) 
		{
			//Resets the level and moves the player to that position.
			ResetScene();
		}
	}
}
