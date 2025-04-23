using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public float checkRadius = 0f;
        public bool randomRotation = true;
        [HideInInspector]
        public bool infiniteSpawns = true;
    }
    [System.Serializable]
    public class SpawnableCollectable : SpawnableObject
    {
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
        //Random.InitState(69420);
        spawnRandomObjects(decor, numDecor);
        spawnCollectables(collectables);
        spawnRandomObjects(obstacles, numObstacles);
    }

    private void spawnRandomObjects(List<SpawnableObject> objects, int numToSpawn = 1)
    {
        foreach (SpawnableObject obj in objects)
        {
            if (obj.maxSpawns > 0) { obj.infiniteSpawns = false; } else { obj.infiniteSpawns = true; }

            if (obj.minSpawns > 0)
            {
                for (int i = 0; i < obj.minSpawns; i++)
                {
                    spawnObject(obj.spawnObject, obj.checkRadius, obj.randomRotation);
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

            if (objects[current].maxSpawns > 0 || objects[current].infiniteSpawns) { spawnObject(objects[current].spawnObject, objects[current].checkRadius, objects[current].randomRotation); objects[current].maxSpawns--; }
            else { objects.RemoveAt(current); numToSpawn++; }
        }
    }

    private void spawnCollectables(List<SpawnableCollectable> objects)
    {
        foreach (SpawnableCollectable obj in objects)
        {
            for (int i = 0; i < obj.numToSpawn; i++)
            {
                spawnObject(obj.spawnObject, obj.checkRadius, obj.randomRotation);
            }
        }
    }

    private async void spawnObject(GameObject obj, float checkRadius = 0f, bool randomRotation = false)
    {
        if (checkRadius != 0)
        {
            await Task.Delay(1);
        }

        if (obj == null) { return; }

        float x = Random.Range(topLeft.x, bottomRight.x);
        float z = Random.Range(topLeft.y, bottomRight.y);

        RaycastHit hit;

        if (Physics.Raycast(new Vector3(x, height, z), Vector3.down, out hit, rayLength) && hit.collider.tag == "SpawnArea")
        {
            if (checkRadius != 0)
            {
                Collider[] detectedObjects = Physics.OverlapSphere(hit.point + (Vector3.up * checkRadius / 4), checkRadius);
                int numObjects = detectedObjects.Length;
                foreach (Collider c in detectedObjects)
                {
                    if (c.tag == "SpawnArea" || c.tag == "Platform")
                    {
                        numObjects--;
                    }
                }

                print(numObjects);
                if (numObjects > 0) { spawnObject(obj, checkRadius, randomRotation); return; }

            }
            GameObject newObj = Instantiate(obj);
            newObj.transform.position = hit.point;

            if (randomRotation) { newObj.transform.Rotate(Vector3.up, Random.Range(0, 360)); }
            else 
            {
                Vector3 lookPos = new Vector3(GameManager.instance.player.getOriginalPos().x, newObj.transform.position.y, GameManager.instance.player.getOriginalPos().z);
                newObj.transform.LookAt(lookPos); 
            }
        }
        else
        {
            spawnObject(obj, checkRadius, randomRotation);
        }
    }
}
