using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour {
    public Slider musicSlider; // The slider for music volume
    public Slider sfxSlider;   // The slider for sound effect volume

    public static float DEFAULT_MUSIC_VOLUME = 0.25f;
    public static float DEFAULT_SFX_VOLUME = 0.25f;

    private void Start() {
        // Load the saved volume settings from PlayerPrefs (if they exist)
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", DEFAULT_MUSIC_VOLUME); // Default if not set
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", DEFAULT_SFX_VOLUME);   // Default if not set
    }

    public void SetMusicVolume(float volume) {
        AudioManager.instance.SetMusicVolume(volume); // Update the music volume
        PlayerPrefs.SetFloat("MusicVolume", volume);   // Save the setting
    }

    public void SetSFXVolume(float volume) {
        AudioManager.instance.SetSFXVolume(volume); // Update the SFX volume
        PlayerPrefs.SetFloat("SFXVolume", volume);   // Save the setting
    }
}
