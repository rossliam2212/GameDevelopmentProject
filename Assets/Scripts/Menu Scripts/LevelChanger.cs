using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    [SerializeField] private Animator animator;
    private int levelToLoad;


    private void Start() {
        animator = GetComponent<Animator>();
    }
       
    public void FadeToLevel(int levelIndex) {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        SceneManager.LoadScene(levelToLoad);
    }
}
