using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;

    [HideInInspector]
    public int shotsLeft = 10;
    [HideInInspector]
    public int objectivesLeft = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HUD.instance.setTargetsLeft(objectivesLeft);
        HUD.instance.setShotsLeft(shotsLeft);
    }

    public void useShots(int value = 1) 
    { 
        shotsLeft -= value;
        HUD.instance.setShotsLeft(shotsLeft);
    }

    public void addShots(int value = 3) 
    { 
        shotsLeft += value;
        HUD.instance.setShotsLeft(shotsLeft);
        player.resetBall();
    }

    public void addObjectives(int value = 1)
    {
        objectivesLeft += value;
        HUD.instance.setTargetsLeft(objectivesLeft);
    }

    public void objectiveShot()
    {
        objectivesLeft -= 1;
        player.resetBall();
        HUD.instance.setTargetsLeft(objectivesLeft);
    }

    public void returnToMainMenu()
    {

    }
}
