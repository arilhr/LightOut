using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public int userSlot;
    private int currentUser = 0;
    private bool isOn = false;
    private bool isUsed = false;

    public float timeToSpawnMonster;

    private void Update()
    {
        CheckFurnitureUsed();
    }

    public void Use()
    {
        currentUser++;
        isOn = true;
    }

    public void TurnOff()
    {
        isOn = false;
    }

    public int CurrentUser
    {
        get { return currentUser; }
    }

    public Vector3 GetFurniturePosition()
    {
        return transform.position;
    }

    private void CheckFurnitureUsed()
    {
        if (currentUser == 0) isUsed = false;
    }
}
