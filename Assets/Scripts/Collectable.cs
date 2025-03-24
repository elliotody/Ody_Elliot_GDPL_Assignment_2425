using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public virtual void collect() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collect();
            Destroy(gameObject);
        }
    }
}
