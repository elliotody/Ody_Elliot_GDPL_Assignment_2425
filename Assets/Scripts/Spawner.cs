using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Spawner class to handle object spawning within a defined area
public class Spawner : MonoBehaviour
{
    [SerializeField] private float rayLength = 10f; // Length of the ray used for detecting spawn positions
    [SerializeField] private GameObject spawnEffect; // Effect that plays when an object is spawned

    [Header("Bounds")]
    [SerializeField] private Vector2 topLeft; // Top-left corner of the spawn area
    [SerializeField] private Vector2 bottomRight; // Bottom-right corner of the spawn area
    [SerializeField] private float height; // Height at which objects are initially placed before raycasting down

    // Class representing objects that can be spawned
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject spawnObject; // Object to spawn
        public int minSpawns = 0; // Minimum number of times to spawn this object
        [Tooltip("If set to 0 this object has no maximum spawns.")]
        public int maxSpawns = 0; // Maximum number of times to spawn this object
        public float checkRadius = 0f; // Radius used to check for nearby objects before spawning
        public bool randomRotation = true; // Whether the object should be randomly rotated upon spawning
        [HideInInspector]
        public bool infiniteSpawns = true; // Whether the object can spawn infinitely
    }

    // Extended class for collectable items
    [System.Serializable]
    public class SpawnableCollectable : SpawnableObject
    {
        public int numToSpawn = 1; // Number of collectables to spawn
    }

    [Header("Items to Spawn (spawns them in the order displayed)")]
    [SerializeField] private int numDecor = 1; // Number of decorative items to spawn
    [SerializeField] private int numObstacles = 1; // Number of obstacles to spawn
    [SerializeField] private List<SpawnableObject> decor; // List of decorative objects
    [SerializeField] private List<SpawnableCollectable> collectables; // List of collectable objects
    [SerializeField] private List<SpawnableObject> obstacles; // List of obstacle objects

    // Start method to initiate spawning
    private void Start()
    {
        spawnRandomObjects(decor, numDecor); // Spawn decorative objects
        spawnCollectables(collectables); // Spawn collectables
        spawnRandomObjects(obstacles, numObstacles); // Spawn obstacles
    }

    // Method to spawn a set number of random objects from a list
    private void spawnRandomObjects(List<SpawnableObject> objects, int numToSpawn = 1)
    {
        foreach (SpawnableObject obj in objects)
        {
            obj.infiniteSpawns = obj.maxSpawns == 0; // Determine if the object has infinite spawns

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

            if (objects[current].maxSpawns > 0 || objects[current].infiniteSpawns)
            {
                spawnObject(objects[current].spawnObject, objects[current].checkRadius, objects[current].randomRotation);
                objects[current].maxSpawns--;
            }
            else
            {
                objects.RemoveAt(current);
                numToSpawn++;
            }
        }
    }

    // Method to spawn collectables
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

    // Method to spawn an individual object at a valid position
    private async void spawnObject(GameObject obj, float checkRadius = 0f, bool randomRotation = false)
    {
        if (checkRadius != 0)
        {
            await Task.Delay(1);
            if (!Application.isPlaying) { return; }
        }

        if (obj == null) { return; }

        float x = Random.Range(topLeft.x, bottomRight.x);
        float z = Random.Range(topLeft.y, bottomRight.y);

        RaycastHit hit;

        // Raycasting downward to find a suitable spawn location
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

                if (numObjects > 0)
                {
                    spawnObject(obj, checkRadius, randomRotation);
                    return;
                }
            }

            GameObject newObj = Instantiate(obj);
            newObj.transform.position = hit.point;
            GameObject effect = Instantiate(spawnEffect);
            effect.transform.position = hit.point + (Vector3.up * 0.5f);

            if (randomRotation)
            {
                newObj.transform.Rotate(Vector3.up, Random.Range(0, 360));
            }
            else
            {
                Vector3 lookPos = new Vector3(GameManager.instance.player.getOriginalPos().x, newObj.transform.position.y, GameManager.instance.player.getOriginalPos().z);
                newObj.transform.LookAt(lookPos);
            }
        }
        else
        {
            spawnObject(obj, checkRadius, randomRotation); // Retry spawning if no valid location is found
        }
    }
}
