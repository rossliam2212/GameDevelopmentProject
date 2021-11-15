using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private float moveSpeed = .3f;
    private Platform platform;

    private void Start() {
        platform = GameObject.FindObjectOfType(typeof(Platform)) as Platform;
    }

    private void Update() {
        MoveCoin();
    }

    private void MoveCoin() {
        if (platform.isMovingUp()) {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        } else {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
    }
}
