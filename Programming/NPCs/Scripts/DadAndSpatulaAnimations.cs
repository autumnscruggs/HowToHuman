using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadAndSpatulaAnimations : MonoBehaviour
{
    public Animator Dad;
    public Animator Spatula;

    private void OnEnable()
    {
        PlayIdle();
        ArchingProjectileSpawner.SayingVoiceLine += Flip;
    }

    private void OnDisable()
    {
        ArchingProjectileSpawner.SayingVoiceLine -= Flip;
    }

    private void Flip(object sender, System.EventArgs e)
    {
        PlayFlipping();
    }

    public void PlayIdle()
    {
        Dad.Play("Idle");
        Spatula.Play("Idle");
    }

    public void PlayFlipping()
    {
        Dad.Play("Flipping");
        Spatula.Play("Flipping");
    }

    public void PlayFull()
    {
        Dad.Play("Full");
        Spatula.Play("Full");
    }
}
