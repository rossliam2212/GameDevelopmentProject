using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour {

    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float bulletSpeed = 10f;

    private void Start() {
        Physics2D.IgnoreLayerCollision(10, 12); // Ignore Collision with the Gold Coins
        Physics2D.IgnoreLayerCollision(7, 12); // Ignore Collision with the Enemies
        Physics2D.IgnoreLayerCollision(11, 12); // Ignore Collision with the Archers bullets
        Physics2D.IgnoreLayerCollision(12, 14); // Ignore Collision with the Heart

        rigidbody.velocity = transform.right * bulletSpeed;
    }

    /* Boss Bullet Collision Detection */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "GoldCoin" || collision.gameObject.layer == 8 || collision.gameObject.tag == "MapEdge") {
            Destroy(gameObject);
        }
    }
}
