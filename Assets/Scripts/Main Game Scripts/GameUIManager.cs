using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour {

    // Variables
    private int score;
    private int lives = 3;
    private int goldCoinsCollected;

    public int goldCoinPoints = 10;

    [Header("Text Components:")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI goldCoinCounter;

    [Header("Other Components:")]
    [SerializeField] private Image lifeImage;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Life Sprites:")]
    [SerializeField] private Sprite threeLives;
    [SerializeField] private Sprite twoLives;
    [SerializeField] private Sprite oneLife;
    [SerializeField] private Sprite zeroLives;

    private void Start() {
        scoreText.text = "Score: " + 0;
        timeText.text = "Time: " + 0;
        goldCoinCounter.text = "0";
    }

    private void Update() {
        scoreText.text = "Score: " + score;
        //timeText.text = "Time: " + Time.time;
        goldCoinCounter.text = "" + goldCoinsCollected;
    }

    /* This method increases the players score by a given amount. */
    public void IncreaseScore(int amount) {
        score += amount;
    }

    /* This method increments the gold coin counter by 1 every time a coin is collected */
    public void GoldCoinCounter() {
        goldCoinsCollected++;
    }

    /* This method removes a life from the player and changes the lifeImage sprite depending on how mnay lives are left. */
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

    /* This method is used to end the game when the player has run of of lives. */
    /* *NOT FINISHED* */
    private void EndGame() {
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + score;
    }
}
