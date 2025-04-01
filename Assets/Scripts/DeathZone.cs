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
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
