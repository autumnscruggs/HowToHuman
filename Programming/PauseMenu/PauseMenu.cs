using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Text optionsButton;
    public GameObject container;
    public GameObject buttons;
    public GameObject objectives;
    public GameObject options;

    public Slider music;
    public Slider sFX;
    public Slider dialogue;

    public Toggle subtitle;
    public Toggle tutorial;
    public Toggle camera;

    private void OnEnable()
    {
        Pauser.OnPause += OpenMenu;
        Pauser.OnUnPause += CloseMenu;
    }

    private void OnDisable()
    {
        Pauser.OnPause -= OpenMenu;
        Pauser.OnUnPause -= CloseMenu;
    }

    private void Start()
    {
        ToggleMain(true);
        OpenPauseMenu(false);
    }


    public void Resume()
    {
        Pauser.UnPauseGame();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleOptions()
    {
        ToggleMain(options.activeInHierarchy);
    }


    public void OpenMenu(object sender, System.EventArgs e)
    {
        ToggleMain(true);
        OpenPauseMenu(true);

        //if (InputManager.ControllerType == ControlType.Gamepad)
        //{
        //    GameObject button = buttons.GetComponentInChildren<Button>().gameObject;
        //    //Debug.Log("Name : // " + button.name);
        //    EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(button);
        //}
    }

    public void CloseMenu(object sender, System.EventArgs e)
    {
        ToggleMain(true);
        OpenPauseMenu(false);
    }

    private void ToggleMain(bool show)
    {
        optionsButton.text = show ? "Options" : "Objectives";
        objectives.gameObject.SetActive(show);
        options.gameObject.SetActive(!show);

        GameObject activeParent = objectives.gameObject.activeInHierarchy ? objectives : options;

        if (InputManager.ControllerType == ControlType.Gamepad)
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(activeParent.GetComponentInChildren<Button>().gameObject);
        }
    }

    public void OpenPauseMenu(bool show)
    {
        container.gameObject.SetActive(show);
    }

}
