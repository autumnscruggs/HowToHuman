using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(SubtitleManager))]
public class SubtitleManagerEditor : Editor
{
    SerializedProperty ThisList;
    int ListSize;
    Vector2 scrollPos;
    bool ShowList = true;

    void OnEnable()
    {
        ThisList = this.serializedObject.FindProperty("Subtitles"); // Find the List in our script and create a refrence of it
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        this.serializedObject.Update();

        EditorGUI.indentLevel++;


        //Resize our list
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));

        ListSize = ThisList.arraySize;

        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
        foldoutStyle.fontStyle = FontStyle.Bold;
        foldoutStyle.fontSize = 12;
        foldoutStyle.margin = new RectOffset(2, 0, 0, 0);
        ShowList = EditorGUILayout.Foldout(ShowList, "Subtitles", foldoutStyle);
        //EditorGUILayout.LabelField("Subtitles", EditorStyles.boldLabel, GUILayout.MaxWidth(40));
        ListSize = EditorGUILayout.IntField(ListSize);

        if (ListSize != ThisList.arraySize)
        {
            while (ListSize > ThisList.arraySize)
            {
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            }
            while (ListSize < ThisList.arraySize)
            {
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (ShowList)
        {
            float width = Screen.width - 20;
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, 
                false, GUILayout.Width(width), GUILayout.Height(500));

            float label = 90;
            float prop = 200;


            //Display our list to the inspector window

            for (int i = 0; i < ThisList.arraySize; i++)
            {
                //EditorGUI.indentLevel++;

                SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
                //id
                SerializedProperty ID = MyListRef.FindPropertyRelative("ID");
                ID.intValue = i;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ID:", EditorStyles.boldLabel, GUILayout.MaxWidth(50));
                EditorGUILayout.LabelField(ID.intValue.ToString());
                EditorGUILayout.EndHorizontal();

                //Character
                SerializedProperty Character = MyListRef.FindPropertyRelative("Character");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Character:", GUILayout.MaxWidth(label));
                EditorGUILayout.PropertyField(Character, new GUIContent(""), GUILayout.MaxWidth(prop));
                EditorGUILayout.EndHorizontal();


                //audioclip
                SerializedProperty AudioClip = MyListRef.FindPropertyRelative("AudioClip");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Audio Clip:", GUILayout.MaxWidth(label));
                EditorGUILayout.PropertyField(AudioClip, new GUIContent(""), GUILayout.MaxWidth(prop));
                EditorGUILayout.EndHorizontal();

                //DialogueText
                EditorStyles.textField.wordWrap = true;
                SerializedProperty DialogueText = MyListRef.FindPropertyRelative("DialogueText");
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Dialogue Text:");
                DialogueText.stringValue = EditorGUILayout.TextArea(DialogueText.stringValue, 
                    GUILayout.MinHeight(80), GUILayout.MaxWidth(290));
                EditorGUILayout.EndVertical();
                EditorStyles.textField.wordWrap = false;
                //UpTime
                SerializedProperty UpTime = MyListRef.FindPropertyRelative("UpTime");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Up Time:", GUILayout.MaxWidth(label));
                EditorGUILayout.PropertyField(UpTime, new GUIContent(""), GUILayout.MaxWidth(prop));
                EditorGUILayout.EndHorizontal();
                //CloseAfter
                SerializedProperty CloseAfter = MyListRef.FindPropertyRelative("CloseAfter");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Close After:", GUILayout.MaxWidth(label));
                EditorGUILayout.PropertyField(CloseAfter, new GUIContent(""), GUILayout.MaxWidth(prop));
                EditorGUILayout.EndHorizontal();
                //NextID
                SerializedProperty NextID = MyListRef.FindPropertyRelative("NextID");
                if (CloseAfter.boolValue == false)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Next ID:", GUILayout.MaxWidth(label));
                    EditorGUILayout.PropertyField(NextID, new GUIContent(""), GUILayout.MaxWidth(prop));
                    EditorGUILayout.EndHorizontal();
                }


                //EditorGUI.indentLevel--;

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.Space();
 
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndScrollView();
        }

        this.serializedObject.ApplyModifiedProperties();
    }

}
