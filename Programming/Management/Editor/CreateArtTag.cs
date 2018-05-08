using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CreateArtTag : Editor
{
    static CreateArtTag()
    {
        CreateLayer();
    }

    static void CreateLayer()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        SerializedProperty layers = tagManager.FindProperty("layers");
        if (layers == null || !layers.isArray)
        {
            Debug.LogWarning("Can't set up the layers.  It's possible the format of the layers and tags data has changed in this version of Unity.");
            Debug.LogWarning("Layers is null: " + (layers == null));
            return;
        }

        SerializedProperty layerSP = layers.GetArrayElementAtIndex(29);

            if (layerSP.stringValue != "Art")
            {
                Debug.Log("Setting up layers.  Layer " + 29 + " is now called " + "Art");
                layerSP.stringValue = "Art";
            }

            tagManager.ApplyModifiedProperties();
        }
}
 
