using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointValues : MonoBehaviour
{
	public Vector3 ValuesForSpawn = new Vector3(-13.75848f, -0.1713867f, 46.24906f);

	[HideInInspector] public GameObject Bigfoot;
	public static GameState CurrentState { get; set; }

	//public bool OutofStart;


	//Shit is for music
	public AudioClip BeforeClothing, AfterClothing;
	public AudioSource MusicPlayer;
	[HideInInspector] public bool GotClothing;

	private void Awake()
	{
		
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if(arg0.name == "PresentScene")
		{
			if (CurrentState == GameState.GetOnBus) { CurrentState = GameState.FindClothes; }

			//SetMusic();
			Bigfoot = GameObject.FindObjectOfType<Bigfoot>().gameObject;
			Bigfoot.transform.position = this.ValuesForSpawn;
			MusicPlayer = GameObject.Find("Music Player").GetComponent<AudioSource>();

			if (CurrentState != GameState.FindClothes)
			{
				Bigfoot.GetComponent<DisguiseManager>().PickUpDisguise(DisguiseTypes.Hunter);
				MusicPlayer.clip = AfterClothing;
				MusicPlayer.Play();
			}
			else
			{
				Bigfoot.GetComponent<DisguiseManager>().LoseDisguise();
				MusicPlayer.clip = BeforeClothing;
				MusicPlayer.Play();
			}

			GameObject.FindObjectOfType<UpdateObjective>().SetUIForState(CurrentState);
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
	}

	void Start ()
	{
		

		if(this.ValuesForSpawn == Vector3.zero) { this.ValuesForSpawn = Bigfoot.transform.position; }


	   
	}

	void SetMusic()
	{
		if (GotClothing == true)
		{
			MusicPlayer.clip = AfterClothing;
			MusicPlayer.Play();
		}
		else
		{
			MusicPlayer.clip = BeforeClothing;
			MusicPlayer.Play();
		}
	}
	
}
