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

    public void addShots(int value) 
    { 
        shotsLeft += value;
        HUD.instance.setShotsLeft(shotsLeft);
        player.resetBall();
    }

    public void objectiveShot()
    {
        objectivesLeft -= 1;
        HUD.instance.setTargetsLeft(objectivesLeft);
        player.resetBall();
    }
}
