using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Reference to the trajectory line
    [SerializeField] private TrajectoryLine tl;

    // Speed and physics-related properties
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float gravitySpeed = 9.8f;

    private int currentPower = 10; // Represents power level affecting speed
    private float currentSpeed = 0f; // Current ball speed
    private float SpeedChangeAmount; // Change in speed between power levels

    // Rotation angles for aiming
    [SerializeField] private float yAngleChange = 10f;
    private float currentYAngle = 0f;

    [SerializeField] private float xAngleChange = 10f;
    private float currentXAngle = 0f;

    private Rigidbody rb; // Rigidbody component reference

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
        SpeedChangeAmount = (maxSpeed - minSpeed) / 10; // Determine step size for speed changes
        currentSpeed = maxSpeed; // Start with maximum speed
        updateTrajectoryLine(); // Make the trajectory visuals
    }

    void Update()
    {
        // Adjust power and speed with J/K keys
        if (Input.GetKeyDown(KeyCode.J)) { currentPower += 1; updateCurrentSpeed(SpeedChangeAmount); }
        if (Input.GetKeyDown(KeyCode.K)) { currentPower -= 1; updateCurrentSpeed(-SpeedChangeAmount); }

        // Adjust vertical aim angle with W/S keys
        if (Input.GetKey(KeyCode.W)) { updateYAngle(yAngleChange); }
        if (Input.GetKey(KeyCode.S)) { updateYAngle(-yAngleChange); }

        // Adjust horizontal aim angle with A/D keys
        if (Input.GetKey(KeyCode.D)) { updateXAngle(xAngleChange); }
        if (Input.GetKey(KeyCode.A)) { updateXAngle(-xAngleChange); }

        // Shoot the ball with Space key
        if (Input.GetKeyDown(KeyCode.Space)) { shootBall(); }
        // Reset the ball with R key (used for debugging)
        if (Input.GetKeyDown(KeyCode.R)) { resetBall(); }
    }

    void LateUpdate()
    {
        // Apply gravity manually when ball is in motion (gives more control)
        if (rb.isKinematic == false) { rb.velocity -= new Vector3(0f, gravitySpeed * Time.deltaTime, 0f); }
    }

    // Shoots the ball in the direction it's facing
    void shootBall()
    {
        if (rb.isKinematic)
        {
            rb.isKinematic = false; // Enable physics
            rb.velocity = rb.transform.forward.normalized * currentSpeed; // Apply velocity
        }
        if (tl != null) { tl.hideIndicators(); } // Hide trajectory indicators when the ball is shot
    }

    // Resets the ball to its initial state
    void resetBall()
    {
        if (!rb.isKinematic)
        {
            rb.velocity = transform.localPosition = new Vector3(0, 0, 0); // Reset position and velocity
            rb.rotation = Quaternion.Euler(0f, 0f, 0f); // Reset rotation
            rb.isKinematic = true; // Disable physics
        }
        updateTrajectoryLine(); // Update trajectory visualization
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
        if (currentXAngle < -45) { currentXAngle = -45; }
        else if (currentXAngle > 45) { currentXAngle = 45; }

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
            tl.rb.rotation = Quaternion.Euler(0f, currentXAngle, 0f);
            tl.createPrediction(u.z, u.y, 0f, -gravitySpeed);
        }
    }
}