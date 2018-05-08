using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDad : MonoBehaviour
{
    public GameObject grillDad;
    public GameObject normalDad;

    private void Awake()
    {
        TurnOnNormalDad();
    }

    public void TurnOnGrillDad()
    {
        grillDad.SetActive(true);
        grillDad.GetComponent<DadAndSpatulaAnimations>().PlayFlipping();
        StartCoroutine(ReturnToIdle());
        normalDad.SetActive(false);
    }

    public void TurnOnNormalDad()
    {
        grillDad.SetActive(false);
        normalDad.SetActive(true);
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(0.2f);
        grillDad.GetComponent<DadAndSpatulaAnimations>().PlayIdle();
    }
}
