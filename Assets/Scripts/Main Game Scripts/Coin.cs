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

        // If the current scene is level 1, then the coin will be set to move, otherwise it will stay still.
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

    /* This method moves the coin in the same direction as the platform in level 1. */
    private void MoveCoin() {
        if (platform.isMovingUp()) {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        } else {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
    }
}
