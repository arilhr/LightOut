using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderRoomManager : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Open(bool b)
    {
        anim.SetBool("Open", b);
    }
}
