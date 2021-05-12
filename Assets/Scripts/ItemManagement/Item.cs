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
    public int quantity;

    public Item(int _id, string _name, string _spriteName, string _prefabName, int _quantity)
    {
        id = _id;
        name = _name;
        spriteName = _spriteName;
        prefabName = _prefabName;
        quantity = _quantity;
    }
}

[System.Serializable]
public class Items
{
    public Item[] items;
}
