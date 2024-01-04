using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    bool playerHasHorizontalSpeed;
    [SerializeField] float moveSpeed = 5f;
    Animator anim;
    Vector2 moveInput;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
