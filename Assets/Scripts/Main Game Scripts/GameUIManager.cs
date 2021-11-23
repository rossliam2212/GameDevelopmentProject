using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour {

    // Variables
    private int score = 0;
    private int lives = 3;
    private int goldCoinsCollected;

    public int goldCoinPoints = 10;

    [Header("Text Components:")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI goldCoinCounter;
    [SerializeField] private TextMeshProUGUI bossHealthText;
    [Space]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private GameObject greenArrowImage;
    [SerializeField] private GameObject redArrowImage;
    //[SerializeField] private GameObject keyContainer;

    private int tempAmmo = 0;
    private int currentLevel;
    private int nextLevel;

    [Header("Other Components:")]
    [SerializeField] private Image lifeImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject keyImage;
    private Mage mage;
    private ArcherMovement archer;
    private AudioManager audioManager;
    private LevelChanger levelChanger;
    private Boss boss;

    [Header("Life Sprites:")]
    [SerializeField] private Sprite threeLives;
    [SerializeField] private Sprite twoLives;
    [SerializeField] private Sprite oneLife;
    [SerializeField] private Sprite zeroLives;

    private void Start() {
        mage = GameObject.FindObjectOfType(typeof(Mage)) as Mage;
        archer = GameObject.FindObjectOfType(typeof(ArcherMovement)) as ArcherMovement;
        audioManager = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
        levelChanger = GameObject.FindObjectOfType(typeof(LevelChanger)) as LevelChanger;
        boss = GameObject.FindObjectOfType(typeof(Boss)) as Boss;

        currentLevel = SceneManager.GetActiveScene().buildIndex;
        nextLevel = currentLevel + 1;

        scoreText.text = "Score: " + score;
        goldCoinCounter.text = "0";

        ammoText.text = "Ammo: x7";
        greenArrowImage.SetActive(true);
        redArrowImage.SetActive(false);

        if (currentLevel != 3) {
            keyText.text = "Key: ?";
            keyImage.SetActive(false);
        } else {
            bossHealthText.text = "Golem Health: " + boss.GetHealth();
        }
    }

    private void Update() {
        scoreText.text = "Score: " + score;
        goldCoinCounter.text = "" + goldCoinsCollected;
        ammoText.text = "Ammo: x" + archer.getAmmo();

        if (currentLevel == 3) {
            bossHealthText.text = "Golem Health: " + boss.GetHealth();
        }
    }

    /* This method increases the players score by a given amount. */
    public void IncreaseScore(int amount) {
        score += amount;
    }

    /* This method increments the gold coin counter by 1 every time a coin is collected.
     * After collecting 5 coins the player is given 5 upgraded bullets. The amount of ammo the player had before receiving the upgraded bullets is stored
     * in a temporary variable and given back to the player after the upgraded bullets have been used.
     */
    public void GoldCoinCounter() {
        goldCoinsCollected++;

        if (goldCoinsCollected % 5 == 0) {
            audioManager.Play("UpgradedArrow");
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

    /* This method sets the key image in the game hud to active. */
    public void EquipKey() {
        keyText.text = "Key: ";
        keyImage.SetActive(true);
    }

    /* This method removes a life from the player and changes the lifeImage sprite depending on how many lives are left. */
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

    /* This method increases the players lives by 1 and changes the lifeImage sprite depending on how many they have now */
    public void IncreaseLives() {
        lives++;
        switch (lives) {
            case 2:
                lifeImage.sprite = twoLives;
                break;
            case 3:
                lifeImage.sprite = threeLives;
                break;
        }
    }

    /* This method is called to reset the arrow image in the game hud to green when the player has used all the upgraded bullets. */
    public void resetArrowImage() {
        greenArrowImage.SetActive(true);
        redArrowImage.SetActive(false);
    }

    /* This method sets the players ammo back to the amount they had before getting the upgraded bullets. */
    public void resetAmmo() {
        archer.setAmmo(tempAmmo);
    }

    /* This method is used to end the game when the player has run of of lives. */
    private void EndGame() {
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + score;
        //Time.timeScale = 0;
        archer.KillArcher();
    }

    /* This method is called when the player has defeated the boss and touched the diamond */
    public void WinGame() {
        //Time.timeScale = 0;
        levelChanger.FadeToLevel(nextLevel);
    }

    public int GetLives() {
        return lives;
    }
}
