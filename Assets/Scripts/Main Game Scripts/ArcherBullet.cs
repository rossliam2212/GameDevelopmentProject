using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBullet : MonoBehaviour {

    [SerializeField] private float bulletSpeed = 20f;
    private int bulletDamage = 20;
    [SerializeField] Rigidbody2D rigidbody;

    private void Start() {
        rigidbody.velocity = transform.right * bulletSpeed;
     }

    // REMOVE FROM THIS SCRIPT 
    // ADD COLLISION METHOD TO EACH ENEMY SCRIPT INSTEAD
    //private void OnTriggerEnter2D(Collider2D collision) {
    //    Executioner executioner = collision.GetComponent<Executioner>();

    //    if (executioner != null) {
    //        executioner.TakeDamage(bulletDamage);
    //    }
    //    Destroy(gameObject);
    //}
}
