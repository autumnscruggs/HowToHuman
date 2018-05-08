#if DEBUG
using UnityEngine;
using UnityEditor;

using System.Collections;


[InitializeOnLoad]
public class GPMOrderFix : Editor
{
    static GPMOrderFix()
    {
        SetExecutionOrderToBeFirst();
        //CreateObjectIfDoesntExist();
    }

    public GPMOrderFix()
    {
    }

    private static void SetExecutionOrderToBeFirst()
    {
        string[] gamepadManagerAssets = AssetDatabase.FindAssets("GamepadManager t:MonoScript");
        string gamepadManager = AssetDatabase.GUIDToAssetPath(gamepadManagerAssets[0]);
        MonoScript monoScript =  AssetDatabase.LoadAssetAtPath(gamepadManager, typeof(MonoScript)) as MonoScript;

        int currentExecutionOrder = MonoImporter.GetExecutionOrder(monoScript);

        if (currentExecutionOrder != -100)  {  MonoImporter.SetExecutionOrder(monoScript, -100); }
    }

    private static void CreateObjectIfDoesntExist()
    {
        if (GameObject.Find("GamepadInputManager") == null)
        {
            GameObject go = new GameObject("GamepadInputManager");
            go.transform.position = Vector3.zero;
            go.AddComponent<GamepadManager>();
        }
    }

}
#endif
