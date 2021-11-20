using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    /* Code From: https://www.youtube.com/watch?v=6OT43pvUyfY */

    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [HideInInspector] public AudioSource source;
}
