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
                barInterface.GetComponent<HungerManager>().ReplenishHunger(consumable.foodPercentage);
                barInterface.GetComponent<WaterManager>().ReplenishWater(consumable.waterPercentage);
                barInterface.GetComponent<HealthManager>().HealPlayer(consumable.healthPercentage);
                return true;
            }
        }
        return false;
    }
}
