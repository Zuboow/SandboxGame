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

    static void CraftItem(Recipe recipe) //needs to be adjusted to new quantity system
    {
        bool canCraft = true;
        Dictionary<int, int> inventorySlotsWithQuantity = new Dictionary<int, int>();
        Dictionary<int, int> hotbarSlotsWithQuantity = new Dictionary<int, int>();
        foreach (IDQuantityPair iDQuantityPair in recipe.itemIds)
        {
            int remainingItems = iDQuantityPair.quantity;
            foreach (KeyValuePair<int, Item> item in Inventory.items)
            {
                if (remainingItems > 0 && item.Value != null && item.Value.id == iDQuantityPair.id)
                {
                    inventorySlotsWithQuantity.Add(item.Key, item.Value.quantity - remainingItems <= 0 ? item.Value.quantity : remainingItems);
                    remainingItems -= item.Value.quantity;
                    if (remainingItems <= 0)
                        break;
                }
            }
            foreach (KeyValuePair<int, Item> item in Inventory.hotbarItems)
            {
                if (remainingItems > 0 && item.Value != null && item.Value.id == iDQuantityPair.id)
                {
                    hotbarSlotsWithQuantity.Add(item.Key, item.Value.quantity - remainingItems <= 0 ? item.Value.quantity : remainingItems);
                    remainingItems -= item.Value.quantity;
                    if (remainingItems <= 0)
                        break;
                }
            }
            if (remainingItems > 0)
            {
                canCraft = false;
                Debug.Log("Can't craft item");
                break;
            } else
            {
                Debug.Log("Can craft item");
            }
        }
        if (canCraft)
        {
            foreach (KeyValuePair<int, int> inventorySlotWithQuantity in inventorySlotsWithQuantity)
            {
                if (Inventory.items[inventorySlotWithQuantity.Key].quantity - inventorySlotWithQuantity.Value <= 0)
                {
                    Inventory.items[inventorySlotWithQuantity.Key] = null;
                }
                else
                {
                    Inventory.items[inventorySlotWithQuantity.Key].quantity -= inventorySlotWithQuantity.Value;
                }
            }
            foreach (KeyValuePair<int, int> hotbarSlotWithQuantity in hotbarSlotsWithQuantity)
            {
                if (Inventory.hotbarItems[hotbarSlotWithQuantity.Key].quantity - hotbarSlotWithQuantity.Value <= 0)
                {
                    Inventory.hotbarItems[hotbarSlotWithQuantity.Key] = null;
                }
                else
                {
                    Inventory.hotbarItems[hotbarSlotWithQuantity.Key].quantity -= hotbarSlotWithQuantity.Value;
                }
            }
            int craftedItemsToDrop = SlotManager.AddItem(recipe.id, recipe.quantityMade);
            if (craftedItemsToDrop > 0)
            {
                for (int x = craftedItemsToDrop; x > 0; x--)
                {
                    SlotManager.ThrowItemOut(recipe.id);
                }
            }
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadInventory();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>().ReloadHotbar();
        }
    }
}
