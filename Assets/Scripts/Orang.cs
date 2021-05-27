using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orang : MonoBehaviour
{
    [System.Serializable]
    public struct Routine
    {
        public Furniture furniture;
        public float time;
    }

    [Header("Stats")]
    public float speed;

    [Header("Routine")]
    public Routine[] routines;
    private Routine currentRoutine;
    private float currentUseTime = 0;

    [Header("Penalties")]
    public float timePenalty;
    private float currentTimePenalty = 0f;

    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool isUsingFurniture = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DoRoutine();
    }

    private void Update()
    {
        UsingFurniture();
    }

    private void FixedUpdate()
    {
        MoveToFurniture();
    }

    private void MoveToFurniture()
    {
        if (!isMoving) return;

        Vector3 furniturePos = currentRoutine.furniture.GetFurniturePosition();
        Vector2 targetPosition = new Vector2(furniturePos.x, transform.position.y);

        if (transform.position.x != furniturePos.x)
        {
            Vector2 p = Vector2.MoveTowards(transform.position,
                                            targetPosition,
                                            speed);

            rb.MovePosition(p);
        }
        else
        {
            UseFurniture();
            isMoving = false;
        }
    }

    private void UseFurniture()
    {
        currentRoutine.furniture.Use(gameObject);
        isUsingFurniture = true;

        // animation
    }

    private void UsingFurniture()
    {
        if (!isUsingFurniture) return;

        if (currentUseTime >= currentRoutine.time)
        {
            isUsingFurniture = false;
            currentUseTime = 0;
            currentRoutine.furniture.Unuse();
            DoRoutine();
        }
        else
        {
            currentUseTime += Time.deltaTime;
        }
    }

    private void Penalties()
    {
        // player tidak bisa melakukan apa-apa selama waktu penalty

    }

    private void DoRoutine()
    {
        // controlled
        foreach (Routine r in routines)
        {
            if (!r.furniture.IsFull)
            {
                currentRoutine = r;
                break;
            }
        }

        // random
        while (true)
        {
            currentRoutine = routines[Random.Range(0, routines.Length)];

            if (!currentRoutine.furniture.IsFull)
            {
                currentRoutine.furniture.AddUser();
                isMoving = true;
                break;
            }

        }
    }
}
