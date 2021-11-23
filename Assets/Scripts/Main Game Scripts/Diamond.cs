using UnityEngine;

public class Diamond : MonoBehaviour {

    private AudioManager audioManager;

    private void Start() {
        audioManager = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // When the diamond hits the ground layer, play the diamond thump sound
        if (collision.gameObject.layer == 8) {
            audioManager.Play("DiamondThump");
        }
    }
}
