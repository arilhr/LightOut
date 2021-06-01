using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saklar : Furniture
{
    public Bohlam bohlam;

    public override void Use(GameObject _lastUser)
    {
        base.Use(_lastUser);
        bohlam.On(true);
    }

    public override void TurnOff()
    {
        base.TurnOff();
        bohlam.On(false);
    }
}
