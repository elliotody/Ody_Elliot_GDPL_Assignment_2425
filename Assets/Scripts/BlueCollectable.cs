using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCollectable : Collectable
{
    private void Start()
    {
        GameManager.instance.objectivesLeft++;
    }

    public override void collect()
    {
        GameManager.instance.objectiveShot();
    }
}
