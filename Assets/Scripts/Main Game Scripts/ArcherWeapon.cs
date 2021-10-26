using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherWeapon : MonoBehaviour {

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject archerBullet;

    [SerializeField] private Animator animator;
    const string ARCHER_ATTACK = "archer_attack";
    const string ARCHER_IDLE = "archer_idle";
    private string currentState;

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            ChangeAnimationState(ARCHER_ATTACK);
            Shoot();
        }
    }

    private void Shoot() {
        Instantiate(archerBullet, firePoint.position, firePoint.rotation);
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState)
            return;
        animator.Play(newState);
    }
}
