using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindAllObjectsInSceneWithScript : EditorWindow
{
    private static MonoScript[] monoScripts;
    private static List<string> nameList = new List<string>();
    private static List<string> filteredNameList = new List<string>();
    private static string filterWords;
    private static int selectedScript;
    private static int prevIndex = 0;
    private static List<GameObject> sceneObjects = new List<GameObject>();
    private static Vector2 scrollValue = Vector2.zero;


    [MenuItem("Window/Script Finder")]
    public static void ShowWindow()
    {
        Setup();
        prevIndex = selectedScript;
        //SelectScript(monoScripts[selectedScript]);
    }

    private static void Setup()
    {
        EditorWindow.GetWindow(typeof(FindAllObjectsInSceneWithScript));
        monoScripts = (MonoScript[])Resources.FindObjectsOfTypeAll(typeof(MonoScript));
        nameList = new List<string>();
        //Debug.Log("Length // " + monoScripts.Length);
        for (int x = 0; x < monoScripts.Length; x++)
        {
            nameList.Add(monoScripts[x].name);
            //Debug.Log("scriptNames[ " + x + "] // " + monoScripts[x].name);
        }
        nameList.Sort();

        filteredNameList = new List<string>(nameList);
       // Debug.Log("FilteredNameList Count // " + filteredNameList.Count);
    }

    void OnGUI()
    {
        // The actual window code goes here

        //Setup();

        EditorGUILayout.BeginHorizontal();
        filterWords = EditorGUILayout.TextField(filterWords);
        if(GUILayout.Button("Filter List"))
        {
            if (!string.IsNullOrEmpty(filterWords))
            {
                foreach (string s in nameList)
                {
                    if (!s.ToLower().Contains(filterWords.ToLower()) && filteredNameList.Contains(s))
                    {
                        filteredNameList.Remove(s);
                    }
                    else if (!filteredNameList.Contains(s) && s.ToLower().Contains(filterWords.ToLower()))
                    {
                        filteredNameList.Add(s);
                    }
                }
            }
            else
            {
                filteredNameList = new List<string>(nameList);
            }

        }
        EditorGUILayout.EndHorizontal();

        selectedScript = EditorGUILayout.Popup(selectedScript, filteredNameList.ToArray());

        if(GUILayout.Button("Show All Objects With Script"))
        {
            MonoScript s = System.Array.Find(monoScripts, x => x.name == filteredNameList[selectedScript]);
            SelectScript(s);
        }

        scrollValue = EditorGUILayout.BeginScrollView(scrollValue);
        GUI.backgroundColor = Color.cyan;

        if (sceneObjects.Count > 0 && sceneObjects != null)
        {
            for(int y = 0; y < sceneObjects.Count; y++)
            {
                //Debug.Log("Name : " + sceneObjects[y].name);

                if (GUILayout.Button(sceneObjects[y].name))
                {
                    Selection.activeObject = sceneObjects[y];
                    EditorGUIUtility.PingObject(sceneObjects[y]);
                }
            }
        }

        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndScrollView();

        prevIndex = selectedScript;
    }

    static void SelectScript(MonoScript type)
    {
        //Debug.Log("Selected Type: " + _type.name);

        sceneObjects = new List<GameObject>();
        //        Debug.Log(GameObject.FindSceneObjectsOfType(typeof(GameObject)).Length);

        GameObject[] objs = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach(GameObject obj in objs)
        {
            if (obj.GetComponent(type.name))
            {
                sceneObjects.Add(obj);
            }
        }


        Selection.objects = sceneObjects.ToArray();
    }

}
