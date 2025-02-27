using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource[] _musicSources;
    public int musicSourceIndex = 0;
    public AudioSource sfxSource;
    public Song[] songs;

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
        this.currentMusic = Array.Find(musicSounds, sound => sound.name == name);
        if (this.currentMusic == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        goalTime = AudioSettings.dspTime + 0.5;
        _musicSources[1].clip = this.currentMusic.clip;
        _musicSources[1].PlayScheduled(goalTime);

        musicDuration = (double)this.currentMusic.clip.samples / this.currentMusic.clip.frequency;
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
        string nextTrackName = NextTrack();
        Debug.Log("PlayScheduledClip() searching for: " + nextTrackName);
        this.currentMusic = Array.Find(musicSounds, sound => sound.name == nextTrackName);

        if (this.currentMusic == null)
        {
            Debug.LogError("Music track " + nextTrackName + " not found!");
            return;
        }


        _musicSources[musicSourceIndex].clip = this.currentMusic.clip;
        _musicSources[musicSourceIndex].PlayScheduled(goalTime);

        musicDuration = (double)this.currentMusic.clip.samples / this.currentMusic.clip.frequency;
        goalTime = goalTime + musicDuration;

        musicSourceIndex = 1 - musicSourceIndex;
    }

    public string NextTrack()
    {
        if (musicSounds == null || musicSounds.Length == 0)
        {
            Debug.LogError("No music sounds defined!");
            return null;
        }

        int index = random.Next(0, musicSounds.Length - 1);

        if (musicSounds[index] == null || musicSounds[index].name == null)
        {
            Debug.LogError($"Invalid music sound at index {index}");
            return musicSounds[0].name;
        }

        Debug.Log("NextTrack() returning: " + musicSounds[index].name);
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

    public void StopMusic() {
        _musicSources[musicSourceIndex].Stop();
    }


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
