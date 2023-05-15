using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager game;

    [SerializeField] private GameObject ball;

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
}
