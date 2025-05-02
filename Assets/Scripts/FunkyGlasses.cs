using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyGlasses : MonoBehaviour
{
    void Start()
    {
        checkFunky();
    }

    public void checkFunky()
    {
        gameObject.SetActive(PersistentData.funkyMode);
    }
}
