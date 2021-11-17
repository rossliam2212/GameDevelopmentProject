using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingPlayerCastle : MonoBehaviour {

    private ArcherMovement archer;
    private Transform playerTransform;

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Archer").transform;
        archer = GameObject.FindObjectOfType(typeof(ArcherMovement)) as ArcherMovement;
    }


    private void LateUpdate() {

        // Storing camera's current position
        Vector3 temp = transform.position;

        // Setting the camera's x & y position equal to the player's x & y position
        temp.x = playerTransform.position.x;

        if (!archer.getIsJumping()) {
            temp.y = playerTransform.position.y + 2f;
        }

        // Setting the camer's temporary position back to the current position
        transform.position = temp;
    }
}
