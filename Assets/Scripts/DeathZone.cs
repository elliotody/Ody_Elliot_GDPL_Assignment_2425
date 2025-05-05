using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Resets the ball if the player hits the zone
        if (other.tag == "Player")
        { 
            other.GetComponent<Player>().resetBall();
            return;
        }
        // Collects the collectable if it hits the zone
        else if (other.tag == "Collectable")
        {
            other.GetComponent<Collectable>().collected();
            return;
        }

        // Destroys any other objects so they don't take up memory any more
        Destroy(other.transform.parent.gameObject);
    }
}
