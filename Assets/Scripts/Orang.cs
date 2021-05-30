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

    private Transform target;

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
        Move();
    }

    private void Move()
    {
        if (!isMoving) return;

        Vector3 furniturePos = currentRoutine.furniture.GetFurniturePosition();

        // check if in the same floor or not
        if (furniturePos.y != transform.position.y)
        {
            Transform ladderRoom = FindLadderRoom(transform.position.y);

            Vector2 targetPosition = new Vector2(ladderRoom.position.x, transform.position.y);
            if (transform.position.x != ladderRoom.position.x)
            {
                Vector2 p = Vector2.MoveTowards(transform.position,
                                                targetPosition,
                                                speed);

                rb.MovePosition(p);
            }
            else
            {
                ladderRoom = FindLadderRoom(furniturePos.y);
                transform.position = ladderRoom.position;
            }
        }
        else
        {
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

    public void Attacked()
    {
        isMoving = false;
    }

    private void Penalties()
    {
        // player tidak bisa melakukan apa-apa selama waktu penalty

    }

    private void DoRoutine()
    {

        // random
        while (true)
        {
            int routineNum = Random.Range(0, routines.Length);
            
            currentRoutine = routines[routineNum];

            
            if (!currentRoutine.furniture.IsFull)
            {
                currentRoutine.furniture.AddUser();
                isMoving = true;
                break;
            }

        }
    }
}
