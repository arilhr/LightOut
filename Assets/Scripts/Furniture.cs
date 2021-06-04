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
    public Image percentageToAttack;
    private bool spawnMode = false;
    private bool waitMode = false;
    private float currentTimeWait = 0;
    private float waitModeDuration = 4;

    [Header("Sounds")]
    [SerializeField] private AudioSource onSfx;
    [SerializeField] private AudioSource spawnSfx;
    [SerializeField] private AudioSource attackSfx;

    private Animator furnitureAnim;

    public List<Orang> targets = new List<Orang>();

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

    public bool GetSpawnMode { get { return spawnMode; } }
    public bool GetWaitMode { get { return waitMode; } }

    private void Start()
    {
        furnitureAnim = GetComponent<Animator>();
        onSfx.volume = PlayerPrefs.GetFloat("Volume");
        spawnSfx.volume = PlayerPrefs.GetFloat("Volume");
        attackSfx.volume = PlayerPrefs.GetFloat("Volume");
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameOver && !GameManager.Instance.IsPaused)
        {
            CheckToSpawnMonster();
            SpawnMode();
            WaitMode();
        }
    }

    public void AddUser()
    {
        currentUser += 1;
        CheckFull();
    }

    public virtual void Use()
    {
        isOn = true;
        isUsed = true;
        //lastUser = _lastUser;

        furnitureAnim.SetBool("On", true);
        onSfx.Play();
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
        isUsed = false;
        spawnMode = false;
        waitMode = false;

        foreach (Orang o in targets)
        {
            o.SetIsMoving(true);
        }

        targets.RemoveAll(MovingTrue);

        furnitureAnim.SetBool("On", false);
        furnitureAnim.SetBool("Spawn", false);
        furnitureAnim.SetBool("Wait", false);
        onSfx.Stop();
        spawnSfx.Stop();
        attackSfx.Stop();
    }

    private static bool MovingTrue(Orang o)
    {
        return o.IsMoving == true;
    }

    private void SpawnMode()
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
                        o.Attacked();
                    }
                }

                if (targets.Count > 0)
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
            percentageToAttack.color = Color.red;
            percentageToAttack.fillAmount = currentTimeWait / waitModeDuration;
            
            if (currentTimeWait >= waitModeDuration)
            {
                furnitureAnim.SetBool("Wait", false);
                furnitureAnim.SetBool("Attack", true);
                attackSfx.Play();
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
            furnitureAnim.SetBool("Spawn", true);
            spawnSfx.Play();
            currentTimeSpawn += Time.deltaTime;
            percentageToSpawn.color = Color.white;
            percentageToSpawn.fillAmount = currentTimeSpawn / timeToSpawnMonster;
            
            if (currentTimeSpawn >= timeToSpawnMonster)
            {
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
