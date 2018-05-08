using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInGame : MonoBehaviour
{
	private Image FadeBox;
	private Color fadeboxColor;

	// Use this for initialization
	void Start ()
	{
		FadeBox = GameObject.Find("Fade Box").GetComponent<Image>();
		fadeboxColor = FadeBox.color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		FadeBox.color = fadeboxColor;
	}

	public void FadeIn(float value)
	{
		
			fadeboxColor.a -= value * Time.deltaTime;
	  
	}

	public void FadeOut(float value)
	{
	   
			fadeboxColor.a += value * Time.deltaTime;
		
	}
}
