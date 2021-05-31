using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public GameObject healthBar, barInterface;
    public float healthPointsPercentage = 100, healthLossSpeed = 5f;
    float currentTime, lastUpdateOnCriticalState = 0f;
    public int healthLossOverTime = 3;
    public static bool playerAlive = true;

    private void Update()
    {
        if (barInterface.GetComponent<HungerManager>().hungerPointsPercentage == 0 || barInterface.GetComponent<WaterManager>().waterPointsPercentage == 0)
        {
            currentTime = Time.time;
            if (currentTime - lastUpdateOnCriticalState > healthLossSpeed)
            {
                lastUpdateOnCriticalState = currentTime;
                HurtPlayer(healthLossOverTime);
            }
        }
    }

    public void HurtPlayer(int damage)
    {
        if (healthPointsPercentage - damage <= 0)
        {
            playerAlive = false;
            healthPointsPercentage = 0;
            healthBar.GetComponent<Slider>().value = healthPointsPercentage;
        }
        else
        {
            healthPointsPercentage -= damage;
            healthBar.GetComponent<Slider>().value = healthPointsPercentage;
        }
    }

    public void HealPlayer(int healing)
    {
        if (healthPointsPercentage + healing > 100)
        {
            healthPointsPercentage = 100;
            healthBar.GetComponent<Slider>().value = healthPointsPercentage;
        }
        else
        {
            healthPointsPercentage += healing;
            healthBar.GetComponent<Slider>().value = healthPointsPercentage;
        }
    }
}
