using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] private int health = 100;

    // Animation States
    const string EXECUTIONER_DEATH = "executioner_death";

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount) {
        health -= damageAmount;

        if (health <= 0) {
            KillExecutioner();
        }
    }

    private void KillExecutioner() {
        animator.Play(EXECUTIONER_DEATH);
        Destroy(gameObject);
    }
}