using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Debugger))]
public class DebuggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var debugger = (Debugger)this.target;
        base.OnInspectorGUI();

        if(GUILayout.Button("Set State"))
        {
            debugger.SetState();
        }
    }
}
