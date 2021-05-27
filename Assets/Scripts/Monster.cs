using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving = false;

    private Rigidbody2D rb;
    private Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isMoving = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!isMoving) return;

        Vector3 furniturePos = target.position;
        Vector2 targetPosition = new Vector2(furniturePos.x, transform.position.y);

        if (transform.position.x != furniturePos.x)
        {
            Vector2 p = Vector2.MoveTowards(transform.position,
                                            targetPosition,
                                            moveSpeed);

            rb.MovePosition(p);
        }
        else
        {
            // attack
            isMoving = false;
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
