using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private AudioSource source;
    public List<AudioClip> soundClips = new List<AudioClip>();

    public void PlayOnce(string name)
    {
        source.PlayOneShot(soundClips.Find(x => x.name == name));
    }

    public void PlayOnce(int index)
    {
        source.PlayOneShot(soundClips[index]);
    }

    public void PlayDelayed(string name, float delay)
    {
        source.clip = soundClips.Find(x => x.name == name);
        source.PlayDelayed(delay);
    }
    
    public void PlayDelayed(int index, float delay)
    {
        source.clip = soundClips[index];
        source.PlayDelayed(delay);
    }
}


