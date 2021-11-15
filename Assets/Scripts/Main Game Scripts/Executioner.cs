using UnityEngine;

/* CURRENTLY NOT BEING USED */
public class Executioner : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform leftAttackPoint;
    [SerializeField] private Transform rightAttackPoint;
    [SerializeField] private LayerMask playerLayer;


    // Animation States
    private string currentState;
    private const string EXECUTIONER_IDLE = "executioner_idle";
    private const string EXECUTIONER_ATTACK = "executioner_attack";

    [Header("Variables:")]
    [SerializeField] private int executionerHealth = 100;
    [SerializeField] private float moveSpeed = .5f;
    [Space]
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isAttacking = false;

    private int archerBulletDamage = 10;
    private int executionerDeathScore = 20;

    [SerializeField] private float executionerAttackRange = .5f;
    private float executionerFollowDistance = 3f;
    private float executionerYAttackDistance = 0.5f;
    private float attackTimer = 0f;

    private float rightPoint;
    private float leftPoint;

    [Header("Other Objects/Components:")]
    [SerializeField] private Transform archerTransform;
    private GameUIManager gameUIManager;


    private void Start() {
        animator = GetComponent<Animator>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;

        // Setting up the two points on the x-axis that the mage moves between, if it is not following the player.
        rightPoint = transform.position.x + 3;
        leftPoint = transform.position.x - 3;
    }

    private void Update() {
        MoveExecutioner();
    }

    private void MoveExecutioner() {
        ChangeAnimationState(EXECUTIONER_IDLE);
        if (isFacingRight) {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            isFacingRight = true;
        } else {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
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

    private void Attack() {
        Collider2D[] hitplayer;

        if (isFacingRight) {
            hitplayer = Physics2D.OverlapCircleAll(rightAttackPoint.position, executionerAttackRange, playerLayer);
        } else {
            hitplayer = Physics2D.OverlapCircleAll(leftAttackPoint.position, executionerAttackRange, playerLayer);
        }

        foreach (Collider2D player in hitplayer) {
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

    private void ResetAttacking() {
        isAttacking = false;
        attackTimer = 0f;
    }

    private void OnDrawGizmosSelected() {
        if (isFacingRight) {
            Gizmos.DrawWireSphere(rightAttackPoint.position, executionerAttackRange);
        } else {
            Gizmos.DrawWireSphere(leftAttackPoint.position, executionerAttackRange);
        }
    }

    /* This method is used to change the mages animation state. */
    private void ChangeAnimationState(string newState) {
        if (currentState == newState)
            return;
        animator.Play(newState);
    }

    /* This method is sed to check whether or not the executioner is within following distance of the player. */
    private bool checkFollowDistance(float archerPosition, float executionerPosition) {
        if (Mathf.Abs(archerPosition - executionerPosition) < executionerFollowDistance) {
            return true;
        }
        return false;
    }

    /* This method is used to check whether or not the executioner is within attacking distance of the player. */
    private bool checkAttackDistance(float archerPosition, float executionerPosition) {
        if (Mathf.Abs(archerPosition - executionerPosition) < executionerFollowDistance) {
            return true;
        }
        return false;
    }

    /* This method is used to check whether or not the executioner is within a certain distance on the y axis */
    private bool inYRange() {
        if ((archerTransform.position.y - transform.position.y) < executionerYAttackDistance)
            return true;

        return false;
    }

    /* This method is used to take damage away from the executioner. */
    private void TakeDamage() {
        executionerHealth -= archerBulletDamage;
        if (executionerHealth <= 0) {
            KillExecutioner();
        }
    }

    /* This method is used to destroy the instance of the executioner. */
    private void KillExecutioner() {
        gameUIManager.IncreaseScore(executionerDeathScore);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "ArcherBullet") {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }
}