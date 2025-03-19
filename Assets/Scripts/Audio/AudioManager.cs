using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private float masterVolume = 0.5f; // Default full volume
    private float musicVolume = 0.2f;
    private float sfxVolume = 1.0f;

    public Slider masterSlider, musicSlider, sfxSlider;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("BGM");

        if (masterSlider) masterSlider.value = masterVolume;
        if (musicSlider) musicSlider.value = musicVolume;
        if (sfxSlider) sfxSlider.value = sfxVolume;
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.volume = musicVolume * masterVolume; // Apply master volume
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip, sfxVolume * masterVolume); // Apply master volume
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume * masterVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }
}
