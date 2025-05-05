using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill_Blades : MonoBehaviour
{
    [SerializeField] private float speed = 3;

    private void Update()
    {
        // Animates the windmill
        transform.Rotate(-Vector3.forward, speed * Time.deltaTime);
    }
}
