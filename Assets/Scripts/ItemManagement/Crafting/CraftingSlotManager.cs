using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlotManager : MonoBehaviour
{
    public static Recipes recipesFromJSON;
    static Recipe currentRecipe;

    private void OnEnable()
    {
        if (recipesFromJSON == null)
            recipesFromJSON = JsonUtility.FromJson<Recipes>((Resources.Load("JSON/recipes") as TextAsset).text);
    }
    public void StartCraftingProccess()
    {
        if (Input.GetKeyDown(KeyCode.E) && HealthManager.playerAlive)
        {
            int checkedCraftingListIndex = Int32.Parse(name.Split('_')[1]);

            if (CraftingList.craftingOptions[checkedCraftingListIndex] != null)
            {
                foreach (Recipe recipe in recipesFromJSON.recipes)
                {
                    if (recipe.id == CraftingList.craftingOptions[checkedCraftingListIndex].id)
                    {
                        currentRecipe = recipe;
                        CraftItem(recipe);
                    }
                }
            }
        }
    }

    static void CraftItem(Recipe recipe)
    {
        bool canCraft = true;
        List<int> inventorySlotsWithRequiredItems = new List<int>();
        List<int> hotbarSlotsWithRequiredItems = new List<int>();
        foreach (int itemId in recipe.itemIds)
        {
            bool itemAvailable = false;
            foreach (KeyValuePair<int, Item> item in Inventory.items)
            {
                if (!itemAvailable && item.Value != null && item.Value.id == itemId && !inventorySlotsWithRequiredItems.Contains(item.Key))
                {
                    inventorySlotsWithRequiredItems.Add(item.Key);
                    itemAvailable = true;
                    break;
                }
            }
            if (!itemAvailable)
                foreach (KeyValuePair<int, Item> item in Inventory.hotbarItems)
                {
                    if (!itemAvailable && item.Value != null && item.Value.id == itemId && !hotbarSlotsWithRequiredItems.Contains(item.Key))
                    {
                        hotbarSlotsWithRequiredItems.Add(item.Key);
                        itemAvailable = true;
                    }
                }
            if (!itemAvailable)
            {
                canCraft = false;
                Debug.Log("Can't craft item");
                break;
            }
        }
        if (canCraft)
        {
            foreach (int slotId in inventorySlotsWithRequiredItems)
            {
                Inventory.items[slotId] = null;
            }
            foreach (int slotId in hotbarSlotsWithRequiredItems)
            {
                Inventory.hotbarItems[slotId] = null;
            }
            SlotManager.AddItem(recipe.id, 1);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadInventory();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadHotbar();
        }
    }
}
