using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public int id;
    public string name;
    public int[] itemIds;
    public int resultId;

    public Recipe(int _id, string _name, int[] _itemIds, int _resultId)
    {
        id = _id;
        name = _name;
        itemIds = _itemIds;
        resultId = _resultId;
    }
}

[System.Serializable]
public class Recipes
{
    public Recipe[] recipes;
}
