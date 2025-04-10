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

    [System.Serializable]
    public class SpawnCollectable
    {
        public GameObject collectable;
        public int numToSpawn = 1;
    }


    [Header("Items to Spawn (spawns them in the order displayed)")]
    [SerializeField] private int numDecor = 1;
    [SerializeField] private int numObstacles = 1;
    [SerializeField] private List<GameObject> decor;
    [SerializeField] private List<SpawnCollectable> collectables;
    [SerializeField] private List<GameObject> obstacles;

    private void Start()
    {
        spawnObjects(decor);
        spawnObjects(collectables);
    }

    private void spawnObjects(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            spawnObject(obj);
        }
    }

    private void spawnObjects(List<SpawnCollectable> objects)
    {
        foreach (SpawnCollectable obj in objects)
        {
            for (int i = 0; i < obj.numToSpawn; i++)
            {
                spawnObject(obj.collectable);
            }
        }
    }

    private void spawnObject(GameObject obj)
    {
        if (obj == null) { return; }

        float x = Random.Range(topLeft.x, bottomRight.x);
        float z = Random.Range(topLeft.y, bottomRight.y);

        RaycastHit hit;

        if (Physics.Raycast(new Vector3(x, height, z), Vector3.down, out hit, rayLength) && hit.collider.tag == "Platform")
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
