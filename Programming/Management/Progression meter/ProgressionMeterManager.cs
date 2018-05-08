using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionMeterManager : MonoBehaviour {

	private Slider progressSlider;
	public Slider ProgressSlider { get { return progressSlider; } private set { progressSlider = value; } }


	private float progressValue;
	public float ProgressValue { get { return progressValue; } private set { progressValue = value; } }

	private AudioSource ProgresionSoundplayer;
	private bool IsPlaying = false;

	//private float fillSpeed = 5;
	//public float FillSpeed { get { return fillSpeed; } private set { fillSpeed = value; } }

	// Use this for initialization
	void Start ()
	{
		progressSlider = GameObject.FindObjectOfType<ProgressBar>().GetComponent<Slider>();
		HideProgressionMeter();
		ProgresionSoundplayer = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void UpdateProgressionBar(float fillSpeed)
	{
		if (ProgressValue <= 100)
		{
			ProgressValue += fillSpeed * (Time.deltaTime);
			progressSlider.value = ProgressValue;
			StartCoroutine(PlaySound());
		}
	}

	public void ResetProgress()
	{
		ProgressValue = 0;
		progressSlider.value = ProgressValue;
	}

	public void ShowProgressionMeter()
	{
		progressSlider.gameObject.SetActive(true);
	}
	public void HideProgressionMeter()
	{
		progressSlider.gameObject.SetActive(false);
	}

	public void PlaySoundOneShot()
	{
		ProgresionSoundplayer.PlayOneShot(ProgresionSoundplayer.clip);
	}

	IEnumerator PlaySound()
	{
		if (IsPlaying == false)
		{
			IsPlaying = true;
			ProgresionSoundplayer.Play();
			yield return new WaitForSeconds(ProgresionSoundplayer.clip.length);
			IsPlaying = false;
		}
		

	}
}
