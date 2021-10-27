using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMovement : MonoBehaviour {

    [SerializeField] private Animator animator;
    private string currentState;
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isJumping;
    //[SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isShooting;

    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 400f;

    private float horizontalMove = 0f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject archerBullet;
    private float shootingDelay = 0.5f;

    // Animation States
    const string ARCHER_ATTACK = "archer_attack";
    const string ARCHER_IDLE = "archer_idle";


    private void Start() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
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

    private void FlipArcher() {
        isFacingRight = !isFacingRight; // Switching the way the Archer is being labelled as facing
        transform.Rotate(0f, 180f, 0f); // Rotating the Archer 180 deg
    }

    private void Shoot() {
        Instantiate(archerBullet, firePoint.position, firePoint.rotation); // Instantiate an instance of the archer bullet (prefab)
    }

    private void ResetShoot() {
        isShooting = false;
        ChangeAnimationState(ARCHER_IDLE); // Change to the idle animation
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState)
            return;
        animator.Play(newState);
    }
}
