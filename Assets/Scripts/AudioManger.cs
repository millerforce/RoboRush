using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    public AudioClip clip;
    public AudioSource musicSource;
    public AudioSource[] sfxSources;

    private void Start() {
        PlayMusic(clip);
    }

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else {
            Destroy(gameObject);
            return;
        }

        LoadVolumeSettings();
    }

    public void PlayMusic(AudioClip clip) {
        if (musicSource.clip != clip) {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic() {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip) {
        foreach (var source in sfxSources) {
            if (!source.isPlaying) // Play on the first available source
            {
                source.clip = clip;
                source.Play();
                return;
            }
        }
    }

    public void SetMusicVolume(float volume) {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetSFXVolume(float volume) {
        foreach (var source in sfxSources) {
            source.volume = volume;
        }
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    private void LoadVolumeSettings() {
        musicSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, VolumeSettings.DEFAULT_MUSIC_VOLUME);
        foreach (var source in sfxSources) {
            source.volume = PlayerPrefs.GetFloat(SFXVolumeKey, VolumeSettings.DEFAULT_SFX_VOLUME);
        }
    }

}
