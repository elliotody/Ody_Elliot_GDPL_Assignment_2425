using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] private GameObject extraInfo;

    [SerializeField] private TextMeshProUGUI shotsLeft;
    [SerializeField] private TextMeshProUGUI targetsLeft;
    [SerializeField] private TextMeshProUGUI elevation;
    [SerializeField] private TextMeshProUGUI horizontal;
    [SerializeField] private TextMeshProUGUI power;

    private void Awake()
    {
        instance = this;
    }

    public void setShotsLeft(int value)
    {
        shotsLeft.SetText("Shots Left: " + value);
    }

    public void setTargetsLeft(int value)
    {
        targetsLeft.SetText("Targets Left: " + value);
    }

    public void setElevation(int value)
    {
        elevation.SetText("Elevation: " + value + "°");
    }

    public void setHorizontal(int value)
    {
        horizontal.SetText("Horizontal: " + value + "°");
    }

    public void setPower(int value)
    {
        power.SetText("Power: " + value);
    }

    public void infoButtonClicked()
    {
        extraInfo.SetActive(!extraInfo.activeInHierarchy);
    }
}
