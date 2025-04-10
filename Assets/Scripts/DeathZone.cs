using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        { 
            other.GetComponent<Player>().resetBall();
            return;
        }
        else if (other.tag == "Collectable")
        {
            other.GetComponent<Collectable>().collected();
        }

        //Destroy(other.transform.gameObject);
    }
}
