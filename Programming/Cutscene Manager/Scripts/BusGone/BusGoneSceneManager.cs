using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BusGoneSceneManager : MonoBehaviour
{
	public Animator BusAnimation;
    public Image FadeoutBox2;
	public GameObject Bigfoot, Camera, Buspad, UI;
	
	public float FadeOutValue1, FadeInValue, FadeOutValue2;
	public float Courtine1, Courtine2, Courtine3, Courtine4;


	private SubtitleManager subtitleRef;
	private FadeInGame fadeRef;

	[HideInInspector] public bool DoIt;
	private bool PlayOnce;

	// Use this for initialization
	void Start ()
	{
		subtitleRef = FindObjectOfType<SubtitleManager>().GetComponent<SubtitleManager>();
		fadeRef = FindObjectOfType<FadeInGame>().GetComponent<FadeInGame>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (DoIt == true)
		{
			if (PlayOnce == false)
			{
				PlayOnce = true;
				subtitleRef.PlaySubtitle(59);
				StartCoroutine(PlayOffPlayer());
			}
		}
	}

   


	public IEnumerator PlayOffPlayer()
	{
		CutsceneManagement.InCutscene = true;

		Bigfoot.SetActive(false);
		UI.SetActive(false);
		Buspad.SetActive(false);
		Camera.SetActive(true);
		fadeRef.FadeOut(FadeOutValue1);
		yield return new WaitForSeconds(Courtine1);
		fadeRef.FadeIn(FadeInValue);
        
        Camera.SetActive(true);
		yield return new WaitForSeconds(Courtine2);
		BusAnimation.enabled = true;
		
		yield return new WaitForSeconds(Courtine3);
        FadeoutBox2.gameObject.SetActive(true);
        fadeRef.FadeOut(FadeOutValue2);
        yield return new WaitForSeconds(Courtine4);
        yield return new WaitForSeconds(4f);

		CutsceneManagement.InCutscene = false;

		SceneManager.LoadScene(3);

	}
}
