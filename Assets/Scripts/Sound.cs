using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public Sound(string name, AudioClip clip)
    {
        this.name = name;
        this.clip = clip;
    }

    public void SetClip(AudioClip clip)
    {
        this.clip = clip;
    }
}
