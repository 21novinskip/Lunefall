using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string ItemName;
    public Sprite Icon;
    public int MaxStack;
    public GameObject ItemPrefab;

    public InventoryItem(string itemName, Sprite icon, int maxStack, GameObject itemPrefab)
    {
        ItemName = itemName;
        Icon = icon;
        MaxStack = maxStack;
        ItemPrefab = itemPrefab;
    }
}

[System.Serializable]
public class InventorySlot
{
    public InventoryItem Item;
    public int Quantity;

    public InventorySlot(InventoryItem item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    public bool CanAddItem(InventoryItem item, int quantity)
    {
        return Item == item && Quantity + quantity <= Item.MaxStack;
    }

    public void AddItem(int quantity)
    {
        Quantity += quantity;
    }
}

public class Inventory : MonoBehaviour
{
    public int Capacity;
    public List<InventorySlot> Slots = new List<InventorySlot>();

    public bool AddItem(InventoryItem item, int quantity)
    {
        foreach (var slot in Slots)
        {
            if (slot.CanAddItem(item, quantity))
            {
                slot.AddItem(quantity);
                return true;
            }
        }

        if (Slots.Count < Capacity)
        {
            Slots.Add(new InventorySlot(item, quantity));
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public bool RemoveItem(InventoryItem item, int quantity)
    {
        foreach (var slot in Slots)
        {
            if (slot.Item == item)
            {
                if (slot.Quantity >= quantity)
                {
                    slot.Quantity -= quantity;
                    if (slot.Quantity == 0)
                    {
                        Slots.Remove(slot);
                    }
                    return true;
                }
                else
                {
                    Debug.Log("Not enough items to remove.");
                    return false;
                }
            }
        }

        Debug.Log("Item not found in inventory.");
        return false;
    }

    public void PrintInventory()
    {
        foreach (var slot in Slots)
        {
            Debug.Log($"Item: {slot.Item.ItemName}, Quantity: {slot.Quantity}");
        }
    }
}
