using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMenu : MonoBehaviour
{
    public GameObject itemPanel;
    public GameObject content;
    public GameObject buttonPrefab;
    private List<GameObject> item_list = new List<GameObject>();
    // Start is called before the first frame update
    public void ActivatePanel()
    {
        itemPanel.SetActive(true);
        item_list.Clear();
        var inv = GameManager.Instance.GetComponent<PlayerInventory>().inventorySlots;
        for (int i = 0; i < inv.Count; i++)
        {
            GameObject newPanel = Instantiate(buttonPrefab, content.transform);
            item_list.Add(newPanel);

            var pInfo = newPanel.GetComponent<GearButtonUI>();
            //pInfo.itemNameUI = inv.gearItemPrefab.
        }
    }
}
