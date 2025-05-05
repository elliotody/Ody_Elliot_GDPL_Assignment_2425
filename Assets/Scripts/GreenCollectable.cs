using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCollectable : Collectable
{
    [SerializeField] private int extraShots = 3;

    public override void collect()
    {
        // Adds more shots when the player hits it
        GameManager.instance.addShots(extraShots);
    }
}
