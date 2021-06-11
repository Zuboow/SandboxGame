using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingManager : MonoBehaviour
{
    public GameObject barInterface;
    Consumables consumablesFromJSON;

    private void OnEnable()
    {
        consumablesFromJSON = JsonUtility.FromJson<Consumables>((Resources.Load("JSON/consumables") as TextAsset).text);
    }

    public bool ConsumeItem(int id)
    {
        foreach (Consumable consumable in consumablesFromJSON.consumables)
        {
            if (consumable.id == id)
            {
                if (consumable.foodPercentage > 0)
                    barInterface.GetComponent<HungerManager>().ReplenishHunger(consumable.foodPercentage);
                else
                    barInterface.GetComponent<HungerManager>().IncreaseHunger(-consumable.foodPercentage);
                if (consumable.waterPercentage > 0)
                    barInterface.GetComponent<WaterManager>().ReplenishWater(consumable.waterPercentage);
                else
                    barInterface.GetComponent<WaterManager>().IncreaseThirst(-consumable.waterPercentage);
                if (consumable.healthPercentage > 0)
                    barInterface.GetComponent<HealthManager>().HealPlayer(consumable.healthPercentage);
                else
                    barInterface.GetComponent<HealthManager>().HurtPlayer(-consumable.healthPercentage);
                return true;
            }
        }
        return false;
    }
}
