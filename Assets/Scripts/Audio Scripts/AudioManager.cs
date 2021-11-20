using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    /* Code From: https://www.youtube.com/watch?v=6OT43pvUyfY */

    public Sound[] sounds;

    private void Awake() {
        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            Debug.LogWarning("Error! Sound: " + name + " was not found!");
            return;
        }

        s.source.Play();
    }
}
