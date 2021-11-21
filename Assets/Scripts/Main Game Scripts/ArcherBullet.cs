using UnityEngine;

public class ArcherBullet : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] new Rigidbody2D rigidbody;

    [Header("Variables:")]
    [SerializeField] private float bulletSpeed = 20f;

    private void Start() {
        Physics2D.IgnoreLayerCollision(10, 11);
        rigidbody.velocity = transform.right * bulletSpeed;
     }
}
