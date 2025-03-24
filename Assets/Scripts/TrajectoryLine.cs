using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    // Prefab for trajectory indicator points
    [SerializeField] private GameObject indicatorPrefab;
    // Reference to the Ball object
    [SerializeField] private Ball ball;
    // Number of indicators used to visualize the trajectory
    [SerializeField] private int numOfIndicators = 6;
    // Time step between each indicator point (the gap between the points)
    [SerializeField] private float timeStep = 0.01f;

    private List<GameObject> indicators; // List to store indicator objects
    public Rigidbody rb; // Rigidbody component reference

    private void Start()
    {
        transform.position = ball.transform.position; // Set initial position to match the ball
        rb = transform.GetComponent<Rigidbody>(); // Get Rigidbody component
        indicators = new List<GameObject>(); // Initialize the list

        // Instantiate indicator objects and store them in the list
        for (int i = 0; i < numOfIndicators; i++)
        {
            GameObject newIndicator = Instantiate(indicatorPrefab);
            newIndicator.transform.parent = transform; // Set parent to apply rotations easier
            indicators.Add(newIndicator);
        }
        hideIndicators(); // Hide indicators after they are created
    }

    // Creates the predicted trajectory based on the ball's initial velocity and acceleration
    public void createPrediction(float ux, float uy, float ax, float ay)
    {
        Vector2 pos = new Vector2(); // Position variable for SUVAT
        float t = 0f; // Time variable for SUVAT

        // Loop through indicators and calculate their positions
        for (int i = 0; i < indicators.Count; i++)
        {
            t += timeStep; // Increment time (gap between indicators)

            // Apply kinematic equations for projectile motion (YAY MATHS!)
            pos.x = (ux * t) + (0.5f * ax * t * t);
            pos.y = (uy * t) + (0.5f * ay * t * t);

            // Update indicator position relative to the ball
            indicators[i].transform.localPosition = new Vector3(0f, pos.y, pos.x);
        }

        showIndicators(); // Make indicators visible
    }

    // Hides all trajectory indicators
    public void hideIndicators()
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Shows all trajectory indicators
    public void showIndicators()
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
