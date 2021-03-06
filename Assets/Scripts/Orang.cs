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
    private Routine prevRoutine;
    private float currentUseTime = 0;

    [Header("Penalties")]
    public float timePenalty;
    private float currentTimePenalty = 0f;

    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool isUsingFurniture = false;

    private Transform target;

    private Animator anim;

    private float facing;

    public bool IsMoving { get { return isMoving; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DoRoutine();
        anim = GetComponent<Animator>();
    }

    private void Update()
    { 
        UsingFurniture();

        if (isMoving)
        {
            anim.SetBool("Idle", false);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.isGameOver && !GameManager.Instance.IsPaused)
        {
            Move();
        }
    }

    private void Move()
    {
        if (!isMoving) return;

        Vector3 furniturePos = currentRoutine.furniture.GetFurniturePosition();

        // check if in the same floor or not
        if (furniturePos.y != transform.position.y)
        {
            Transform door = FindDoor(transform.position.y);

            Vector2 targetPosition = new Vector2(door.position.x, transform.position.y);
            Vector2 dir = targetPosition - (Vector2)transform.position;

            if (transform.position.x != door.position.x)
            {
                Vector2 p = Vector2.MoveTowards(transform.position,
                                                targetPosition,
                                                speed);

                rb.MovePosition(p);
                anim.SetBool("Idle", false);
                if(dir.normalized == Vector2.right)
                {
                    anim.SetFloat("Facing", 1);
                }
                else
                {
                    anim.SetFloat("Facing", -1);
                }
            } 
            else
            {
                door = FindLadderRoom(furniturePos.y);
                transform.position = door.position;
            } 
        }
        else
        {
            Vector2 targetPosition = new Vector2(furniturePos.x, transform.position.y);
            Vector2 dir = targetPosition - (Vector2)transform.position;
            if (transform.position.x != furniturePos.x)
            {
                Vector2 p = Vector2.MoveTowards(transform.position,
                                                targetPosition,
                                                speed);

                rb.MovePosition(p);
                anim.SetBool("Idle", false);

                if (dir.normalized == Vector2.right)
                {
                    anim.SetFloat("Facing", 1);
                }
                else
                {
                    anim.SetFloat("Facing", -1);
                }
            }
            else
            {
                if(!currentRoutine.furniture.GetSpawnMode || !currentRoutine.furniture.GetWaitMode || !currentRoutine.furniture.IsOn)
                {
                    UseFurniture();
                    isMoving = false;
                }
                else
                {
                    DoRoutine();
                }
            }
        }
    }

    private void UseFurniture()
    {
        currentRoutine.furniture.Use();
        currentRoutine.furniture.AddUser();
        isUsingFurniture = true;

        // animation
        anim.SetBool("Idle", true);
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

    private Transform FindDoor(float yPos)
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject d in doors)
        {
            if (d.transform.position.y == yPos)
            {
                return d.transform;
            }
        }

        return null;
    }

    public void Attacked()
    {
        isMoving = false;
        anim.SetBool("Idle", true);
    }

    public void SetIsMoving(bool m)
    {
        isMoving = m;
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

            if(currentRoutine.furniture != prevRoutine.furniture)
            {
                if (!currentRoutine.furniture.IsFull)
                {
                    prevRoutine = currentRoutine;
                    //currentRoutine.furniture.AddUser();
                    isMoving = true;
                    break;
                }
            }
        }
    }

    IEnumerator LadderAnimDelayTime(float time, Transform ladder)
    {
        LadderRoomManager ladderRoom = ladder.GetComponent<LadderRoomManager>();
        ladderRoom.Open(true);
        yield return new WaitForSeconds(time);
        ladderRoom.Open(false);
    }

    IEnumerator DoorAnimDelayTime(float time, Transform door)
    {
        Door doorObj = door.GetComponent<Door>();
        doorObj.Open(true);
        yield return new WaitForSeconds(time);
        doorObj.Open(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            StartCoroutine(LadderAnimDelayTime(0.5f, collision.gameObject.transform));
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            StartCoroutine(DoorAnimDelayTime(0.5f, collision.gameObject.transform));
        }
    }
}
