using UnityEngine;

public class ArcherBullet : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] new Rigidbody2D rigidbody;

    [Header("Variables:")]
    [SerializeField] private float bulletSpeed = 20f;

    private void Start() {
        rigidbody.velocity = transform.right * bulletSpeed;
     }
}
