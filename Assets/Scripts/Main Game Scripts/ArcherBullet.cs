using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBullet : MonoBehaviour {

    [SerializeField] private float bulletSpeed = 20f;
    public int bulletDamage = 20;
    [SerializeField] new Rigidbody2D rigidbody;

    private void Start() {
        rigidbody.velocity = transform.right * bulletSpeed;
     }
}
