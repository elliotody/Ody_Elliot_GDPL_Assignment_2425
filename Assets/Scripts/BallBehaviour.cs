using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Speed properties
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float minSpeed = 5f;

    private int currentPower = 10; // Represents power level affecting speed
    private float currentSpeed = 0f; // Current ball speed
    private float speedChangeAmount; // Change in speed between power levels

    // Rotation angles for aiming
    [SerializeField] private float maxXAngle = 45f;

    [SerializeField] private float yAngleChange = 10f;
    private float currentYAngle = 0f;

    [SerializeField] private float xAngleChange = 10f;
    private float currentXAngle = 0f;

    private Vector3 originalPos; // The original position of the ball

    private Rigidbody rb; // Rigidbody component reference
    private TrajectoryLine tl; // Reference to the trajectory line

    void Start()
    {
        originalPos = transform.position; // Set the balls initial position for reset
        tl = GetComponent<TrajectoryLine>(); // Get TrajectoryLine component
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
        speedChangeAmount = (maxSpeed - minSpeed) / 10; // Determine step size for speed changes
        currentSpeed = maxSpeed; // Start with maximum speed
    }

    void Update()
    {
        // Adjust speed with J/K keys
        if (Input.GetKeyDown(KeyCode.J)) { currentPower += 1; updateCurrentSpeed(speedChangeAmount); }
        if (Input.GetKeyDown(KeyCode.K)) { currentPower -= 1; updateCurrentSpeed(-speedChangeAmount); }

        // Adjust vertical aim angle with W/S keys
        if (Input.GetKey(KeyCode.W)) { updateYAngle(yAngleChange); }
        if (Input.GetKey(KeyCode.S)) { updateYAngle(-yAngleChange); }

        // Adjust horizontal aim angle with A/D keys
        if (Input.GetKey(KeyCode.D)) { updateXAngle(xAngleChange); }
        if (Input.GetKey(KeyCode.A)) { updateXAngle(-xAngleChange); }

        // Shoot the ball with Space key (starts a coroutine)
        if (Input.GetKeyDown(KeyCode.Space)) { StartCoroutine(shootBall()); }
        // Reset the ball with R key (used for debugging)
        if (Input.GetKeyDown(KeyCode.R)) { resetBall(); }
    }

    // Shoots the ball in the direction it's facing (as a coroutine so the timer works)
    IEnumerator shootBall()
    {
        GameManager.instance.useShots(); // Uses up a shot
        if (tl != null) { tl.hideIndicators(); } // Hide trajectory indicators when the ball is shot
        if (rb.isKinematic)
        {
            rb.isKinematic = false; // Enable physics
            rb.velocity = rb.transform.forward.normalized * currentSpeed; // Apply velocity
            yield return new WaitForSeconds(10); // Wait for 10 seconds
            resetBall(); // Reset the ball
        }
    }

    // Resets the ball to its initial state
    public void resetBall()
    {

        if (!rb.isKinematic)
        {
            currentXAngle = currentYAngle = 0f;
            rb.velocity = new Vector3(0f, 0f, 0f); // Reset velocity
            transform.position = originalPos; // Reset position
            rb.rotation = Quaternion.Euler(0f, 0f, 0f); // Reset rotation
            rb.isKinematic = true; // Disable physics
            StopAllCoroutines(); // Stops the shootBall() coroutine from resetting
            updateTrajectoryLine(); // Update trajectory visualization
            tl.hideIndicators(); // Hides the indicators
        }
    }

    // Updates vertical aim angle
    void updateYAngle(float value)
    {
        currentYAngle += value * Time.deltaTime;
        // Clamp vertical angle between 0 and 89 degrees
        if (currentYAngle < 0) { currentYAngle = 0; }
        else if (currentYAngle > 89) { currentYAngle = 89; }

        rb.rotation = Quaternion.Euler(-currentYAngle, currentXAngle, rb.rotation.z);
        updateTrajectoryLine(); // Refresh trajectory visualization
    }

    // Updates horizontal aim angle
    void updateXAngle(float value)
    {
        currentXAngle += value * Time.deltaTime;
        // Clamp horizontal angle between -45 and 45 degrees
        if (currentXAngle < -maxXAngle) { currentXAngle = -maxXAngle; }
        else if (currentXAngle > maxXAngle) { currentXAngle = maxXAngle; }

        rb.rotation = Quaternion.Euler(-currentYAngle, currentXAngle, rb.rotation.z);
        updateTrajectoryLine(); // Refresh trajectory visualization
    }

    // Updates the current speed of the ball based on power changes
    void updateCurrentSpeed(float value)
    {
        currentSpeed += value;
        // Ensure speed stays within min and max limits
        if (currentSpeed < minSpeed) { currentSpeed = minSpeed; currentPower = 0; }
        else if (currentSpeed > maxSpeed) { currentSpeed = maxSpeed; currentPower = 10; }

        updateTrajectoryLine(); // Refresh trajectory visualization
    }

    // Updates the trajectory visualization
    void updateTrajectoryLine()
    {
        // makes sure tl exists and the ball isn't in motion
        if (tl != null && rb.isKinematic)
        {
            Vector3 u = rb.transform.forward.normalized * currentSpeed;
            //tl.rb.rotation = Quaternion.Euler(0f, currentXAngle, 0f);
            tl.createPrediction(u.z, u.y, u.x, 0f, Physics.gravity.y, 0f);
        }
    }
}