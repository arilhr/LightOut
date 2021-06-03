using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Furniture> furnitures = new List<Furniture>();

    private bool isDanger = false;

    public bool IsDanger { get { return isDanger; } }

    private void Start()
    {
        //SetFurnitureRoom();
    }

    private void Update()
    {
        //Dangerous();
    }

    /*private void SetFurnitureRoom()
    {
        foreach(Furniture f in furnitures)
        {
            f.SetRoom(this);
        }
    }

    private void Dangerous()
    {
        foreach(Furniture f in furnitures)
        {
            if (f.MonsterIsSpawned)
            {
                isDanger = true;
            }
        }
    }*/
}
