using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    public static AsyncLoader Instance { get; private set; }
    private AsyncOperation async;
    public bool CanLoad { get; private set; }

    private void Awake()
    {
        CanLoad = true;

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        DetectionCameraManager.DetectionStarted += StartReloadAfterDetection;
        DetectionCameraManager.DetectionEnded += FinishLoad;
    }

    private void OnDisable()
    {
        DetectionCameraManager.DetectionStarted -= StartReloadAfterDetection;
        DetectionCameraManager.DetectionEnded -= FinishLoad;
    }

    private void StartReloadAfterDetection(object sender, System.EventArgs e)
    {
        StartLoading("PresentScene");
    }

    private void FinishLoad(object sender, System.EventArgs e)
    {
        ActivateScene();
    }

    public void StartLoading(string levelName)
    {
        StartCoroutine(Load(levelName));
    }

    IEnumerator Load(string levelName)
    {
        //Debug.LogWarning("ASYNC LOAD STARTED - " +
        //   "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        async = Application.LoadLevelAsync(levelName);
        async.allowSceneActivation = false;
        yield return async;
    }

    public void ActivateScene()
    {
        async.allowSceneActivation = true;
    }
}
