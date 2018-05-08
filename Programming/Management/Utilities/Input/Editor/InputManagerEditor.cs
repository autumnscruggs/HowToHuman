using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerEditor : Editor
{
    private void OnEnable()
    {
    }

    private void LoadAll(InputManager target)
    {
        InputManagerSaveManager.Load();

        target.gpDeadSpace = InputManager.GamepadDeadSpace;

        if (InputManager.Keyboard != null)
        {
            target.keyboard.LeftInputs = new List<KeyCode>(InputManager.Keyboard.LeftInputs);
            target.keyboard.RightInputs = new List<KeyCode>(InputManager.Keyboard.RightInputs);
            target.keyboard.UpInputs = new List<KeyCode>(InputManager.Keyboard.UpInputs);
            target.keyboard.DownInputs = new List<KeyCode>(InputManager.Keyboard.DownInputs);
            target.keyboard.CrouchInputs = new List<KeyCode>(InputManager.Keyboard.CrouchInputs);
            target.keyboard.SprintInputs = new List<KeyCode>(InputManager.Keyboard.SprintInputs);
            target.keyboard.InteractionInputs = new List<KeyCode>(InputManager.Keyboard.InteractionInputs);
        }

        if (InputManager.Gamepad != null)
        {
            target.gamepad.CrouchInputs = new List<GamepadButtons>(InputManager.Gamepad.CrouchInputs);
            target.gamepad.SprintInputs = new List<GamepadButtons>(InputManager.Gamepad.SprintInputs);
            target.gamepad.InteractionInputs = new List<GamepadButtons>(InputManager.Gamepad.InteractionInputs);
        }
    }

    public override void OnInspectorGUI()
    {
        var inputTarget = (InputManager)target;
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("File located @ \"" + InputManagerSaveManager.filePath + "/\"");

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load Input Values"))
        {
            LoadAll(inputTarget);
        }

        if (GUILayout.Button("Save Input Values"))
        {
            InputManager.Keyboard = inputTarget.keyboard;
            InputManager.Gamepad = inputTarget.gamepad;

            InputManagerSaveManager.Save();
        }

        EditorGUILayout.EndHorizontal();
    }
   
    
}
