using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp = 3;
    public float moveSpeed;
    public float attackSpeed = 3f;
    private float currentAttackTime = 0;
    private bool isMoving = false;
    private bool isAttacking = false;

    private Rigidbody2D rb;
    private Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    { 
        if (hp <= 0)
        {
            Die();
        }

        if (isAttacking) Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!isMoving) return;

        Vector3 targetPos = target.position;
        Vector2 targetPosition = new Vector2(targetPos.x, transform.position.y);

        if (transform.position.x != targetPos.x)
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
            isAttacking = true;

            // animation
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
        isMoving = true;
    }

    private void Attack()
    {
        currentAttackTime += Time.deltaTime;
        if (currentAttackTime >= attackSpeed)
        {
            // lose

            if (GameManager.GameLose != null)
            {
                GameManager.GameLose.Invoke();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        hp--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Orang")
        {
            Orang orang = collision.gameObject.GetComponent<Orang>();
            orang.Attacked();
        }
    }
}
