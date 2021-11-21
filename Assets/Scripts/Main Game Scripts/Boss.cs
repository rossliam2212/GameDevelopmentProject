using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bossBullet;


    // Animation States
    private string currentState;
    private const string BOSS_IDLE = "boss_idle";
    private const string BOSS_DEFENCE = "boss_defence";
    private const string BOSS_SHOOT = "boss_shoot";

    [Header("Variables:")]
    [SerializeField] private int bossHealth = 100;
    [SerializeField] private float moveSpeed = .5f;
    [SerializeField] private float shootingDelay = 1f;
    [SerializeField] private float regenerationTime = 1f;
    [Space]
    [SerializeField] private bool isMovingUp = true;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isDefence = false;

    private int archerBulletDamage = 5;
    private int archerUpgradedBulletDamage = 10;
    private int bossDeathScore = 100;

    private float topPoint;
    private float bottomPoint;

    [Header("Other Objects/Components:")]
    [SerializeField] private Transform archerTransform;
    [SerializeField] private GameObject diamond;
    private GameUIManager gameUIManager;
    private AudioManager audioManager;

    private void Start() {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
        audioManager = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;

        spriteRenderer.flipX = true;          

        // Setting up the two points on the x-axis that the executioner moves between, if it is not following the player.
        topPoint = transform.position.y + 7;
        bottomPoint = transform.position.y - 7;

        audioManager.Play("GolemGrowl");
    }

    private void Update() {
        if (!isDefence) {
            MoveBoss();

            if (isAttacking) {
                return;
            } else {
                isAttacking = true;
                Invoke("Shoot", shootingDelay);
                Invoke("ResetShooting", shootingDelay);      
            }
        }

    }

    private void MoveBoss() {
        //ChangeAnimationState(BOSS_IDLE);
        ChangeAnimationState(BOSS_SHOOT);

        if (isMovingUp) {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            isMovingUp = true;
        } else {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            isMovingUp = false;
        }

        if (transform.position.y > topPoint) {
            isMovingUp = false;
            //spriteRenderer.flipX = true;
        }

        if (transform.position.y < bottomPoint) {
            isMovingUp = true;
            //spriteRenderer.flipX = false;
        }

        //if (RandomNum() >= 990) {
        //    RegenerateBoss();
        //}
    }

    private void RegenerateBoss() {
        ChangeAnimationState(BOSS_DEFENCE);
        isDefence = true;
        bossHealth = 100;
        Invoke("ResetDefence", 2f);
    }

    private void ResetDefence() {
        isDefence = false;
    }

    private void Shoot() {
        Instantiate(bossBullet, firePoint.position, firePoint.rotation);
    }

    private void ResetShooting() {
        isAttacking = false;
    }

    private int RandomNum() {
        return Random.Range(1, 1001);
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState)
            return;
        animator.Play(newState);
    }

    private void TakeDamage(int amount) {
        bossHealth -= amount;
        if (bossHealth <= 0) {
            KillBoss();
        }
    }

    private void KillBoss() {
        gameUIManager.IncreaseScore(bossDeathScore);
        audioManager.Play("Diamond");
        Instantiate(diamond, transform.position, transform.rotation);
        Destroy(gameObject);
        audioManager.Stop("GolemGrowl");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "ArcherBullet") {
            TakeDamage(archerBulletDamage);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "ArcherUpgradedBullet") {
            TakeDamage(archerUpgradedBulletDamage);
            Destroy(collision.gameObject);
        }
    }
}
