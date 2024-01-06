using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Animator anim;
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        AnimationStart();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }
    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
    }

    void AnimationStart()
    {
        anim.SetBool("isRunning", true);
    }
}
