using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour {

    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float bulletSpeed = 10f;

    private void Start() {
        Physics2D.IgnoreLayerCollision(10, 12);
        Physics2D.IgnoreLayerCollision(7, 12);
        rigidbody.velocity = transform.right * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "GoldCoin" || collision.gameObject.layer == 8 || collision.gameObject.tag == "MapEdge") {
            Destroy(gameObject);
        }
    }
}
