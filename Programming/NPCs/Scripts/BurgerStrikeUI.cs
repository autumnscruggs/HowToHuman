using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurgerStrikeUI : MonoBehaviour
{
	private LearningGrillManager grillManager;
	private Image[] strikes;
	private GameObject strikeHolder;
	public GameObject strikeMark;

	private void OnEnable()
	{
		if(grillManager == null) { grillManager = GameObject.FindObjectOfType<LearningGrillManager>(); }
		LearningGrillManager.MissedIngredientEvent += UpdateUI;
	}

	private void OnDisable()
	{
		LearningGrillManager.MissedIngredientEvent -= UpdateUI;
	}

	void Start()
	{
		strikeHolder = this.transform.GetChild(0).gameObject;
		for (int i = 0; i < grillManager.MaxStrikes; i++)
		{
			GameObject.Instantiate(strikeMark, strikeHolder.transform);
		}
		HideUI();
		strikes = strikeHolder.GetComponentsInChildren<Image>();
	}

	public void ShowUI()
	{
		strikeHolder.SetActive(true);
	}

	public void HideUI()
	{
		strikeHolder.SetActive(false);
	}

	private void UpdateUI(object sender, System.EventArgs e)
	{
		int index = grillManager.Strikes - 1;
		if (index >= 0 && index < strikes.Length) { strikes[index].color = Color.red; }
	}
}
