using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
//money
    public int currentMoney;
//inventory slot #1
    public GameObject slot1Item;
    public string slot1Name;
    public int slot1Price;
    public int slot1Quantity;
//inventory slot #2
    public GameObject slot2Item;
    public string slot2Name;
    public int slot2Price;
    public int slot2Quantity;
void Update()
{
    if (slot1Item != null)
    {
        var item1 = slot1Item.GetComponent<ItemDetails>();
        slot1Name = item1.itemName;
        slot1Price = item1.itemSellPrice;
        slot1Quantity = item1.itemQuantity;
    }
    if (slot2Item != null)
    {
        var item2 = slot2Item.GetComponent<ItemDetails>();
        slot2Name = item2.itemName;
        slot2Price = item2.itemSellPrice;
        slot2Quantity = item2.itemQuantity;
    }
}
}