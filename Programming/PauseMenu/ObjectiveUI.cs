using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public GameObject objContainer;
    public List<Transform> objectives = new List<Transform>();

    private RectTransform objectiveUI;
    private ProgressBar progressBar;
    private bool on = true;
    private bool notificationInProgress = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        UpdateObjective.ObjectiveChangedEvent += ShowNotification;
        IntroCameraScript.CutsceneComplete += ShowNotification;
        Pauser.OnPause += RefreshUI;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        UpdateObjective.ObjectiveChangedEvent -= ShowNotification;
        IntroCameraScript.CutsceneComplete -= ShowNotification;
        Pauser.OnPause -= RefreshUI;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(objectiveUI == null) { objectiveUI = GameObject.Find("ObjectiveUI").GetComponent<RectTransform>(); objectiveUI.gameObject.SetActive(false); }
        if(progressBar == null) { progressBar = GameObject.FindObjectOfType<ProgressBar>(); }

        Transform[] array = objContainer.GetComponentsInChildren<Transform>();
        foreach(Transform t in array)
        {
            if(!t.gameObject.name.Contains("Strike") && !t.gameObject.name.Contains("Objectives"))
            {
                objectives.Add(t);
            }
        }

        ShowObjective((int)CheckpointValues.CurrentState);
    }

    private void ShowObjective(int objIndex)
    {
        for(int x = 0; x < objectives.Count; x++)
        {
            if(x < objIndex)
            {
                //Debug.Log("Crossy // " + x);
                if (!objectives[x].gameObject.activeInHierarchy) { objectives[x].gameObject.SetActive(true); }
                if (!objectives[x].GetChild(0).gameObject.activeInHierarchy) { objectives[x].GetChild(0).gameObject.SetActive(true); }
            }
            else if(x == objIndex)
            {
                //Debug.Log("Showey // " + x);
                if (!objectives[x].gameObject.activeInHierarchy) { objectives[x].gameObject.SetActive(true); }
                if (objectives[x].GetChild(0).gameObject.activeInHierarchy) { objectives[x].GetChild(0).gameObject.SetActive(false); }
            }
            else
            {
                //Debug.Log("Hidey // " + x);
                if (objectives[x].gameObject.activeInHierarchy) { objectives[x].gameObject.SetActive(false); }
            }
        }
    }

    public void RefreshUI(object sender, System.EventArgs e)
    {
        StartCoroutine(RefreshDelay());
    }


    public void SetUI(GameState state)
    {
        ShowObjective((int)state);
    }

    public void ShowNotification(object sender, System.EventArgs e)
    {
        if (!notificationInProgress) { StopCoroutine(Notification()); }
        if (!progressBar.gameObject.activeInHierarchy) {  objectiveUI.anchoredPosition = new Vector2(objectiveUI.anchoredPosition.x, 0); }
        else { objectiveUI.anchoredPosition = new Vector2(objectiveUI.anchoredPosition.x, 90); }
        StartCoroutine(Notification());
    }

    IEnumerator RefreshDelay()
    {
        yield return new WaitForEndOfFrame();
        SetUI(CheckpointValues.CurrentState);
    }

    IEnumerator Notification()
    {
        notificationInProgress = true;

        objectiveUI.gameObject.SetActive(true);
        objectiveUI.transform.GetChild(1).gameObject.GetComponent<Text>().text = ObjectiveNameByNumber((int)CheckpointValues.CurrentState);

        for (int z = 0; z < 11; z++)
        {
            yield return new WaitForSeconds(0.3f);
            on = !on;
            objectiveUI.transform.GetChild(0).gameObject.SetActive(on);
        }

        objectiveUI.gameObject.SetActive(false);

        notificationInProgress = false;
    }

    private string ObjectiveNameByNumber(int value)
    {
        switch (value)
        {
            case 0:
                return "Find Clothes!";
            case 1:
                return "Go to Bus Station";
            case 2:
                return "Learn to Speak \"Human\"";
            case 3:
                return "Listen to phone call";
            case 4:
                return "Return to Bus Driver";
            case 5:
                return "Find food for Bus Driver";
            case 6:
                return "Catch ingredients to make \"Burger\"";
            case 7:
                return "Deliver Burger to Bus Driver";
            case 8:
                return "Get on the Bus";
            default:
                return "";
        }
    }
}
