using UnityEngine;
using System.Collections;

public class RandomNPCMovement : MonoBehaviour
{
    public float moveRadius = 5f;
    public float moveSpeed = 2f;
    private Vector2 targetPosition;

    private bool facingRight = false;
    void Start()
    {
        StartCoroutine(ChangeTargetPosition());
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if ((targetPosition.x > transform.position.x && facingRight) || (targetPosition.x < transform.position.x && !facingRight))
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

    IEnumerator ChangeTargetPosition()
    {
        while (true)
        {
            targetPosition = new Vector2(
                transform.position.x + Random.Range(-moveRadius, moveRadius),
                transform.position.y + Random.Range(-moveRadius, moveRadius)
            );
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }
}
