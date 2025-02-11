using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public GameObject gearItemPrefab;
    public int gearItemQuantity;
}

public class PlayerInventory : MonoBehaviour
{ 
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public void AddToInventory(GameObject newItem)
    {
        bool inInventory = false;
        foreach(InventorySlot slot in inventorySlots)//Checks each inventory slot to see if we already own this item
        {
            if (slot.gearItemPrefab == newItem)//if so, it adds and the nullifies it
            {
                inInventory = true;
                slot.gearItemQuantity += 1;
                return;
            }
        }
        if (inInventory == false)//if it isn't in the inventory, we create a new slot and put the item in
        {
            InventorySlot new_slot = new InventorySlot
            {
                gearItemPrefab = newItem,
                gearItemQuantity = 1   
            };
            inventorySlots.Add(new_slot);
        }
    }

    public void RemoveFromInventory(GameObject itemRemoved)
    {
        foreach(InventorySlot slot in inventorySlots)
        {
            if (slot.gearItemPrefab == itemRemoved)
            {
                if (slot.gearItemQuantity > 1)
                {
                    slot.gearItemQuantity -= 1;
                }
                else
                {
                    inventorySlots.Remove(slot);
                }
            }
        }
    }
}
