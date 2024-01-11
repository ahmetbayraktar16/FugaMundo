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
    [SerializeField] Vector2 deathKick = new Vector2(2f, 10f);
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip fireSFX;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector3 deathRotation = new Vector3(0, 0, 90);
    bool playerHasHorizontalSpeed;
    bool isAlive = true;
    Transform playerTransform;
    CapsuleCollider2D capsuleCollider;
    Animator anim;
    Vector2 moveInput;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerTransform = GetComponent<Transform>();
        gun = GetComponent<Transform>();
    }

    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        Die();
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
    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }

        Instantiate(bullet, gun.position, transform.rotation);
        AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            return;
        }

        if (value.isPressed)
        {
            AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
            rb.velocity += new Vector2(0f, jumpSpeed);
            anim.SetTrigger("Jumping");
        }
    }
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
    }

    void Die()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            isAlive = false;
            anim.SetTrigger("takeOff");
            playerTransform.rotation = Quaternion.Euler(deathRotation);
            rb.velocity = deathKick;
            FindObjectOfType<GameSession>().PlayerProcessDeath();
        }
    }
}
