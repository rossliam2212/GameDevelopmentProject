using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    /* Code From: https://www.youtube.com/watch?v=6OT43pvUyfY */

    public Sound[] sounds;
    private int currentLevel;

    private void Awake() {
        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start() {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        
        switch(currentLevel) {
            case 0: 
            case 1:
                Play("MainMusic");
                break;
            case 2:
                Play("CastleMusic");
                break;
            case 3:
                Play("BossMusic");
                break;
            case 4:
                Play("VictoryMusic");
                break;
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

    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) {
            Debug.LogWarning("Error! Sound: " + name + " was not found!");
            return;
        }

        s.source.Stop();
    }
}
