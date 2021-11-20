using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour {

    private float moveSpeed = .3f;
    private Platform platform;
    private bool moveCoin = false;

    private void Start() {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 1) {
            moveCoin = true;
            platform = GameObject.FindObjectOfType(typeof(Platform)) as Platform;
        }
    }

    private void Update() {
        if (moveCoin) {
            MoveCoin();
        }
    }

    private void MoveCoin() {
        if (platform.isMovingUp()) {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        } else {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
    }
}
