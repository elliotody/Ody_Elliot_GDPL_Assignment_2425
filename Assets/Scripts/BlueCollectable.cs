using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCollectable : Collectable
{
    private void Awake()
    {
        GameManager.instance.addObjectives();
    }

    public override void collect()
    {
        GameManager.instance.objectiveShot();
    }
}
