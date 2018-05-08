using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigfootWaveHack : MonoBehaviour
{
    private Animator bigfootAnimator;
    private PlayerMovement movement;
    private IntroCameraScript cameraScript;

    private void OnEnable()
    {
        IntroCameraScript.CutsceneComplete += StopWaitAndPlayWave;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        IntroCameraScript.CutsceneComplete -= StopWaitAndPlayWave;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (cameraScript == null) { cameraScript = GameObject.FindObjectOfType<IntroCameraScript>(); }
        if (movement == null) { movement = this.GetComponentInParent<PlayerMovement>(); }
        if (bigfootAnimator == null) { bigfootAnimator = this.GetComponent<Animator>(); }
        if(cameraScript.CameraAnimation == null) { cameraScript.CameraAnimation = cameraScript.IntroCamera.GetComponent<Animation>(); }

        float delayTime = 0;

        if (CheckpointValues.CurrentState == GameState.FindClothes)
        {
            delayTime = cameraScript.CameraAnimation.clip.length;
        }

        StartCoroutine(WaveDelay(delayTime));
    }

    public void StopWaitAndPlayWave(object sender, System.EventArgs e)
    {
        StopCoroutine("WaveDelay");
        StartCoroutine(WaveDelay(0));
    }

    IEnumerator WaveDelay(float delayTime)
    {
        movement.CanMove = false;
        yield return new WaitForSeconds(delayTime);
        bigfootAnimator.Play("Wave");
        yield return new WaitForSeconds(3);
        bigfootAnimator.Play("Still");
        movement.CanMove = true;
    }
}
