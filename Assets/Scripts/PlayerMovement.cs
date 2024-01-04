using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float moveSpeed = 5f;
    bool playerHasHorizontalSpeed;

    CapsuleCollider2D capsuleCollider;
    Animator anim;
    Vector2 moveInput;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
        FlipSprite();
    }

    private void FlipSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        anim.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void OnJump(InputValue value)
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpSpeed);
        }
        
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
