using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.objectivesLeft += countObjectives();
    }

    int countObjectives()
    {
        int i = 0;

        foreach (BlueCollectable b in transform.GetComponentsInChildren<BlueCollectable>())
        {
            i++;
        }

        return i;
    }
}
