using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public enum ControlType { Keyboard, Gamepad }

public class InputManager : MonoBehaviour
{
    public static KeyboardInputStrings Keyboard;
    public static GamepadInputStrings Gamepad;

    public static ControlType ControllerType;

    public static float GamepadDeadSpace = 0.2f;

    private void Awake()
    {
        InputManagerSaveManager.Load();
    }

    private void Update()
    {
        TestForControllerType();
    }

    private bool LeftInputDown()
    {
        return true;
    }

    private void TestForControllerType()
    {
        if (ControllerType != ControlType.Gamepad)
        {
            Gamepad gamepad = GamepadManager.Instance.GetGamepad(1);
            if (gamepad != null && gamepad.IsConnected &&
                (gamepad.IsHoldingAnyButton() || gamepad.IsMovingEitherThumbStick))
            {
                ControllerType = ControlType.Gamepad;
            }
        }
        else if (ControllerType != ControlType.Keyboard)
        {
            Gamepad gamepad = GamepadManager.Instance.GetGamepad(1);
            if (gamepad == null || !gamepad.IsConnected) { ControllerType = ControlType.Keyboard; }
            if (KeyboardInput.IsAnyKeyDown())
            {
                ControllerType = ControlType.Keyboard;
            }
        }
    }

    #region For Inspector
    public float gpDeadSpace;
    public KeyboardInputStrings keyboard = new KeyboardInputStrings();
    public GamepadInputStrings gamepad = new GamepadInputStrings();
    #endregion
}

public class InputManagerSaveManager
{
    public static string filePath;
    public static string realFilePath;
    public static string kbFileName;
    public static string gpFileName;

    public static void GetFilePaths()
    {
        filePath = "/3) Prefabs/Management/Utilities/Input/Resources/";
        realFilePath = Application.dataPath + filePath;
        kbFileName = "keyboard";
        gpFileName = "gamepad";
    }

    public static void Save()
    {
        GetFilePaths();
        SaveInputFile(kbFileName, InputManager.Keyboard);
        SaveInputFile(gpFileName, InputManager.Gamepad);
    }

    public static void Load()
    {
        GetFilePaths();
        InputManager.Keyboard = (KeyboardInputStrings)LoadInputFile(kbFileName);
        InputManager.Gamepad = (GamepadInputStrings)LoadInputFile(gpFileName);
    }

    private static InputStrings LoadInputFile(string fileName)
    {
        InputStrings input = null;

        if (fileName.Contains(kbFileName))
        {
            TextAsset file = Resources.Load(fileName) as TextAsset;

            if (file != null && file.text != "")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(KeyboardInputStrings));
                StringReader reader = new StringReader(file.text);
                input = serializer.Deserialize(reader) as KeyboardInputStrings;
                reader.Close();
            }
        }
        else
        {
            TextAsset file = Resources.Load(fileName) as TextAsset;

            if (file != null && file.text != "")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GamepadInputStrings));
                StringReader reader = new StringReader(file.text);
                input = serializer.Deserialize(reader) as GamepadInputStrings;
                reader.Close();
            }
        }

        return input;
    }

    private static void SaveInputFile(string fileName, InputStrings inputStrings)
    {
        string filepath = realFilePath + fileName + ".xml";

        if (File.Exists(filepath)) { File.Delete(filepath); }

        StreamWriter writer;
        FileInfo t = new FileInfo(filepath);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }
        writer.Write(XMLFileHandler.ObjectToXml(inputStrings));
        writer.Close();
    }
}

[System.Serializable]
public abstract class InputStrings
{

}

[System.Serializable]
public class KeyboardInputStrings : InputStrings
{
    public List<KeyCode> LeftInputs = new List<KeyCode>();
    public List<KeyCode> RightInputs = new List<KeyCode>();
    public List<KeyCode> UpInputs = new List<KeyCode>();
    public List<KeyCode> DownInputs = new List<KeyCode>();

    public List<KeyCode> SprintInputs = new List<KeyCode>();
    public List<KeyCode> CrouchInputs = new List<KeyCode>();
    public List<KeyCode> InteractionInputs = new List<KeyCode>();
    public List<KeyCode> PauseInputs = new List<KeyCode>();
}

[System.Serializable]
public class GamepadInputStrings : InputStrings
{
    public List<GamepadButtons> SprintInputs = new List<GamepadButtons>();
    public List<GamepadButtons> CrouchInputs = new List<GamepadButtons>();
    public List<GamepadButtons> InteractionInputs = new List<GamepadButtons>();
    public List<GamepadButtons> PauseInputs = new List<GamepadButtons>();
}

