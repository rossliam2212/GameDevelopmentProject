using UnityEngine;

public class ArcherBullet : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] new Rigidbody2D rigidbody;

    [Header("Variables:")]
    [SerializeField] private float bulletSpeed = 20f;

    private void Start() {
        Physics2D.IgnoreLayerCollision(10, 11); // Ignoring collision with gold coins
        Physics2D.IgnoreLayerCollision(10, 14); // Ignoring collision with hearts
        rigidbody.velocity = transform.right * bulletSpeed;
     }

    /* Archer Bullet Collision Detection */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "MapEdge") {
            Destroy(gameObject);
        }
    }
}
