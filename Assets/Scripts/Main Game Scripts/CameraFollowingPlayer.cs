using UnityEngine;

public class CameraFollowingPlayer : MonoBehaviour {

    private Transform playerTransform;

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Archer").transform;
    }


    private void LateUpdate() {

        // Storing camera's current position
        Vector3 temp = transform.position;

        // Setting the camera's x position equal to the player's x position
        temp.x = playerTransform.position.x;

        // Setting the camer's temporary position back to the current position
        transform.position = temp;
    }
}
