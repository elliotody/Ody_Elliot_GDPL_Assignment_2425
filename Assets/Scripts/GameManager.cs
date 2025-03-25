using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Player player;

    [HideInInspector]
    public int shotsLeft = 10;
    [HideInInspector]
    public int objectivesLeft;

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
        objectivesLeft -= 1;
        player.resetBall();
    }
}
