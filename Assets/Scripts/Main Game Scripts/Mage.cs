using UnityEngine;

public class Mage : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    private Vector2 startCollider;
    private Vector2 newCollider;

    // Animation States
    private string currentState;
    const string MAGE_IDLE = "mage_idle";
    const string MAGE_ATTACK = "mage_attack";
    const string MAGE_RUNNING = "mage_running";

    [Header("Variables:")]
    [SerializeField] private int mageHealth = 100;
    [SerializeField] private float walkSpeed = .5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private bool isFacingRight = true;

    private int archerBulletDamage = 20;
    private int mageDeathScore = 10;

    private float mageFollowDistance = 3f;
    private float mageAttackDistance = 0.45f;
    private float mageYAttackDistance = 0.5f;

    private float rightPoint;
    private float leftPoint;

    [Header("Other Objects/Components:")]
    [SerializeField] private Transform archerTransform;
    private GameUIManager gameUIManager;


    private void Start() {
        animator = GetComponent<Animator>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
        boxCollider = GetComponent<BoxCollider2D>();

        // Setting up the two points on the x-axis that the mage moves between, if it is not following the player.
        rightPoint = transform.position.x + 3;
        leftPoint = transform.position.x - 3;

        startCollider = new Vector2(boxCollider.size.x, boxCollider.size.y);
        newCollider = new Vector2(boxCollider.size.x + .3f, boxCollider.size.y);
    }

    private void Update() {
        if (checkFollowDistance(archerTransform.position.x, transform.position.x)) {
            FollowArcher();
        } 
        else {
            MoveMage();
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
    }

    /* This method is called when the mage is within following distance of the player. */
    /* While in following distance, the checkAttackDistance() method is called to check if the player is in attacking range, 
     * if the player is, the mage is set to attack the player. */
    private void FollowArcher() {

        if (archerTransform.position.x > transform.position.x) {

            if (checkAttackDistance(archerTransform.position.x, transform.position.x) && inYRange()) {
                //boxCollider.size = newCollider;
                ChangeAnimationState(MAGE_ATTACK);
            }
            else {
                //boxCollider.size = startCollider;
                if (!inYRange()) {
                    MoveMage();
                } else {
                    ChangeAnimationState(MAGE_RUNNING);
                    transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
                    spriteRenderer.flipX = false;
                }
            }
        } 
        else if (archerTransform.position.x < transform.position.x) {

            if (checkAttackDistance(archerTransform.position.x, transform.position.x) && inYRange()) {
                //boxCollider.size = newCollider;
                ChangeAnimationState(MAGE_ATTACK);
            }
            else {
                //boxCollider.size = startCollider;
                if (!inYRange()) {
                    MoveMage();
                } else {
                    ChangeAnimationState(MAGE_RUNNING);
                    transform.Translate(Vector2.left * runSpeed * Time.deltaTime);
                    spriteRenderer.flipX = true;
                }
            }
        }
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
        if ((archerTransform.position.y - transform.position.y) < mageYAttackDistance)
            return true;

        return false;
    }

    /* This method is used to take damage away from the mage. */
    private void TakeDamage() {
        mageHealth -= archerBulletDamage;
        if (mageHealth <= 0) {
            KillMage();
        }
    }

    /* This method is used to destroy the instance of the mage. */
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
