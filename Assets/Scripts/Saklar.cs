using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saklar : Furniture
{
    public Bohlam bohlam;

    /*public override void Use(])
    {
        base.Use();
        bohlam.On(true);
    }*/

    public override void TurnOff()
    {
        base.TurnOff();
        bohlam.On(false);
    }
}
