using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [Header("Variables: ")]
    [SerializeField] private float moveSpeed = .3f;
    [SerializeField] private bool movingUp;
    private float topPoint;
    private float bottomPoint;



    private void Start() {
        // Setting up the two points on the y axis that the platform will move between.
        topPoint = transform.position.y + 2f;
        bottomPoint = transform.position.y - 2f;

        movingUp = RandomNumber() > 0.5; // Deciding whether the platform starts off moving up or down.
    }

    private void Update() {
        MovePlatform();
    }

    /* This method moves the platform between two points on the y axis. */
    private void MovePlatform() {
        if (movingUp) {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        } else {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }

        if (transform.position.y > topPoint) {
            movingUp = false;
        } 
        if (transform.position.y < bottomPoint) {
            movingUp = true;
        }
    }

    private float RandomNumber() {
        return Random.Range(0f, 1f);
    }

    public bool isMovingUp() {
        return movingUp;
    }
}
