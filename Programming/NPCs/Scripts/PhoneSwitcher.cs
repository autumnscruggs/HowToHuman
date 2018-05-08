using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneSwitcher : MonoBehaviour
{
    private NPCAnimator animator;

    public GameObject phoneWalk;
    public GameObject phoneIdle;

    private void Awake()
    {
        animator = this.GetComponent<NPCAnimator>();
        phoneWalk.SetActive(false);
        phoneIdle.SetActive(true);
    }

    private void Start()
    {
        if (animator != null && animator.NpcMove != null) { animator.NpcMove.MoveStateChanged += SwitchPhones; }
    }

    private void SwitchPhones(MovementStateArgs e)
    {
        StartCoroutine(SwitchDelay(e));
    }

    public void TurnOffIdlePhone()
    {
        phoneWalk.SetActive(false);
        phoneIdle.SetActive(false);
    }

    public void TurnOnIdlePhone()
    {
        phoneWalk.SetActive(false);
        phoneIdle.SetActive(true);
    }

    IEnumerator SwitchDelay(MovementStateArgs e)
    {
        yield return new WaitForEndOfFrame();

        switch (e.MovementState)
        {
            case MovementStates.Idle:
                phoneWalk.SetActive(false);
                phoneIdle.SetActive(true);
                break;
            case MovementStates.Moving:
                phoneWalk.SetActive(true);
                phoneIdle.SetActive(false);
                break;
            default:
                break;
        }
    }

}
