using UnityEngine;

public class Mage : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform leftAttackPoint;
    [SerializeField] private Transform rightAttackPoint;
    [SerializeField] private LayerMask playerLayer;

    private Vector2 startCollider;
    private Vector2 newCollider;

    // Animation States
    private string currentState;
    private const string MAGE_IDLE = "mage_idle";
    private const string MAGE_ATTACK = "mage_attack";
    private const string MAGE_RUNNING = "mage_running";
    private const string MAGE_REGEN = "mage_regen";

    [Header("Variables:")]
    [SerializeField] private int mageHealth = 100;
    [SerializeField] private float walkSpeed = .5f;
    [SerializeField] private float runSpeed = 5f;
    [Space]
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isAttacking = false;
    //[SerializeField] private bool isRegenerating = false;
    [SerializeField] private bool holdingKey = false;

    private int archerBulletDamage = 20;
    private int archerUpgradedBulletDamage = 40;
    private int mageDeathScore = 10;

    [SerializeField] private float mageAttackRange = 0.5f;
    private float mageAttackDistance = 0.3f;
    private float mageFollowDistance = 3f;
    private float mageYAttackDistance = 0.5f;
    private float attackTimer = 0f;

    private float rightPoint;
    private float leftPoint;

    [Header("Other Objects/Components:")]
    [SerializeField] private Transform archerTransform;
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private GameObject key;
    private GameUIManager gameUIManager;
    private AudioManager audioManager;
    private ArcherMovement archer;


    private void Start() {
        animator = GetComponent<Animator>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
        audioManager = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
        archer = GameObject.FindObjectOfType(typeof(ArcherMovement)) as ArcherMovement;

        // Setting up the two points on the x-axis that the mage moves between, if it is not following the player.
        rightPoint = transform.position.x + 2;
        leftPoint = transform.position.x - 2;

        Physics2D.IgnoreLayerCollision(7, 10); // Ignore collision with gold coins
        Physics2D.IgnoreLayerCollision(7, 14); // Ignore Collision with hearts
    }

    private void Update() {
        if (archerTransform != null) {
            if (checkFollowDistance(archerTransform.position.x, transform.position.x) && !archer.GetIsDead()) {
                FollowArcher();
            } 
            else {
                MoveMage();
            }
        }
    }

    /* This method is called when the mage is not within following distance of the player. */
    /* The mage is set to move between two points on the x-axis. */
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

        //if (mageHealth > 100) {
        //    isRegenerating = true;
        //}
    }

    /* This method is called when the mage is within following distance of the player. */
    /* While in following distance, the checkAttackDistance() method is called to check if the player is in attacking range, 
     * if the player is, the mage is set to attack the player. */
    private void FollowArcher() {

        if (archerTransform == null)
            return;

        if (archerTransform.position.x > transform.position.x) {

            if (checkAttackDistance(archerTransform.position.x, transform.position.x) && inYRange()) {
                ChangeAnimationState(MAGE_ATTACK);
                audioManager.Play("SwordSwing");
                if (archer.GetIsDead()) {
                    MoveMage();
                } else {
                    CheckAttack();
                }
            }
            else {
                if (!inYRange()) {
                    MoveMage();
                } else {
                    isFacingRight = true;
                    ChangeAnimationState(MAGE_RUNNING);
                    transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
                    spriteRenderer.flipX = false;
                }
            }
        } 
        else if (archerTransform.position.x < transform.position.x) {

            if (checkAttackDistance(archerTransform.position.x, transform.position.x) && inYRange()) {
                ChangeAnimationState(MAGE_ATTACK);
                audioManager.Play("SwordSwing");
                if (archer.GetIsDead()) {
                    MoveMage();
                } else {
                    CheckAttack();
                }
            }
            else {
                if (!inYRange()) {
                    MoveMage();
                } else {
                    isFacingRight = false;
                    ChangeAnimationState(MAGE_RUNNING);
                    transform.Translate(Vector2.left * runSpeed * Time.deltaTime);
                    spriteRenderer.flipX = true;
                }
            }
        }
    }

    //private void RegenerateMage() {
    //    ChangeAnimationState(MAGE_REGEN);
    //    mageHealth += 5;
    //    isRegenerating = false;
    //}

    private void Attack() {
        Collider2D[] hitplayer;

        if (isFacingRight) {
            hitplayer = Physics2D.OverlapCircleAll(rightAttackPoint.position, mageAttackRange, playerLayer);
        } else {
            hitplayer = Physics2D.OverlapCircleAll(leftAttackPoint.position, mageAttackRange, playerLayer);
        }

        foreach(Collider2D player in hitplayer) {
            gameUIManager.RemoveLife();
        }

        ResetAttacking();
    }

    private void CheckAttack() {
        if (attackTimer <= 0f) {
            attackTimer = 1f;
        }

        if (attackTimer > 0f) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f) {
                isAttacking = true;
                Attack();
            }
        }
    }

    /* This method is used to reset the attack timer for the mage */
    private void ResetAttacking() {
        isAttacking = false;
        attackTimer = 0f;
    }

    /* This method draws the circle radius of the mages attack distance to make it easier to edit */
    private void OnDrawGizmosSelected() {
        if (isFacingRight) {
            Gizmos.DrawWireSphere(rightAttackPoint.position, mageAttackRange);
        } else {
            Gizmos.DrawWireSphere(leftAttackPoint.position, mageAttackRange);
        }
    }

    private void Regenerate() {
        ChangeAnimationState(MAGE_REGEN);
        mageHealth = 100;
    }

    /* This method is used to change the mages animation state. */
    private void ChangeAnimationState(string newState) {
        if (currentState == newState)
            return;
        animator.Play(newState);
    }

    /* This method is sed to check whether or not the mage is within following distance of the player. */
    private bool checkFollowDistance(float archerPosition, float magePosition) {
        if (Mathf.Abs(archerPosition - magePosition) < mageFollowDistance) {
            return true;
        }
        return false;
    }

    /* This method is used to check whether or not the mage is within attacking distance of the player. */
    private bool checkAttackDistance(float archerPosition, float magePosition) {
        if (Mathf.Abs(archerPosition - magePosition) < mageAttackDistance) {
            return true;
        }
        return false;
    }

    /* This method is used to check whether or not the mage is within a certain distance on the y axis */
    private bool inYRange() {
        if (Mathf.Abs(archerTransform.position.y - transform.position.y) < mageYAttackDistance)
            return true;

        return false;
    }

    /* This method is used to take damage away from the mage. */
    private void TakeDamage(int amount) {
        mageHealth -= amount;
        if (mageHealth <= 0) {
            KillMage();
        }
    }

    /* This method is used to destroy the instance of the mage. 
     * If the mage is set to be holding the key, an instance of the key will be spawned when the mage dies.
     * Otherwise a coin will be spawned. 
    */
    private void KillMage() {
        gameUIManager.IncreaseScore(mageDeathScore);
        if (holdingKey) {
            Instantiate(key, transform.position, transform.rotation);
        } else {
            Instantiate(goldCoin, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    /* Mage Collision Detection */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "ArcherBullet") {
            audioManager.Play("EnemyHit");
            TakeDamage(archerBulletDamage);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "ArcherUpgradedBullet") {
            audioManager.Play("EnemyHit");
            TakeDamage(archerUpgradedBulletDamage);
            Destroy(collision.gameObject);
        }
    }
}
