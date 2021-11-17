using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour {

    // Variables
    private int score = 0;
    private int lives = 3;
    private int goldCoinsCollected;

    public int goldCoinPoints = 10;

    [Header("Text Components:")]
    [SerializeField] private TextMeshProUGUI scoreText;
    //[SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI goldCoinCounter;
    [Space]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private GameObject greenArrowImage;
    [SerializeField] private GameObject redArrowImage;

    private int tempAmmo = 0;

    [Header("Other Components:")]
    [SerializeField] private Image lifeImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject keyImage;
    private Mage mage;
    private ArcherMovement archer;

    [Header("Life Sprites:")]
    [SerializeField] private Sprite threeLives;
    [SerializeField] private Sprite twoLives;
    [SerializeField] private Sprite oneLife;
    [SerializeField] private Sprite zeroLives;

    private void Start() {
        scoreText.text = "Score: " + score;
        //timeText.text = "Time: " + 0;
        goldCoinCounter.text = "0";

        ammoText.text = "Ammo: x7";
        greenArrowImage.SetActive(true);
        redArrowImage.SetActive(false);

        keyText.text = "Key: ?";
        keyImage.SetActive(false);

        mage = GameObject.FindObjectOfType(typeof(Mage)) as Mage;
        archer = GameObject.FindObjectOfType(typeof(ArcherMovement)) as ArcherMovement;
    }

    private void Update() {
        scoreText.text = "Score: " + score;
        goldCoinCounter.text = "" + goldCoinsCollected;
        ammoText.text = "Ammo: x" + archer.getAmmo();
    }

    /* This method increases the players score by a given amount. */
    public void IncreaseScore(int amount) {
        score += amount;
    }

    /* This method increments the gold coin counter by 1 every time a coin is collected */
    public void GoldCoinCounter() {
        goldCoinsCollected++;

        if (goldCoinsCollected % 5 == 0) {
            tempAmmo = archer.getAmmo();
            archer.setAmmo(0);

            greenArrowImage.SetActive(false);
            redArrowImage.SetActive(true);

            archer.setUpgradedBullet(true);
            archer.addAmmo(5);
        } else {
            if (!archer.getUpgradedBullet()) {
                archer.addAmmo(1);
            }
        }
    }

    public void EquipKey() {
        keyText.text = "Key: ";
        keyImage.SetActive(true);
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

    public void resetArrowImage() {
        greenArrowImage.SetActive(true);
        redArrowImage.SetActive(false);
    }

    public void resetAmmo() {
        archer.setAmmo(tempAmmo);
    }

    /* This method is used to end the game when the player has run of of lives. */
    /* *NOT FINISHED* */
    private void EndGame() {
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + score;
    }
}
