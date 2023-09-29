using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpAmount = 5f;
    [SerializeField] float climbAmount = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    float startGravity;
    bool isAlive = true;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        startGravity = rb.gravityScale;
    }

    void Update()
    {
        if (!isAlive){ return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            rb.velocity += new Vector2 (0f, jumpAmount);
        }
    }

    void OnFire()
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Run()
    {
        Vector2 playerMovement = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = playerMovement;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            rb.gravityScale = startGravity;
            animator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbAmount);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0;
        bool playerHasVerticalSpeed = Mathf.Abs(moveInput.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard" )))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rb.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
