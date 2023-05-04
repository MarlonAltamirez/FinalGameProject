using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform ceilingCheck;
    public Transform groundCheck;
    public LayerMask groundObjects;
    public float checkRadius;
    public int maxJumpCount;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDirection;
    private bool isJumping = false;
    private bool isGrounded;
    private int jumpCount;

    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        jumpCount = maxJumpCount;
    }

    void Update()
    { 
        ProcessInput();

        Animation();
 
        animator.SetFloat("Horizontal", Mathf.Abs(moveDirection));
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
        if(isGrounded)
        {
            jumpCount = maxJumpCount;
            OnLanding();
        }


        Movement();
    }

    private void FLipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void ProcessInput()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            isJumping = true;
            
        }
    }

    private void OnLanding ()
    {
        animator.SetBool("isJumping", false);
    }

    private void Animation()
    {
        if(moveDirection > 0 && !facingRight)
        {
            FLipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FLipCharacter();
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        if(isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jumpCount--;
            animator.SetBool("isJumping", true);
        }
        isJumping = false;
    }
   
}
