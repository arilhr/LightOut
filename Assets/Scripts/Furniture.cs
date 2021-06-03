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
    //public Monster monster;
    public float timeToSpawnMonster;
    private float currentTimeSpawn = 0;
    public Image percentageToSpawn;
    private bool spawnMode = false;
    private bool waitMode = false;
    private float currentTimeWait = 0;
    private float waitModeDuration = 4;

    private Animator furnitureAnim;

    private List<Orang> targets = new List<Orang>();

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

    private void Start()
    {
        furnitureAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameOver)
        {
            CheckToSpawnMonster();
            DetectNPC();
            WaitMode();
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
        spawnMode = false;
        waitMode = false;

        furnitureAnim.SetBool("On", false);
        furnitureAnim.SetBool("Spawn", false);
        furnitureAnim.SetBool("Wait", false);
    }

    private void DetectNPC()
    {
        if (spawnMode)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2);

            foreach (Collider2D col in hits)
            {
                Orang o = col.GetComponent<Orang>();

                if (o != null)
                {
                    targets.Add(o);
                }
            }

            if(targets.Count > 0)
            {
                furnitureAnim.SetBool("Wait", true);
                furnitureAnim.SetBool("Spawn", false);
                spawnMode = false;
                waitMode = true;
            }
        }
    }

    private void WaitMode()
    {
        if (waitMode)
        {
            currentTimeWait += Time.deltaTime;
            percentageToSpawn.color = Color.red;
            percentageToSpawn.fillAmount = currentTimeWait / waitModeDuration;
            
            if (currentTimeWait >= waitModeDuration)
            {
                furnitureAnim.SetBool("Wait", false);
                furnitureAnim.SetBool("Attack", true);
                currentTimeWait = 0;

                foreach (Orang o in targets)
                {
                    o.Attacked();
                }

                waitMode = false;

                GameManager.Instance.isGameOver = true;
                GameManager.Instance.GameLose.Invoke();
            }
        }
        else
        {
            currentTimeWait = 0;
            percentageToSpawn.fillAmount = currentTimeWait / waitModeDuration;
        }
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
        if (!isUsed && isOn && !spawnMode)
        {
            currentTimeSpawn += Time.deltaTime;
            percentageToSpawn.color = Color.white;
            percentageToSpawn.fillAmount = currentTimeSpawn / timeToSpawnMonster;

            if (currentTimeSpawn >= timeToSpawnMonster)
            {
                furnitureAnim.SetBool("Spawn", true);
                spawnMode = true;
                currentTimeSpawn = 0;
                percentageToSpawn.fillAmount = 0;
                //SpawnMonster();
                //currentTimeSpawn = 0;
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
        //var m = Instantiate(monster.gameObject, transform.position, Quaternion.identity);
        //Monster mScript = m.GetComponent<Monster>();
        //mScript.SetTarget(lastUser.transform);
        //monsterIsSpawned = true;

        Debug.Log("Monster is spawned.");
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
