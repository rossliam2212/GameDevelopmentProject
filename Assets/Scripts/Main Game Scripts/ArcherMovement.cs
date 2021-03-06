using UnityEngine;

public class ArcherMovement : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Variables:")]
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private int ammo = 7;
    private float shootingDelay = 0.5f;
    private float horizontalMove = 0f;

    private int upgradedBulletsShotCounter = 0;

    [Header("Boolean Variables:")]
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private bool isDead = false;
    [Space]
    [SerializeField] private bool hasKey = false;
    [SerializeField] private bool upgradedBullet = false;


    [Header("Other Objects/Components:")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject archerBullet;
    [SerializeField] private GameObject archerUpgradedBullet;

    private GameUIManager gameUIManager;
    private AudioManager audioManager;

    // Animation States
    private string currentState;
    private const string ARCHER_ATTACK = "archer_attack";
    private const string ARCHER_IDLE = "archer_idle";
    private const string ARCHER_DEATH = "archer_death";


    private void Start() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
        audioManager = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
    }

    private void Update() {
        if (!isDead) {
            horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump")) {
                isJumping = true;
            }

            if (Input.GetButtonDown("Fire1")) {
                if (isShooting)
                    return;

                isShooting = true;
                ChangeAnimationState(ARCHER_ATTACK); // Change to the shooting animation
                audioManager.Play("ArrowShot");
                Invoke("Shoot", shootingDelay); // Call the shoot method with a delay to match up with the animation
                Invoke("ResetShoot", shootingDelay); // Call the shooting reset method
            }

            if (transform.position.z < 0) {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            }
        }
    }

    private void FixedUpdate() {
        Move(horizontalMove * Time.fixedDeltaTime, isJumping);
    }

    private void Move(float move, bool jump) {
        if (move != 0 && isGrounded && GetComponent<AudioSource>().isPlaying == false) {
            GetComponent<AudioSource>().Play();
        }

        // If the Archer is facing right, move right
        if (isFacingRight) 
            transform.Translate(move, 0, 0);
        // Else move left
        else 
            transform.Translate(-move, 0, 0);
        

        // If Input is moving the Archer right but is facing left, then flip 
        if (move > 0 && !isFacingRight) {
            FlipArcher();
        }
        // If Input is moving the Archer left but is facing right, then flip 
        else if (move < 0 && isFacingRight) {
            FlipArcher();
        }

        if (jump && isGrounded) {
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            isGrounded = false;
        }
    }

    /* This method flips the archer 180 degrees to face the opposite direction. */
    private void FlipArcher() {
        isFacingRight = !isFacingRight; 
        transform.Rotate(0f, 180f, 0f);
    }

    /* This method instantiates an instance of the archer bullet. */
    private void Shoot() {
        if (ammo > 0) {       
            if (upgradedBullet) {
                Instantiate(archerUpgradedBullet, firePoint.position, firePoint.rotation);
                upgradedBulletsShotCounter++;

                if (upgradedBulletsShotCounter % 5 == 0) {
                    upgradedBullet = false;
                    upgradedBulletsShotCounter = 0;
                    gameUIManager.resetArrowImage();
                    gameUIManager.resetAmmo();
                }
                ammo--;
            } else {
                Instantiate(archerBullet, firePoint.position, firePoint.rotation);
                ammo--;
            }
        }
    }

    /* This method resets the players shooting. */
    private void ResetShoot() {
        isShooting = false;
        ChangeAnimationState(ARCHER_IDLE);
    }

    /* This method changes the players animation state. */
    private void ChangeAnimationState(string newState) {
        if (currentState == newState)
            return;
        animator.Play(newState);
    }

    /* This method kills the player when they have no lives left. */
    public void KillArcher() {
        isDead = true;
        transform.position = new Vector2(transform.position.x, transform.position.y + -0.1f);
        ChangeAnimationState(ARCHER_DEATH);
    }

    /* Archer Collision Detection. */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "GoldCoin") {
            audioManager.Play("CoinPickup");
            gameUIManager.GoldCoinCounter();
            gameUIManager.IncreaseScore(gameUIManager.goldCoinPoints);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Heart") {
            if (gameUIManager.GetLives() < 3) {
                gameUIManager.IncreaseLives();
                Destroy(collision.gameObject);
            } 
        }

        if (collision.gameObject.tag == "Key") {
            audioManager.Play("KeyPickup");
            gameUIManager.EquipKey();
            hasKey = true;
            Destroy(collision.gameObject);
        }

        // Colliding with Floor & Platform Layers
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9) {
            isJumping = false;
            isGrounded = true;
        }

        if (collision.gameObject.tag == "BossBullet") {
            gameUIManager.RemoveLife();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Diamond") {
            gameUIManager.WinGame();
        }
    }

    /* Archer Getter and Setter Methods */
    public bool getIsJumping() {
        return isJumping;
    }

    public void setUpgradedBullet(bool state) {
        upgradedBullet = state;
    }

    public bool getUpgradedBullet() {
        return upgradedBullet;
    }

    public int getAmmo() {
        return ammo;
    }

    public void addAmmo(int amount) {
        ammo += amount;
    }
    public void setAmmo(int amount) {
        ammo = amount;
    }
    public bool getHasKey() {
        return hasKey;
    }

    public bool GetIsDead() {
        return isDead;
    }
}
