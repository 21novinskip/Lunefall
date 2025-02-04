using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GearSlot
{
    public string ItemName;
    public Sprite Icon;
    public GameObject ItemPrefab;
    [TextArea(3, 10)]
    public string ItemDescription;

    public GearSlot(string itemName, Sprite icon, GameObject itemPrefab, string itemDescription)
    {
        ItemName = itemName;
        Icon = icon;
        ItemPrefab = itemPrefab;
        ItemDescription = itemDescription;
    }
}

[System.Serializable]
public class GearList
{
    public List<GearSlot> GearSlots = new List<GearSlot>();

    public void AddGear(GearSlot gear)
    {
        GearSlots.Add(gear);
    }
}

public class PlayerGear : MonoBehaviour
{
    public GearList playerGear = new GearList();

    public void EquipGear(GearSlot newGear)
    {
        playerGear.AddGear(newGear);
        Debug.Log($"Equipped: {newGear.ItemName}");
    }
}
