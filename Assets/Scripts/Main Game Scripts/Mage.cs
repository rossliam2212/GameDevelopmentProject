using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour {

    [SerializeField] private Animator animator;
    private string currentState;
    [SerializeField] private SpriteRenderer spriteRenderer;
    const string MAGE_IDLE = "mage_idle";
    const string MAGE_ATTACK = "mage_attack";
    const string MAGE_RUNNING = "mage_running";

    [SerializeField] private int mageHealth = 100;
    [SerializeField] private float walkSpeed = .5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private bool isFacingRight = true;

    private int archerBulletDamage = 20;
    private int mageDeathScore = 10;
    private float mageFollowDistance = 3f;
    private float mageAttackDistance = 0.3f;

    [SerializeField] private Transform archerTransform;
    private GameUIManager gameUIManager;

    private float rightPoint;
    private float leftPoint;

    private void Start() {
        animator = GetComponent<Animator>();
        rightPoint = transform.position.x + 3;
        leftPoint = transform.position.x - 3;
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
    }

    private void Update() {
        if (checkFollowDistance(archerTransform.position.x, transform.position.x)) {
            FollowArcher();
        } 
        else {
            MoveMage();
        }
    }

    private void MoveMage() {
        ChangeAnimationState(MAGE_IDLE);
        if (isFacingRight) {
            transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);
            isFacingRight = true;
        }
        else {
            transform.Translate(Vector2.left * walkSpeed * Time.deltaTime);
            isFacingRight = false;
        }

        if (transform.position.x > rightPoint) {
            isFacingRight = false;
            spriteRenderer.flipX = true;
        }

        if (transform.position.x < leftPoint) {
            isFacingRight = true;
            spriteRenderer.flipX = false;
        }
    }

    private void FollowArcher() {

        if (archerTransform.position.x > transform.position.x) {

            if (checkAttackDistance(archerTransform.position.x, transform.position.x)) {
                ChangeAnimationState(MAGE_ATTACK);
            }
            else {
                ChangeAnimationState(MAGE_RUNNING);
                transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
                spriteRenderer.flipX = false;
            }
        } 
        else if (archerTransform.position.x < transform.position.x) {

            if (checkAttackDistance(archerTransform.position.x, transform.position.x)) {
                ChangeAnimationState(MAGE_ATTACK);
            }
            else {
                ChangeAnimationState(MAGE_RUNNING);
                transform.Translate(Vector2.left * runSpeed * Time.deltaTime);
                spriteRenderer.flipX = true;
            }
        }
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState)
            return;
        animator.Play(newState);
    }

    private bool checkFollowDistance(float archerPosition, float magePosition) {
        if (Mathf.Abs(archerPosition - magePosition) < mageFollowDistance) {
            return true;
        }
        return false;
    }

    private bool checkAttackDistance(float archerPosition, float magePosition) {
        if (Mathf.Abs(archerPosition - magePosition) < mageAttackDistance) {
            return true;
        }
        return false;
    }

    private void TakeDamage() {
        mageHealth -= archerBulletDamage;
        if (mageHealth <= 0) {
            KillMage();
        }
    }

    private void KillMage() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Archer Bullet(Clone)") {
            TakeDamage();
            gameUIManager.IncreaseScore(mageDeathScore);
            Destroy(collision.gameObject);
        }
    }
}
