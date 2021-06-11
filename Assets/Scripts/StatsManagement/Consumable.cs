using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable
{
    public int id;
    public string name;
    public int foodPercentage;
    public int waterPercentage;
    public int healthPercentage;
    public string consumableType;
}

[System.Serializable]
public class Consumables
{
    public Consumable[] consumables;
}
