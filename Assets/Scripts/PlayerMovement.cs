using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private const float speed = 2f;
    public Rigidbody2D rb;
    private bool facingRight = true;
    Vector2 movement;
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // Odbicie w osi X
        transform.localScale = newScale;
    }
    private void FixedUpdate(){
        animator.SetFloat("speed", Mathf.Abs(movement.x)+Mathf.Abs(movement.y));
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }
}
