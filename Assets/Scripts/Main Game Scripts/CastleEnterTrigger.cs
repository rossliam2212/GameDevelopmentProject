using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleEnterTrigger : MonoBehaviour {

    private ArcherMovement archer;
    private LevelChanger levelChanger;
    private int currentLevel;
    private int nextLevel;

    private void Start() {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        nextLevel = currentLevel + 1;

        archer = GameObject.FindObjectOfType(typeof(ArcherMovement)) as ArcherMovement;
        levelChanger = GameObject.FindObjectOfType(typeof(LevelChanger)) as LevelChanger;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Archer") {
            if (archer.getHasKey()) {
                Debug.Log("Castle Enter Triggered");
                levelChanger.FadeToLevel(nextLevel);
            } else {
                Debug.Log("Key not Found!");
            }
        }
    }
}
