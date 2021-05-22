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

    public Routine[] routines;
    public float speed;

    private Rigidbody2D rb;
    private int indexRoutine = 0;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("RandomMove", 2f);
    }

    private void Update()
    {
        if (isMoving) MoveToFurniture(routines[indexRoutine].furniture);
    }

    private void MoveToFurniture(Furniture _targetFurniture)
    {
        Vector3 furniturePos = _targetFurniture.GetFurniturePosition();
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
            isMoving = false;
            UseFurniture(_targetFurniture);
        }
    }

    private void UseFurniture(Furniture _furnitureToUse)
    {
        _furnitureToUse.Use();

        // animation
    }

    private void RandomMove()
    {
        indexRoutine = Random.Range(0, routines.Length);
        isMoving = true;
    }
}
