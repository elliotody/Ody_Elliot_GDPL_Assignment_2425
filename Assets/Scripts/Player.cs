using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    [SerializeField] private GameObject smokeEffect;

    [Header("Inputs")]
    [SerializeField] private KeyCode inPowerUp;
    [SerializeField] private KeyCode inPowerDown;
    [SerializeField] private KeyCode inAngleUp;
    [SerializeField] private KeyCode inAngleDown;
    [SerializeField] private KeyCode inAngleLeft;
    [SerializeField] private KeyCode inAngleRight;
    [SerializeField] private KeyCode inShoot;
    [SerializeField] private KeyCode inReset;

    private Vector3 originalPos; // The original position of the ball

    private Rigidbody rb; // Rigidbody component reference
    private TrajectoryLine tl; // Reference to the trajectory line

    private Quaternion savedRot = Quaternion.identity;

    void Start()
    {
        updateTrajectoryLine();
        originalPos = transform.position; // Set the balls initial position for reset
        tl = GetComponent<TrajectoryLine>(); // Get TrajectoryLine component
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
        speedChangeAmount = (maxSpeed - minSpeed) / 9; // Determine step size for speed changes
        currentSpeed = maxSpeed; // Start with maximum speed

        updateHUD(); // Updates the HUD
    }

    void Update()
    {
        // Adjust speed with J/K keys
        if (Input.GetKeyDown(inPowerUp)) { currentPower += 1; updateCurrentSpeed(speedChangeAmount); }
        if (Input.GetKeyDown(inPowerDown)) { currentPower -= 1; updateCurrentSpeed(-speedChangeAmount); }

        // Adjust vertical aim angle with W/S keys
        if (Input.GetKey(inAngleUp)) { updateYAngle(yAngleChange); }
        if (Input.GetKey(inAngleDown)) { updateYAngle(-yAngleChange); }

        // Adjust horizontal aim angle with A/D keys
        if (Input.GetKey(inAngleLeft)) { updateXAngle(-xAngleChange); }
        if (Input.GetKey(inAngleRight)) { updateXAngle(xAngleChange); }

        // Shoot the ball with Space key (starts a coroutine)
        if (Input.GetKeyDown(inShoot)) { StartCoroutine(shootBall()); }
        // Reset the ball with R key (used for debugging)
        if (Input.GetKeyDown(inReset)) { resetBall(); }
    }

    // Returns the player's starting position
    public Vector3 getOriginalPos()
    {
        return originalPos;
    }

    // Shoots the ball in the direction it's facing (as a coroutine so the timer works)
    IEnumerator shootBall()
    {
        if (rb.isKinematic)
        {
            savedRot = rb.rotation;
            GameManager.instance.useShots(); // Uses up a shot
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
            tl.showIndicators(); // Hides the indicators
            //currentXAngle = currentYAngle = 0f; // Resets the current aim angle
            rb.velocity = new Vector3(0f, 0f, 0f); // Reset velocity
            transform.position = originalPos; // Reset position
            rb.rotation = savedRot;
            rb.isKinematic = true; // Disable physics
            StopAllCoroutines(); // Stops the shootBall() coroutine from resetting
            smokeEffect.GetComponent<ParticleSystem>().Play();
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
        if (currentSpeed < minSpeed) { currentSpeed = minSpeed; currentPower = 1; }
        else if (currentSpeed > maxSpeed) { currentSpeed = maxSpeed; currentPower = 10; }

        updateTrajectoryLine(); // Refresh trajectory visualization
        updateHUD();
    }

    // Updates the trajectory visualization
    void updateTrajectoryLine()
    {
        // makes sure tl exists and the ball isn't in motion
        if (tl != null && rb.isKinematic)
        {
            Vector3 u = rb.transform.forward.normalized * currentSpeed;
            tl.createPrediction(u.z, u.y, u.x, 0f, Physics.gravity.y, 0f);
            updateHUD();

            tl.showIndicators(); // shows the indicators
        }
    }

    // Updates the HUD
    public void updateHUD()
    {
        HUD.instance.setElevation((int)currentYAngle);
        HUD.instance.setHorizontal((int)currentXAngle);
        HUD.instance.setPower(currentPower);
    }
}