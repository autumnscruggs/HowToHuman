using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpeechUI : MonoBehaviour
{
    private LearningTriggerAndNPCSpeechMeter learningTrigger;
    private Image[] strikes;
    private GameObject strikeHolder;
    public GameObject strikeMark;

    private void OnEnable()
    {
        if(learningTrigger == null) { learningTrigger = GameObject.FindObjectOfType<LearningTriggerAndNPCSpeechMeter>(); }
        learningTrigger.SeenEvent += UpdateUI;
    }

    private void OnDisable()
    {
        learningTrigger.SeenEvent -= UpdateUI;
    }

    void Start()
    {
        strikeHolder = this.transform.GetChild(0).gameObject;
        for(int i = 0; i < learningTrigger.MaxTimesSeen; i++)
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
        for(int z = 0; z < learningTrigger.TimesSeen; z++)
        {
            if(strikes.Length >= z)
            {
                strikes[z].color = Color.red;
            }
        }
    }
}
