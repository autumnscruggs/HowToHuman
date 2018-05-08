using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigfootSoundManager : MonoBehaviour
{
    private AudioClip previousClip;
    public List<AudioClip> footsteps = new List<AudioClip>();
    public List<AudioClip> runSteps = new List<AudioClip>();
    public AudioClip sneakSteps;

    public AudioClip disguiseRipping;
    public AudioClip puttingOnDisguise;
    public AudioClip walkingOnLeaves;

    //Private members
    private BigfootDisguiseState previousState;
    private float footstepTimer = 0;
    private float randomTime = 0;

    //References
    private AudioSource audioSource;
    private PlayerMovement movement;
    private Bigfoot bigfoot;

    private void Awake()
    {
        movement = this.GetComponent<PlayerMovement>();
        audioSource = this.GetComponent<AudioSource>();
        bigfoot = this.GetComponent<Bigfoot>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        MovementAudioStateMachine();
    }

    private void MovementAudioStateMachine()
    {
        if(movement.MovementState == MovementStates.Moving)
        {
             switch (movement.WalkState)
                {
                    case WalkStates.Normal:
                        RandomFootstep(footsteps, 0.1f, 1f);
                        break;
                    case WalkStates.Sprint:
                        RandomFootstep(runSteps, 0.3f, 0.5f);
                        break;
                    case WalkStates.Crouch:
                        if(movement.Input.MovementInputPushed) { PlaySoundConsistently(sneakSteps, 0.9f, 0.2f); }
                        break;
                    default:
                        break;
                }
        }
       
    }

    private void PlayOneShot(AudioClip clip, float volume = 1)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    private void PlaySoundConsistently(AudioClip clip, float time, float volume = 1)
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= time)
        {
            audioSource.PlayOneShot(clip, volume);
            footstepTimer = 0;
        }
    }

    private void RandomTimeToPlaySound(AudioClip clip, float min, float max, float volume = 1)
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= randomTime)
        {
            audioSource.PlayOneShot(clip, volume);

            randomTime = Random.Range(min, max);
            footstepTimer = 0;
        }
    }

    private void RandomFootstep(List<AudioClip> audioClips, float min, float max, float volume = 1)
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= randomTime)
        {
            AudioClip clip;
            do { clip = audioClips[Random.Range(0, audioClips.Count)]; }
            while (clip == previousClip);
             
            audioSource.PlayOneShot(clip, volume);

            randomTime = Random.Range(min, max);
            footstepTimer = 0;
        }
    }

    
}
