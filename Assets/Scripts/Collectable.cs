using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public virtual void collect() { }

    private void OnTriggerEnter(Collider other)
    {
        // Collects if the player hits it
        if (other.tag == "Player")
        {
            collected();
        }
    }

    public void collected()
    {
        // Runs the collect function and then destroys the object
        collect();
        GameManager.instance.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
