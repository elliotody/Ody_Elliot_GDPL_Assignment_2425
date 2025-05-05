using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the player mechanics, including movement, aiming, and shooting
public class Player : MonoBehaviour
{
    // Speed properties for the projectile
    [SerializeField] private float maxSpeed = 25f; // Maximum speed of the projectile
    [SerializeField] private float minSpeed = 5f;  // Minimum speed of the projectile

    private int currentPower = 10; // Represents power level affecting speed
    private float currentSpeed = 0f; // Current speed of the projectile
    private float speedChangeAmount; // Incremental change in speed per power level

    // Rotation angles for aiming
    [SerializeField] private float maxXAngle = 45f; // Maximum horizontal aiming angle
    [SerializeField] private float yAngleChange = 10f; // Change rate for vertical aiming
    private float currentYAngle = 0f; // Current vertical aiming angle
    [SerializeField] private float xAngleChange = 10f; // Change rate for horizontal aiming
    private float currentXAngle = 0f; // Current horizontal aiming angle

    [SerializeField] private GameObject smokeEffect; // Effect when the ball resets

    [Header("Inputs")]
    [SerializeField] private KeyCode inPowerUp;   // Key for increasing power
    [SerializeField] private KeyCode inPowerDown; // Key for decreasing power
    [SerializeField] private KeyCode inAngleUp;   // Key for increasing vertical angle
    [SerializeField] private KeyCode inAngleDown; // Key for decreasing vertical angle
    [SerializeField] private KeyCode inAngleLeft; // Key for rotating left
    [SerializeField] private KeyCode inAngleRight;// Key for rotating right
    [SerializeField] private KeyCode inShoot;     // Key for shooting
    [SerializeField] private KeyCode inReset;     // Key for resetting position

    private Vector3 originalPos; // Stores the player's original position for resets

    private Rigidbody rb; // Reference to Rigidbody component for physics interactions
    private TrajectoryLine tl; // Reference to trajectory visualization component

    private Quaternion savedRot = Quaternion.identity; // Stores rotation for reset
    private bool gameEnded = false; // Tracks whether the game has ended

    void Start()
    {
        updateTrajectoryLine();
        originalPos = transform.position; // Stores the starting position
        tl = GetComponent<TrajectoryLine>(); // Gets trajectory visualization component
        rb = GetComponent<Rigidbody>(); // Gets Rigidbody component for movement
        speedChangeAmount = (maxSpeed - minSpeed) / 9; // Calculates step size for speed increments
        currentSpeed = maxSpeed; // Starts at max speed

        updateHUD(); // Updates the HUD
        gameEnded = false;
    }

    void Update()
    {
        if (gameEnded) { return; } // Prevents further input processing if the game has ended

        // Adjust speed based on input keys
        if (Input.GetKeyDown(inPowerUp)) { currentPower += 1; updateCurrentSpeed(speedChangeAmount); }
        if (Input.GetKeyDown(inPowerDown)) { currentPower -= 1; updateCurrentSpeed(-speedChangeAmount); }

        // Adjust aiming angles based on input keys
        if (Input.GetKey(inAngleUp)) { updateYAngle(yAngleChange); }
        if (Input.GetKey(inAngleDown)) { updateYAngle(-yAngleChange); }
        if (Input.GetKey(inAngleLeft)) { updateXAngle(-xAngleChange); }
        if (Input.GetKey(inAngleRight)) { updateXAngle(xAngleChange); }

        // Shoot projectile using Space key
        if (Input.GetKeyDown(inShoot)) { StartCoroutine(shootBall()); }

        // Reset position and properties using R key (mainly for debugging)
        if (Input.GetKeyDown(inReset)) { resetBall(); }
    }

    // Returns the player's original position
    public Vector3 getOriginalPos()
    {
        return originalPos;
    }

    // Shoots the projectile in the direction it's facing
    IEnumerator shootBall()
    {
        if (rb.isKinematic) // Ensures the ball is in a ready state before shooting
        {
            savedRot = rb.rotation; // Stores current rotation
            GameManager.instance.useShots(); // Consumes a shot
            rb.isKinematic = false; // Enables physics interactions
            rb.velocity = rb.transform.forward.normalized * currentSpeed; // Sets movement velocity
            yield return new WaitForSeconds(10); // Waits for 10 seconds before resetting
            resetBall(); // Resets ball position and properties
        }
    }

    // Resets ball position, rotation, and physics properties
    public void resetBall()
    {
        if (!rb.isKinematic)
        {
            tl.showIndicators(); // Refreshes trajectory indicators
            rb.velocity = Vector3.zero; // Stops movement
            transform.position = originalPos; // Resets to original position
            rb.rotation = savedRot; // Resets rotation
            rb.isKinematic = true; // Disables physics interactions
            StopAllCoroutines(); // Cancels active coroutines
            smokeEffect.GetComponent<ParticleSystem>().Play(); // Plays reset effect

            // Determines if the game should end based on objectives or remaining shots
            if (GameManager.instance.objectivesLeft <= 0) { WinLose.instance.win(); gameEnded = true; }
            else if (GameManager.instance.shotsLeft <= 0) { WinLose.instance.lose(); gameEnded = true; }
        }
    }

    // Adjusts vertical aiming angle
    void updateYAngle(float value)
    {
        currentYAngle += value * Time.deltaTime;
        currentYAngle = Mathf.Clamp(currentYAngle, 0, 89); // Restricts angle to valid range

        rb.rotation = Quaternion.Euler(-currentYAngle, currentXAngle, rb.rotation.z);
        updateTrajectoryLine(); // Refreshes trajectory visualization
    }

    // Adjusts horizontal aiming angle
    void updateXAngle(float value)
    {
        currentXAngle += value * Time.deltaTime;
        currentXAngle = Mathf.Clamp(currentXAngle, -maxXAngle, maxXAngle); // Restricts angle to valid range

        rb.rotation = Quaternion.Euler(-currentYAngle, currentXAngle, rb.rotation.z);
        updateTrajectoryLine(); // Refreshes trajectory visualization
    }

    // Adjusts projectile speed based on power level
    void updateCurrentSpeed(float value)
    {
        currentSpeed += value;
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed); // Keeps speed within valid range

        updateTrajectoryLine(); // Refreshes trajectory visualization
        updateHUD(); // Updates UI elements
    }

    // Refreshes trajectory visualization based on aiming and speed
    void updateTrajectoryLine()
    {
        if (tl != null && rb.isKinematic)
        {
            Vector3 u = rb.transform.forward.normalized * currentSpeed;
            tl.createPrediction(u.z, u.y, u.x, 0f, Physics.gravity.y, 0f);
            updateHUD();
            tl.showIndicators(); // Displays trajectory indicators
        }
    }

    // Updates HUD elements with current aiming angles and power level
    public void updateHUD()
    {
        HUD.instance.setElevation((int)currentYAngle);
        HUD.instance.setHorizontal((int)currentXAngle);
        HUD.instance.setPower(currentPower);
    }
}
