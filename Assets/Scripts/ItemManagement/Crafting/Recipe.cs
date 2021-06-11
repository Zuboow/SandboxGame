using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public int id;
    public string name;
    public IDQuantityPair[] itemIds;
    public int quantityMade;

    public Recipe(int _id, string _name, IDQuantityPair[] _itemIds, int _quantityMade)
    {
        id = _id;
        name = _name;
        itemIds = _itemIds;
        quantityMade = _quantityMade;
    }
}

[System.Serializable]
public class Recipes
{
    public Recipe[] recipes;
}

[System.Serializable]
public class IDQuantityPair
{
    public int id;
    public int quantity;
}
