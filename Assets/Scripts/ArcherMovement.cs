using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArcherMovement : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGrounded;

    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;

    private float horizontalMove = 0f;

    //[Header("Events")]
    //[Space]
    //public UnityEvent OnLandEvent;

    private void Start() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump")) {
            isJumping = true;
            //animator.SetBool("isJumping", true);
        }
    }

    private void FixedUpdate() {
        Move(horizontalMove * Time.fixedDeltaTime, isJumping);
        isJumping = false;
    }

    private void Move(float move, bool jump) {

        if (isFacingRight) {
            transform.Translate(move, 0, 0);
        } else {
            transform.Translate(-move, 0, 0);
        }

        //Moving right
        if (move > 0 && !isFacingRight) {
            FlipArcher();
        }
        //Moving Left
        else if (move < 0 && isFacingRight) {
            FlipArcher();
        }

        if (jump) {
            rigidBody.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void FlipArcher() {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void OnLanding() {
        animator.SetBool("isJumping", false);
    }
}
