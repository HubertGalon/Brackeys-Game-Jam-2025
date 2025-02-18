using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private const float baseSpeed = 7f; 
    private const float sprintMultiplier = 1.5f;  
    public Rigidbody2D rb;
    private bool facingRight = true;
    Vector2 movement;

    private float sprintTime = 3f; 
    public float currentSprintTime; 
    private bool isSprinting = false;
    private bool isExhausted = false; 

    void Start()
    {
        currentSprintTime = sprintTime; 
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (currentSprintTime <= 0)
        {
            isExhausted = true;
            isSprinting = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && currentSprintTime > 0 && !isExhausted)
        {
            isSprinting = true;
            currentSprintTime -= Time.deltaTime; 
        }
        else
        {
            isSprinting = false;

            if (currentSprintTime < sprintTime)
            {
                currentSprintTime += Time.deltaTime;
            }

            if (currentSprintTime >= sprintTime)
            {
                isExhausted = false;
            }
        }

        currentSprintTime = Mathf.Clamp(currentSprintTime, 0, sprintTime);

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
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    private void FixedUpdate()
    {
        float currentSpeed = isSprinting ? baseSpeed * sprintMultiplier : baseSpeed;
        
        animator.SetFloat("speed", Mathf.Abs(movement.x) + Mathf.Abs(movement.y));
        rb.MovePosition(rb.position + movement * currentSpeed * Time.deltaTime);
    }
}
