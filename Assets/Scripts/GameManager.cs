using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Ball player;

    [HideInInspector]
    public int shotsLeft = 10;
    [HideInInspector]
    public int objectivesAchieved = 0;

    private void Awake()
    {
        instance = this;
    }

    public void useShots(int value = 1) 
    { 
        shotsLeft -= value;
    }

    public void addShots(int value) 
    { 
        shotsLeft += value;
        player.resetBall();
    }

    public void objectiveShot()
    {
        objectivesAchieved += 1;
        player.resetBall();
    }
}
