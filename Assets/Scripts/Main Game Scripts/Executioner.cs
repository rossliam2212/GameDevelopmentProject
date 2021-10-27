using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] private int health = 100;
    private float deathDelay = 1.5f;
    private int archerBulletDamage = 20;
    private int executionerDeathScore = 10;

    //private ScoreLivesManager scoreLivesManager;
    private GameUIManager gameUIManager;

    // Animation States
    const string EXECUTIONER_DEATH = "executioner_death";

    private void Start() {
        animator = GetComponent<Animator>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
    }

    public void TakeDamage(int damageAmount) {
        health -= damageAmount;

        if (health <= 0) {
            KillExecutioner();
        }
    }

    private void KillExecutioner() {
        animator.Play(EXECUTIONER_DEATH);
        Invoke("DestroyExecutioner", deathDelay);
    }

    private void DestroyExecutioner() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Archer Bullet(Clone)") {
            TakeDamage(archerBulletDamage);
            gameUIManager.IncreaseScore(executionerDeathScore);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "Archer") {
            gameUIManager.RemoveLife();
        }
    }
}