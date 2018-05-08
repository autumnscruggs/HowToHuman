using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggers : MonoBehaviour
{
    public GameObject Triggers1, Triggers2, Triggers3, Triggers4, Triggers5;
    private UpdateObjective updateobjective_ref;


	// Use this for initialization
	void Start ()
    {
        updateobjective_ref = GetComponent<UpdateObjective>();
        SetAllObjectsOff();
    }

    private void Update()
    {
        SetInstancesOffObjectives();
    }

    

    void SetInstancesOffObjectives()
    {
        switch (updateobjective_ref.CurrentObjective)
        {
            case GameState.GoToBusStation:
                ShowObjectsOne();
                break;
            case GameState.FindFood:
                ShowObjectsTwo();
                break;
            case GameState.LearnToGrill:
                ShowObjectsTwo();
                break;
            //case GameState.MakeBurger:
            //    ShowobjectsFour();
            //    break;
            case GameState.GetOnBus:
                ShowobjectsFive();
                break;
        }
    }

    void ShowObjectsOne()
    {
        Triggers1.SetActive(true);
    }

    void ShowObjectsTwo()
    {
        Triggers2.SetActive(true);
    }

    void ShowobjectsThree()
    {
        Triggers3.SetActive(true);
    }

    void ShowobjectsFour()
    {
        Triggers4.SetActive(true);
    }

    void ShowobjectsFive()
    {
        Triggers5.SetActive(true);
    }

    void SetAllObjectsOff()
    {
        Triggers1.SetActive(false);
        Triggers2.SetActive(false);
        Triggers3.SetActive(false);
        Triggers4.SetActive(false);
        Triggers5.SetActive(false);
    }

    public void SetEvent2Off()
    {
        Triggers2.SetActive(false);
    }
}
