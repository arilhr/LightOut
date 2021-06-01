using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bohlam : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void On(bool b)
    {
        anim.SetBool("On", b);
    }
}
