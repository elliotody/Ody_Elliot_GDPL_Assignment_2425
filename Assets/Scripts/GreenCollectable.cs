using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCollectable : Collectable
{
    [SerializeField] int extraShots = 3;

    public override void collect()
    {
        GameManager.instance.addShots(extraShots);
    }
}
