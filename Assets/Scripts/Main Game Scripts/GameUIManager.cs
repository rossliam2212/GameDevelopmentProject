using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour {

    private int score;
    private int lives = 3;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image lifeImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    [Header("Life Sprites")]
    [SerializeField] private Sprite threeLives;
    [SerializeField] private Sprite twoLives;
    [SerializeField] private Sprite oneLife;
    [SerializeField] private Sprite zeroLives;

    private void Start() {
        scoreText.text = "Score: " + 0;
        timeText.text = "Time: " + 0;
    }

    private void Update() {
        scoreText.text = "Score: " + score;
        //timeText.text = "Time: " + Time.time;
    }

    public void IncreaseScore(int amount) {
        score += amount;
    }

    public void RemoveLife() {
        lives--;
        switch (lives) {
            case 2:
                lifeImage.sprite = twoLives;
                break;
            case 1:
                lifeImage.sprite = oneLife;
                break;
            case 0:
                lifeImage.sprite = zeroLives;
                EndGame();
                break;
        }
    }

    private void EndGame() {
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + score;
    }
}
