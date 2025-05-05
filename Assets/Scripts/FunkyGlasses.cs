using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyGlasses : MonoBehaviour
{
    void Start()
    {
        // CHecks if the game is in funky mode
        checkFunky();
    }

    public void checkFunky()
    {
        // Looks in the persistant data to see if the game is funky or not
        gameObject.SetActive(PersistentData.funkyMode);
    }
}
