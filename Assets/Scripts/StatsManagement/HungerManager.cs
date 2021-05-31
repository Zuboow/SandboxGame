using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerManager : MonoBehaviour
{
    public GameObject hungerBar;
    public float hungerPointsPercentage = 100f, hungerUpdateTime = 4f;
    float currentTime = 0f, lastHungerUpdate = 0f;
    public int hungerLossOverTime = 4;

    private void Update()
    {
        currentTime = Time.time;
        if (currentTime - lastHungerUpdate > hungerUpdateTime)
        {
            IncreaseHunger(hungerLossOverTime);
            lastHungerUpdate = currentTime;
        }
    }

    public void IncreaseHunger(int amount)
    {
        if (hungerPointsPercentage - amount <= 0)
        {
            hungerPointsPercentage = 0;
            hungerBar.GetComponent<Slider>().value = hungerPointsPercentage;
        }
        else
        {
            hungerPointsPercentage -= amount;
            hungerBar.GetComponent<Slider>().value = hungerPointsPercentage;
        }
    }
}
