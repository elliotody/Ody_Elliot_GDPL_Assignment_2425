using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCollectable : Collectable
{
    public override void collect()
    {
        GameManager.instance.objectiveShot();
    }
}
