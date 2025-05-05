using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCollectable : Collectable
{
    [SerializeField] private int worth = 1;

    private void Awake()
    {
        // Increases the amount of objectives left
        GameManager.instance.addObjectives(worth);
    }

    public override void collect()
    {
        // Decreases the amount of objectives left
        GameManager.instance.objectiveShot(worth);
    }
}
