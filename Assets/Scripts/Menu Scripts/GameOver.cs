using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    /* This method loads level 1 from the game over screen. */
    public void PlayAgain() {
        SceneManager.LoadScene("SampleScene");
    }

    /* This method loads the start menu from the game over screen. */
    public void MainMenu() {
        SceneManager.LoadScene("Start Menu");
    }
}
