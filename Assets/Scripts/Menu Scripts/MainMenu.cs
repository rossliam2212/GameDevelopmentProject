using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    /* This method loads level 1 from the start menu. */
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /* This method quits the game from the start menu. */
    public void QuitGame() {
        Application.Quit();
    }
}
