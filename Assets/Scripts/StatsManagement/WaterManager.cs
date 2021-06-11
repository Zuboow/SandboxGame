using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour
{
    public GameObject waterBar;
    public float waterPointsPercentage = 100f, waterUpdateTime = 2f;
    float currentTime = 0f, lastWaterUpdate = 0f;
    public int waterLossOverTime = 6;

    private void Update()
    {
        currentTime = Time.time;
        if (currentTime - lastWaterUpdate > waterUpdateTime)
        {
            IncreaseThirst(waterLossOverTime);
        }
    }

    public void IncreaseThirst(int amount)
    {
        if (waterPointsPercentage - amount <= 0)
        {
            waterPointsPercentage = 0;
            waterBar.GetComponent<Slider>().value = waterPointsPercentage;
        }
        else
        {
            waterPointsPercentage -= amount;
            waterBar.GetComponent<Slider>().value = waterPointsPercentage;
        }
        lastWaterUpdate = currentTime;
    }

    public void ReplenishWater(int amount)
    {
        if (waterPointsPercentage + amount > 100)
        {
            waterPointsPercentage = 100;
            waterBar.GetComponent<Slider>().value = waterPointsPercentage;
        }
        else
        {
            waterPointsPercentage += amount;
            waterBar.GetComponent<Slider>().value = waterPointsPercentage;
        }
    }
}
