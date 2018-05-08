using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    public static bool Paused = false;
    public static EventHandler OnPause;
    public static EventHandler OnUnPause;

    public Gamepad Gamepad { get; private set; }
    public bool PauseInputPushed
    {
        get
        {
            return (Gamepad != null && Gamepad.IsConnected && (Gamepad.IsButtonUpFromList(InputManager.Gamepad.PauseInputs)))
            || KeyboardInput.IsKeyDownFromList(InputManager.Keyboard.PauseInputs);
        }
    }

    private void Awake()
    {
        if (GamepadManager.Instance != null)
        {
            Gamepad = GamepadManager.Instance.GetGamepad(1);
        }
    }

    private void Start()
    {
        UnPauseGame();
    }

    private void Update()
    {
        if(PauseInputPushed)
        {
            Paused = !Paused;
            if(Paused) { PauseGame(); }
            else { UnPauseGame(); }
        }
    }


    public static void PauseGame()
    {
        Cursor.visible = true;
        OnPause(null, EventArgs.Empty);
        Time.timeScale = 0;
    }

    public static void UnPauseGame()
    {
        Cursor.visible = false;
        OnUnPause(null, EventArgs.Empty);
        Time.timeScale = 1;
    }
}
