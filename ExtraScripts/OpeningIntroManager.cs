using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OpeningIntroManager : MonoBehaviour
{
	private PlayerInput input;
	public VideoPlayer video;
	private Loader loader;

	private void Awake()
	{
		input = this.GetComponent<PlayerInput>();
		video = GameObject.FindObjectOfType<VideoPlayer>();
		loader = this.GetComponent<Loader>();
	}

	void Start ()
	{
		Time.timeScale = 1;
		loader.StartLoading();
		video.loopPointReached += EndReached;
	}

	void Update ()
	{
		VideoSkip();
	}

	void VideoSkip()
	{
		if(loader.CanLoad)
		{
			if (input.InteractionInputPushed)
			{
				loader.ActivateScene();
				//SceneManager.LoadScene(2);
			}
		}
	}

	void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
		loader.ActivateScene();
		//SceneManager.LoadScene(2);
	}
}
