using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource[] _musicSources;
    public int musicSourceIndex;
    public AudioSource sfxSource;

    public Sound currentMusic;
    public double musicDuration;
    public double goalTime;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private System.Random random = new System.Random();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        PlayMusic(musicSounds[0].name);
    }

    public void PlayMusic(string name)
    {
        currentMusic = Array.Find(musicSounds, sound => sound.name == name);
        if (currentMusic == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        goalTime = AudioSettings.dspTime + 0.5;
        _musicSources[0].clip = currentMusic.clip;
        _musicSources[0].PlayScheduled(goalTime);

        musicDuration = (double)currentMusic.clip.samples / currentMusic.clip.frequency;
        goalTime = goalTime + musicDuration;
    }

    private void Update()
    {
        if (AudioSettings.dspTime > goalTime - 2)
        {
            PlayScheduledClip();
        }
    }

    private void PlayScheduledClip()
    {
        currentMusic = Array.Find(musicSounds, sound => sound.name == NextTrack());

        _musicSources[musicSourceIndex].clip = currentMusic.clip;
        _musicSources[musicSourceIndex].PlayScheduled(goalTime);

        musicDuration = (double)currentMusic.clip.samples / currentMusic.clip.frequency;
        goalTime = goalTime + musicDuration;

        musicSourceIndex = 1 - musicSourceIndex;
    }

    public string NextTrack()
    {
        int index = random.Next(0, musicSounds.Length - 1);
        return musicSounds[index].name;
    }

    public void SetCurrentMusic(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == NextTrack());
        _musicSources[musicSourceIndex].clip = s.clip;
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("SFX: " + name + " not found!");
            return;
        }
        sfxSource.PlayOneShot(s.clip);
    }


    //private void Awake() {
    //    if (instance == null) {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject); // Persist between scenes
    //    }
    //    else {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    LoadVolumeSettings();
    //}

    //public void PlayMusic(AudioClip clip) {
    //    if (musicSource.clip != clip) {
    //        musicSource.clip = clip;
    //        musicSource.Play();
    //    }
    //}

    //public void StopMusic() {
    //    musicSource.Stop();
    //}

    //public void PlaySFX(AudioClip clip) {
    //    foreach (var source in sfxSources) {
    //        if (!source.isPlaying) // Play on the first available source
    //        {
    //            source.clip = clip;
    //            source.Play();
    //            return;
    //        }
    //    }
    //}

    public void SetMusicVolume(float volume) {
        foreach (var source in _musicSources)
        {
            source.volume = volume;
        }
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetSFXVolume(float volume) {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    //private void LoadVolumeSettings() {
    //    musicSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, VolumeSettings.DEFAULT_MUSIC_VOLUME);
    //    foreach (var source in sfxSources) {
    //        source.volume = PlayerPrefs.GetFloat(SFXVolumeKey, VolumeSettings.DEFAULT_SFX_VOLUME);
    //    }
    //}

}
