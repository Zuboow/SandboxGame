using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public string spriteName;
    public string prefabName;
    public string buildablePrefabName;
    public int quantity;
    public int basicPrice;
    public bool buildable;
    public bool consumable;

    public Item(int _id, string _name, string _spriteName, string _prefabName, string _buildablePrefabName, int _quantity, int _basicPrice, bool _buildable, bool _consumable)
    {
        id = _id;
        name = _name;
        spriteName = _spriteName;
        prefabName = _prefabName;
        buildablePrefabName = _buildablePrefabName;
        quantity = _quantity;
        basicPrice = _basicPrice;
        buildable = _buildable;
        consumable = _consumable;
    }
}

[System.Serializable]
public class Items
{
    public Item[] items;
}
