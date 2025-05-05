using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Creates a singleton
    public static GameManager instance;

    // Reference to the player all scripts can access 
    public Player player;

    // Hides these variables in the inspector
    [HideInInspector]
    public int shotsLeft = 10;
    [HideInInspector]
    public int objectivesLeft = 0;

    // Sets up the singlton
    private void Awake()
    {
        instance = this;
    }

    // Sets up the HUD
    private void Start()
    {
        HUD.instance.setTargetsLeft(objectivesLeft);
        HUD.instance.setShotsLeft(shotsLeft);
    }

    // Triggers when the player is shot
    public void useShots(int value = 1) 
    { 
        shotsLeft -= value;
        HUD.instance.setShotsLeft(shotsLeft);
    }

    // Triggers when the Green collectable is hit
    public void addShots(int value = 3) 
    { 
        shotsLeft += value;
        HUD.instance.setShotsLeft(shotsLeft);
        player.resetBall();
    }

    // Triggers when an objective is added to the scene
    public void addObjectives(int value = 1)
    {
        objectivesLeft += value;
        HUD.instance.setTargetsLeft(objectivesLeft);
    }

    // Triggers when an objective is hit 
    public void objectiveShot(int value = 1)
    {
        objectivesLeft -= value;
        player.resetBall();
        HUD.instance.setTargetsLeft(objectivesLeft);
    }
}
