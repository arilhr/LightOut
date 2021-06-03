using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : MonoBehaviour
{
    [Header("User")]
    public int userSlot;
    private int currentUser = 0;
    private bool isOn = false;
    private bool isUsed = false;
    private bool isFull = false;
    private GameObject lastUser;

    [Header("Spawn Monster")]
    public Monster monster;
    public float timeToSpawnMonster;
    private float currentTimeSpawn = 0;
    public Image percentageToSpawn;
    private bool monsterIsSpawned = false;

    private Room room;

    private Animator furnitureAnim;

    public int CurrentUser
    {
        get { return currentUser; }
    }

    public bool IsOn
    {
        get { return isOn; }
    }

    public bool IsFull
    {
        get { return isFull; }
    }

    public bool MonsterIsSpawned
    {
        get { return monsterIsSpawned; }
    }

    private void Start()
    {
        furnitureAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameOver)
        {
            CheckToSpawnMonster();
        }
    }

    public void AddUser()
    {
        currentUser += 1;
        CheckFull();
    }

    public virtual void Use(GameObject _lastUser)
    {
        isOn = true;
        isUsed = true;
        lastUser = _lastUser;

        furnitureAnim.SetBool("On", true);
    }

    public void Unuse()
    {
        currentUser -= 1;
        if (currentUser == 0)
            isUsed = false;

        CheckFull();
    }

    public virtual void TurnOff()
    {
        isOn = false;
        monsterIsSpawned = false;

        furnitureAnim.SetBool("On", false);
    }

    private void CheckFull()
    {
        if (currentUser >= userSlot)
            isFull = true;
        else
            isFull = false;
    }

    public Vector3 GetFurniturePosition()
    {
        return transform.position;
    }

    private void CheckToSpawnMonster()
    {
        if (!isUsed && isOn)
        {
            currentTimeSpawn += Time.deltaTime;
            percentageToSpawn.fillAmount = currentTimeSpawn / timeToSpawnMonster;
            if (currentTimeSpawn >= timeToSpawnMonster)
            {
                SpawnMonster();
                currentTimeSpawn = 0;
            }
        }
        else
        {
            currentTimeSpawn = 0;
            percentageToSpawn.fillAmount = currentTimeSpawn / timeToSpawnMonster;
        }
    }

    private void SpawnMonster()
    {
        var m = Instantiate(monster.gameObject, transform.position, Quaternion.identity);
        Monster mScript = m.GetComponent<Monster>();
        mScript.SetTarget(lastUser.transform);
        monsterIsSpawned = true;

        Debug.Log("Monster is spawned.");
    }

    public void SetRoom(Room r)
    {
        room = r;
    } 

    public virtual void OnMouseDown()
    {
        if (isOn && !Penalty.Instance.isPenalty)
        {
            TurnOff();

            if (currentUser > 0)
            {
                Penalty.Instance.IsPenalty(true);
            }
        }
    }
}
