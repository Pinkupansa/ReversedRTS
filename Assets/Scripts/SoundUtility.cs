using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundUtility
{
    public static void PlayLooping(AudioSource source, AudioClip clip, float volume = 1f, bool isPriority = false)
    {
        if (!isPriority && source.isPlaying)
        {
            return;
        }
        source.volume = volume;
        source.clip = clip;
        source.loop = true;
        source.Play();
    }

    public static void PlayOneShot(AudioSource source, AudioClip clip, float volume = 1f, bool isPriority = false)
    {
        if (!isPriority && source.isPlaying)
        {
            return;
        }
        source.volume = volume;
        source.clip = clip;
        source.loop = false;
        source.Play();
    }

    public static void PlayRandomFromArrayLoop(AudioSource source, AudioClip[] clips, float volume = 1f, bool isPriority = false)
    {
        if (!isPriority && source.isPlaying)
        {
            return;
        }
        source.volume = volume;
        source.clip = clips[Random.Range(0, clips.Length)];
        source.loop = true;
        source.Play();
    }

    public static void PlayRandomFromArrayOneShot(AudioSource source, AudioClip[] clips, float volume = 1f, bool isPriority = false)
    {
        if (!isPriority && source.isPlaying)
        {
            return;
        }
        source.volume = volume;
        source.clip = clips[Random.Range(0, clips.Length)];
        source.loop = false;
        source.Play();
    }

    public static void CancelSound(AudioSource source, AudioClip clip)
    {
        if (source.clip == clip)
        {
            source.Stop();
        }
    }
    public static void CancelOneOfSounds(AudioSource source, AudioClip[] clips)
    {
        foreach (AudioClip clip in clips)
        {
            if (source.clip == clip)
            {
                source.Stop();
                return;
            }
        }
    }
    public static void PlayImmediately(AudioSource source, AudioClip clip, float volume = 1f, bool isPriority = false)
    {
        if (!isPriority && source.isPlaying)
        {
            return;
        }
        source.volume = volume;
        source.PlayOneShot(clip);
    }
}
