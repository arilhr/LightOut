using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public int userSlot;
    private int currentUser = 0;
    private bool isOn = false;
    private bool isUsed = false;
    private bool isFull = false;

    public float timeToSpawnMonster;
    private float currentTimeSpawn = 0;

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

    private void Start()
    {
        furnitureAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckFurnitureUsed();
    }

    public void AddUser()
    {
        currentUser += 1;
        CheckFull();
    }

    public void Use()
    {
        isOn = true;

        furnitureAnim.SetBool("On", true);
    }

    public void Unuse()
    {
        currentUser -= 1;
        CheckFull();
    }

    public void TurnOff()
    {
        isOn = false;

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

    private void CheckFurnitureUsed()
    {
        if (currentUser == 0) isUsed = false;
    }

    private void OnMouseDown()
    {
        if (isOn)
            TurnOff();
    }
}
