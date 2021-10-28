using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] private int health = 100;
    private float deathDelay = 1.5f;

    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private float moveSpeed = 6f;

    private int archerBulletDamage = 20;
    private int executionerDeathScore = 10;
    private float followDistance = 10f;

    [SerializeField] private Transform archerTransform;
    private GameUIManager gameUIManager;

    // Animation States
    const string EXECUTIONER_DEATH = "executioner_death";
    const string EXECUTIONER_ATTACK = "executioner_attack";
    const string EXECUTIONER_IDLE = "executioner_idle";

    private void Start() {
        animator = GetComponent<Animator>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
        archerTransform = FindObjectOfType<ArcherMovement>().GetComponent<Transform>();
    }

    private void Update() {
        if (checkFollowDistance(archerTransform.position.x, transform.position.x)) {
            if (archerTransform.position.x < transform.position.x) {
                transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
            }
            else if (archerTransform.position.x > transform.position.x) {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
            }
        }
    }

    private bool checkFollowDistance(float archerPosition, float executionerPosition) {
        if (Mathf.Abs(archerPosition - executionerPosition) < followDistance) {
            return true;
        }
        return false;
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