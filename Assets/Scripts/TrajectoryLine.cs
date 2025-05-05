using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the visualization of the object's trajectory using indicator points
public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab; // Prefab used for trajectory indicator points
    [SerializeField] private int numOfIndicators = 6; // Number of indicators used to display trajectory
    [SerializeField] private float timeStep = 0.01f; // Time interval between each indicator point

    private List<GameObject> indicators; // List storing indicator objects

    private void Start()
    {
        indicators = new List<GameObject>(); // Initialize the indicators list

        // Instantiate indicator objects and store them for later positioning
        for (int i = 0; i < numOfIndicators; i++)
        {
            GameObject newIndicator = Instantiate(indicatorPrefab);
            newIndicator.transform.parent = transform.parent; // Set parent for easier transformations
            indicators.Add(newIndicator);
        }

        hideIndicators(); // Initially hide all indicators
    }

    // Generates a predicted trajectory based on the object's initial velocity and acceleration
    public void createPrediction(float ux, float uy, float uz, float ax, float ay, float az)
    {
        Vector3 pos = new Vector3(); // Stores calculated position
        float t = 0f; // Time variable for physics calculations

        // Loop through indicators and calculate their positions over time
        for (int i = 0; i < indicators.Count; i++)
        {
            t += timeStep; // Increase time step for each indicator

            // Apply kinematic equations to calculate projectile motion
            pos.x = (ux * t) + (0.5f * ax * t * t); // Horizontal movement
            pos.y = (uy * t) + (0.5f * ay * t * t); // Vertical movement
            pos.z = (uz * t) + (0.5f * az * t * t); // Depth movement

            // Update each indicator's position relative to the object's position
            indicators[i].transform.localPosition = new Vector3(pos.z, pos.y, pos.x) + transform.localPosition;
        }
    }

    // Hides all trajectory indicators from view
    public void hideIndicators()
    {
        foreach (GameObject indicator in indicators)
        {
            indicator.GetComponent<MeshRenderer>().enabled = false; // Disable visibility
        }
    }

    // Displays all trajectory indicators
    public void showIndicators()
    {
        foreach (GameObject indicator in indicators)
        {
            indicator.GetComponent<MeshRenderer>().enabled = true; // Enable visibility
        }
    }
}
