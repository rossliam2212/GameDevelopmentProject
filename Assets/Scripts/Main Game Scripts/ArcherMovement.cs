using UnityEngine;

public class ArcherMovement : MonoBehaviour {

    [Header("Components:")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Variables:")]
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 400f;
    private float shootingDelay = 0.5f;
    private float horizontalMove = 0f;

    [Header("Boolean Variables:")]
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isShooting;
    //[SerializeField] private bool isGrounded = true;

    [Header("Other Objects/Components:")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject archerBullet;
    private Animator mageAnimator;
    private GameUIManager gameUIManager;
    private Mage mage;

    // Animation States
    private string currentState;
    const string ARCHER_ATTACK = "archer_attack";
    const string ARCHER_IDLE = "archer_idle";


    private void Start() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        mageAnimator = GameObject.FindGameObjectWithTag("Mage").GetComponent<Animator>();
        gameUIManager = GameObject.FindObjectOfType(typeof(GameUIManager)) as GameUIManager;
        mage = GameObject.FindObjectOfType(typeof(Mage)) as Mage;
    }

    private void Update() {
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
            Invoke("Shoot", shootingDelay); // Call the shoot method with a delay to match up with the animation
            Invoke("ResetShoot", shootingDelay); // Call the shooting reset method
        }
    }

    private void FixedUpdate() {
        Move(horizontalMove * Time.fixedDeltaTime, isJumping);
        isJumping = false;
    }

    private void Move(float move, bool jump) {

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

        if (jump) {
            rigidBody.AddForce(new Vector2(0f, jumpForce));
        }
    }

    /* This method flips the archer 180 degrees to face the opposite direction. */
    private void FlipArcher() {
        isFacingRight = !isFacingRight; 
        transform.Rotate(0f, 180f, 0f); 
    }

    /* This method instantiates an instance of the archer bullet. */
    private void Shoot() {
        Instantiate(archerBullet, firePoint.position, firePoint.rotation);
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

    private void OnCollisionEnter2D(Collision2D collision) {
        //if (collision.gameObject.tag == "MageAttackPoint" && mageAnimator.GetCurrentAnimatorStateInfo(0).IsName("mage_attack")) {
        //    //gameUIManager.RemoveLife();
        //    Debug.Log("gameUIManager.RemoveLife();");
        //}

        if (collision.gameObject.tag == "GoldCoin") {
            gameUIManager.GoldCoinCounter();
            gameUIManager.IncreaseScore(gameUIManager.goldCoinPoints);
            Destroy(collision.gameObject);
        }
    }
}
