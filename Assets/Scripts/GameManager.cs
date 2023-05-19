using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public static class GameSounds {
    public static readonly string reflect = "reflect";
    public static readonly string breakBrick = "break";
}
public class GameManager : MonoBehaviour
{
    public static GameManager game;
    [SerializeField] private Sound[] sounds;
    [ReadOnly] public List<AudioSource> audioSrcs;
    [SerializeField] private GameObject ball;

    [Button("Populate Sound")]
    public void PopulateSounds() {
        foreach(Sound s in sounds) {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            s.source = newAudioSource;
            audioSrcs.Add(newAudioSource);
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = s.playOnAwake;
        }
    }
    [Button("Clear Sounds")]
    public void ClearSound() {
        foreach(AudioSource src in audioSrcs) {
            DestroyImmediate(src);
        }
        audioSrcs.Clear();
    }

    private void Awake() {
        game = this;

    }
    public void EndGame() {
        Time.timeScale = 0;
    }

    public void LevelWon() {
        print("won");
        Destroy(ball);
        Time.timeScale = 0;
    }

    public void playSound(string name) {
        Sound currentSound = null;

        foreach(Sound s in sounds) {
            if(s.name == name) {
                currentSound = s;
                currentSound.source.Play();
                return;
            }
        }

        if(currentSound == null) {
            Debug.LogError($"Sound: {name} not found!");
            return;
        }
    }
}
