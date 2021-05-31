using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public int id;
    public string name;
    public int[] itemIds;

    public Recipe(int _id, string _name, int[] _itemIds)
    {
        id = _id;
        name = _name;
        itemIds = _itemIds;
    }
}

[System.Serializable]
public class Recipes
{
    public Recipe[] recipes;
}
