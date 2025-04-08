using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float rayLength = 10f;

    [Header("Bounds")]
    [SerializeField] private Vector2 topLeft;
    [SerializeField] private Vector2 bottomRight;
    [SerializeField] private float height;

    [Header("Items to Spawn (spawns them in the order displayed)")]
    [SerializeField] private List<GameObject> decor;
    [SerializeField] private List<GameObject> collectables;
    [SerializeField] private List<GameObject> obstacles;

    private void Start()
    {
        spawnObjects(collectables);
    }

    private void spawnObjects(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            spawnObject(obj);
        }
    }

    private void spawnObject(GameObject obj)
    {
        float x = Random.Range(topLeft.x, bottomRight.x);
        float z = Random.Range(topLeft.y, bottomRight.y);

        RaycastHit hit;

        if (Physics.Raycast(new Vector3(x, height, z), Vector3.down, out hit, rayLength))
        {
            Instantiate(obj);
            obj.transform.position = hit.point;
        }
        else
        {
            spawnObject(obj);
        }
    }
}
