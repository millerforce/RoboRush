using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections;
using System.Diagnostics.Contracts;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    public Sound[] sfxSounds, ambientSounds;
    public AudioSource[] _musicSources;
    public int musicSourceIndex = 0;
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    public bool playAmbient = false;
    public bool playMusic = true;
    public Song[] songs;

    public Sound currentMusic;
    public Song currentSong;
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
        if (playMusic)
        {
            int randomSong = random.Next(1, songs.Length);
            Debug.Log("Song " + songs[randomSong].name + " playing");
            PlayMusic(songs[randomSong].name);
        }
        if (playAmbient)
        {
            PlayAmbient(ambientSounds[0].name);
        }
    }

    public void PlayMusic()
    {
        int randomSound = random.Next(1, currentSong.sounds.Length);
        this.currentMusic = currentSong.sounds[randomSound];
        if (this.currentMusic == null)
        {
            Debug.LogWarning("Music: " + currentSong.sounds[0].name + " not found!");
            return;
        }
        goalTime = AudioSettings.dspTime + 0.5;
        _musicSources[1].clip = this.currentMusic.clip;
        _musicSources[1].PlayScheduled(goalTime);
        musicDuration = (double)this.currentMusic.clip.samples / this.currentMusic.clip.frequency;
        goalTime = goalTime + musicDuration;
    }

    public void PlayMusic(string name)
    {
        this.currentSong = Array.Find(songs, song => song.name == name);

        if (this.currentSong == null)
        {
            Debug.LogWarning("Song: " + name + " not found!");
            return;
        }

        this.currentMusic = currentSong.sounds[0];
        if (this.currentMusic == null)
        {
            Debug.LogWarning("Music: " + currentSong.sounds[0].name + " not found!");
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
        if (playMusic && (AudioSettings.dspTime > goalTime - 2))
        {
            int timeToChange = random.Next(0, 30);
            if (timeToChange == 2)
            {
                int randomSong = random.Next(1, songs.Length);
                Debug.Log("Song " + songs[randomSong].name + " playing");
                PlayMusic(songs[randomSong].name);
                return;
            }
            PlayScheduledClip();
        }
    }

    private void PlayScheduledClip()
    {
        string nextTrackName = NextTrack();
        Debug.Log("PlayScheduledClip() searching for: " + nextTrackName);
        this.currentMusic = Array.Find(currentSong.sounds, sound => sound.name == nextTrackName);

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
        if (currentSong == null || currentSong.sounds.Length == 0)
        {
            Debug.LogError("No music sounds defined!");
            return null;
        }

        int index = random.Next(1, currentSong.sounds.Length);

        if (currentSong.sounds[index] == null || currentSong.sounds[index].name == null)
        {
            Debug.LogError($"Invalid music sound at index {index}");
            return currentSong.sounds[0].name;
        }

        Debug.Log("NextTrack() returning: " + currentSong.sounds[index].name);
        return currentSong.sounds[index].name;
    }

    //public void SetCurrentMusic(string name)
    //{
    //    Sound s = Array.Find(musicSounds, sound => sound.name == NextTrack());
    //    _musicSources[musicSourceIndex].clip = s.clip;
    //}

    public void PlayAmbient(string name)
    {
        Sound s = Array.Find(ambientSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("SFX: " + name + " not found!");
            return;
        }
        ambientSource.clip = s.clip;
        ambientSource.loop = true;
        ambientSource.Play();
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
        foreach (var source in _musicSources)
        {
            source.Stop();
        }
    }


    public void SetMusicVolume(float volume) {
        ambientSource.volume = volume; // Setting the ambient source volume here for now
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
