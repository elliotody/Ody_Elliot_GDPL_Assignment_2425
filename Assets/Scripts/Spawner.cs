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
    public class SpawnableObject
    {
        public GameObject spawnObject;
        public int minSpawns = 0;
        [Tooltip("If set to 0 this object has no maximum spawns.")]
        public int maxSpawns = 0;
    }
    [System.Serializable]
    public class SpawnableCollectable
    {
        public GameObject collectable;
        public int numToSpawn = 1;
    }

    [Header("Items to Spawn (spawns them in the order displayed)")]
    [SerializeField] private int numDecor = 1;
    [SerializeField] private int numObstacles = 1;
    [SerializeField] private List<SpawnableObject> decor;
    [SerializeField] private List<SpawnableCollectable> collectables;
    [SerializeField] private List<SpawnableObject> obstacles;

    private void Start()
    {
        spawnRandomObjects(decor, numDecor);
        spawnCollectables(collectables);
        spawnRandomObjects(obstacles, numObstacles);
    }

    private void spawnRandomObjects(List<SpawnableObject> objects, int numToSpawn = 1)
    {
        foreach (SpawnableObject obj in objects)
        {
            if (obj.minSpawns > 0)
            {
                for (int i = 0; i < obj.minSpawns; i++)
                {
                    spawnObject(obj.spawnObject);
                    obj.maxSpawns -= 1;
                    numToSpawn -= 1;

                    if (numToSpawn <= 0) { return; }
                }
            }
        }

        for (int i = 0; i < numToSpawn; i++)
        {
            if (objects.Count == 0) { return; }

            int current = Mathf.FloorToInt(Random.Range(0, objects.Count));

            if (objects[current].maxSpawns > 0) { spawnObject(objects[current].spawnObject); }
            else { objects.RemoveAt(current); numToSpawn++; }
        }
    }

    private void spawnCollectables(List<SpawnableCollectable> objects)
    {
        foreach (SpawnableCollectable obj in objects)
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
