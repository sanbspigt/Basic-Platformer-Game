using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
        
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayMusic(AudioClip musicClip,bool isLoop = true)
    {
        musicSource.clip = musicClip;
        musicSource.loop = isLoop;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;        
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}
