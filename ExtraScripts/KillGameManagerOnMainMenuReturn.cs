using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillGameManagerOnMainMenuReturn : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (GameObject.FindObjectOfType<CheckpointValues>() != null)
        {
            GameObject gameManager = GameObject.FindObjectOfType<CheckpointValues>().gameObject;
            Destroy(gameManager);
        }

        CheckpointValues.CurrentState = GameState.FindClothes;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
}
