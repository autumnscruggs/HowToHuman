using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigfootRandomIdles : MonoBehaviour
{
    private BigfootAnimations animations;
    public List<string> AnimationNames = new List<string>();
    public float TimeBeforeIdles = 5f;
    public float TimeBetweenIdles = 2f;
    private bool coroutinePlaying = false;

    private int prevAnimation = -1;
    private int randomAnimation = 0;

    public CameraAnimation cameraAnimation;
    [SerializeField] private bool canStartIdle = false;
    private bool setToStill = false;

    private void OnEnable()
    {
        NPC.DetectedBigfootEvent += CantStartIdle;
        IntroCameraScript.CutsceneComplete += CanStartIdle;
    }
    private void OnDisable()
    {
        NPC.DetectedBigfootEvent -= CantStartIdle;
        IntroCameraScript.CutsceneComplete -= CanStartIdle;
    }
    private void CantStartIdle(object sender, System.EventArgs e)
    {
        setToStill = false;
        canStartIdle = false;
    }

    private void CanStartIdle(object sender, System.EventArgs e)
    {
        setToStill = true;
        CutsceneManagement.InCutscene = false;
        canStartIdle = true;
    }

    private void Start()
    {
        animations = this.GetComponent<BigfootAnimations>();
    }

    private void Update()
    {
        //Debug.Log("CutsceneManagement.InCutscene // " + CutsceneManagement.InCutscene);
        canStartIdle = !CutsceneManagement.InCutscene;

        if (canStartIdle)
        {
            if (!cameraAnimation.gameObject.activeInHierarchy) { cameraAnimation.gameObject.SetActive(true); }

            if (Input.anyKey || MouseInput.IsMovingMouse() || animations.Movement.WalkState == WalkStates.Crouch)
            {
                if (coroutinePlaying) { StopAllCoroutines(); coroutinePlaying = false; if (setToStill) { animations.Animator.Play("Still"); } cameraAnimation.ReturnToBigfootBack(); }
            }
            else
            {
                if (!coroutinePlaying) { StartCoroutine(RandomAnimation()); }
            }
        }
        else
        {
            if (cameraAnimation.gameObject.activeInHierarchy) { cameraAnimation.gameObject.SetActive(false); }
            coroutinePlaying = false;
            StopAllCoroutines();
        }
    }

    IEnumerator RandomAnimation()
    {
        coroutinePlaying = true;

        yield return new WaitForSeconds(TimeBeforeIdles);

        cameraAnimation.AnimateInFrontOfBigfoot();

        yield return new WaitForSeconds(TimeBetweenIdles);

        while (true)
        {
            do
            {
                randomAnimation = Random.Range(0, AnimationNames.Count);
            }
            while (randomAnimation == prevAnimation);

            prevAnimation = randomAnimation;

            animations.Animator.Play(AnimationNames[randomAnimation]);
            yield return new WaitForEndOfFrame();
            //Debug.Log("Length // " + animations.Animator.GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSeconds(animations.Animator.GetCurrentAnimatorStateInfo(0).length);
            //Debug.Log("After animation length time");
            yield return new WaitForSeconds(TimeBetweenIdles);

        }

    }
}
