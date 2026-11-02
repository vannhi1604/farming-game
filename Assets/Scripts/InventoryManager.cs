using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryItem;
    public Dictionary<Ingredient, int> inventory =
        new Dictionary<Ingredient, int>();

    //ui elements
    List<InventoryItem> itemsUI = new List<InventoryItem>();

    //objects loaded, also a list of every item in game
    Ingredient[] loadedItems;

    private void Awake()
    {
        loadedItems = Resources.LoadAll<Ingredient>("Ingredients");
        foreach (var item in loadedItems)
        {
            inventory.Add(item, 0);
        }
        foreach (var item in inventory)
        {
            InventoryItem newItem = Instantiate(inventoryItem, transform).GetComponent<InventoryItem>();
            newItem.item = item.Key;
            itemsUI.Add(newItem);
        }
    }

    public int GetItemAmount(Ingredient ingredient)
    {
        if (inventory.TryGetValue(ingredient, out int amount))
            return amount;

        return 0;
    }

    //uisng itemName to make it easy for PlotManager to call
    public void AddItem(string itemName, int amount)
    {
        Ingredient ingredient = loadedItems.FirstOrDefault(
            item => item.itemName == itemName
        );

        inventory[ingredient] += amount;
        UpdateInventoryUI(itemName, inventory[ingredient]);
    }

    public bool RemoveItem(Ingredient ingredient, int amount)
    {
        if (GetItemAmount(ingredient) < amount)
        {
            return false;
        }

        inventory[ingredient] -= amount;
        UpdateInventoryUI(ingredient.itemName, inventory[ingredient]);

        return true;
    }

    public void UpdateInventoryUI(string name, int amount)
    {
        int targetIndex = System.Array.FindIndex(
            loadedItems,
            item => item.itemName == name
        );
        itemsUI[targetIndex].amountTxt.text = "x" + amount;
    }
}