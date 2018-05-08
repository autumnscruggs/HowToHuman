using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryHide : MonoBehaviour
{
    public GameObject slider;
    public Text objectiveText;

    private void Awake()
    {
        HideSlider();
    }

    public void ShowText()
    {
        HideSlider();
        objectiveText.gameObject.SetActive(true);
    }

    public void HideText()
    {
        objectiveText.gameObject.SetActive(false);
    }

    public void ShowSlider()
    {
        slider.gameObject.SetActive(true);
    }

    public void HideSlider()
    {
        slider.gameObject.SetActive(false);
    }

    public void ShowText(object sender, EventArgs e) { ShowText(); }
    public void HideText(object sender, EventArgs e) { HideText(); }
}
