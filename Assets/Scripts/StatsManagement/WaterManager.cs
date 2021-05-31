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
            lastWaterUpdate = currentTime;
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
    }
}
