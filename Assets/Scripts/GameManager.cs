using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.SceneManagement;

public static class GameSounds {
    public static readonly string reflect = "reflect";
    public static readonly string breakBrick = "break";
}
public class GameManager : MonoBehaviour
{
    public static GameManager game;
    [SerializeField] private GameObject ball;

    [Header("UI")]
    [SerializeField] TMP_Text livesText;
    [SerializeField] GameObject WinContainer;

    [Header("AUDIO")]
    [SerializeField] private Sound[] sounds;
    [ReadOnly] public List<AudioSource> audioSrcs;


    private void Awake() {
        game = this;

    }

    private void Start() {
        livesText.text = PlayerController.instance.currentTries.ToString();
    }

    private void LateUpdate() {
        livesText.text = PlayerController.instance.currentTries.ToString();
    }

    public void RestartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel() {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
    public void EndGame() {
        Time.timeScale = 0;
    }

    public void LevelWon() {
        WinContainer.SetActive(true);
        Destroy(ball);
        Time.timeScale = 0;
    }

    #region Sound
    [Button("Populate Sound")]
    public void PopulateSounds() {
        foreach (Sound s in sounds) {
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
        foreach (AudioSource src in audioSrcs) {
            DestroyImmediate(src);
        }
        audioSrcs.Clear();
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
    #endregion
}
