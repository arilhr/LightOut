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

    private Furniture furniture;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameOver)
        {
            if (hp <= 0)
            {
                target.GetComponent<Orang>().SetIsMoving(true);
                Die();
            }

            if (isAttacking) Attack();
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.isGameOver) 
        {
            Move();
        }
    }

    private void Move()
    {
        if (!isMoving) return;

        Vector3 targetPos = target.position;

        if (targetPos.y != transform.position.y)
        {
            Transform ladder = FindLadderRoom(transform.position.y);
            Vector2 targetPosition = new Vector2(ladder.position.x, transform.position.y);

            if(transform.position.x != ladder.position.x)
            {
                Vector2 p = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed);
                rb.MovePosition(p);
            }
            else
            {
                ladder = FindLadderRoom(targetPos.y);
                transform.position = ladder.position;
            }
        }
        else
        {
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

            if (GameManager.Instance.GameLose != null)
            {
                GameManager.Instance.GameLose.Invoke();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private Transform FindLadderRoom(float yPos)
    {
        GameObject[] ladderRoom = GameObject.FindGameObjectsWithTag("Ladder");

        foreach (GameObject lr in ladderRoom)
        {
            if (lr.transform.position.y == yPos)
            {
                return lr.transform;
            }
        }

        return null;
    }

    /*public void SetFurnitureParent(Furniture f)
    {
        furniture = f;
    }*/

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
            target = orang.transform;
        }
    }
}
