using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum GearPanelStates {PanelOpen, PanelClosed,}

public class GearMenu : MonoBehaviour
{
    public GameObject itemPanel;
    public GameObject content;
    public GameObject buttonPrefab;
    private List<GameObject> item_list = new List<GameObject>();
    public GearPanelStates panelState;
    private GameObject selectedGear;
    public PauseMenuButtons pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        panelState = GearPanelStates.PanelClosed;
    }

    public void UpdatePanel()
    {
        if (item_list.Count > 0)
        {
            foreach (GameObject panel in item_list)
            {
                Destroy(panel);
            }
            item_list.Clear();
        }

        if (panelState == GearPanelStates.PanelClosed)
        {
            ActivatePanel();
            panelState = GearPanelStates.PanelOpen;
        }
        else if ( panelState == GearPanelStates.PanelOpen)
        {
            DeactivatePanel();
            panelState = GearPanelStates.PanelClosed;
        }
    }
    
    private void ActivatePanel()
    {
        itemPanel.SetActive(true);
        var inv = GameManager.Instance.GetComponent<PlayerInventory>().inventorySlots;
        foreach (InventorySlot slot in inv)
        {
            GameObject newPanel = Instantiate(buttonPrefab, content.transform);
            item_list.Add(newPanel);

            var pInfo = newPanel.GetComponent<GearButtonUI>();
            pInfo.itemNameUI.text = slot.gearItemPrefab.GetComponent<Gear>().gearName;
            pInfo.itemDescriptionUI.text = slot.gearItemPrefab.GetComponent<Gear>().gearDescription;
            pInfo.gearImageUI.sprite = slot.gearItemPrefab.GetComponent<Gear>().gearIcon;
            pInfo.itemQuantityUI.text = "QTY: " + slot.gearItemQuantity.ToString();

            newPanel.GetComponent<Button>().onClick.AddListener(() => EquipGear(slot.gearItemPrefab));
        }
    }
    private void DeactivatePanel()
    {
        itemPanel.SetActive(false);
    }

    public void EquipGear(GameObject gearItem)
    {
        PlayerInventory playinv = GameManager.Instance.GetComponent<PlayerInventory>();
        if (pauseMenu == null) return;

        Gear gear = gearItem.GetComponent<Gear>();
        if (gear == null) return;

        switch (pauseMenu.activePlayer)
        {
            case "p0":
                if (GameManager.Instance.karlotGear != null)
                {
                    UnequipStatChanges(GameManager.Instance.karlotGear); // Remove old gear's effects
                    playinv.AddToInventory(GameManager.Instance.karlotGear);
                }
                GameManager.Instance.karlotGear = gearItem;
                ApplyStatChanges(gear, "p0"); // Apply new gear's effects
                playinv.RemoveFromInventory(gearItem);
                break;
            case "p1":
                if (GameManager.Instance.catalinaGear != null)
                {
                    UnequipStatChanges(GameManager.Instance.catalinaGear);
                    playinv.AddToInventory(GameManager.Instance.catalinaGear);
                }
                GameManager.Instance.catalinaGear = gearItem;
                ApplyStatChanges(gear, "p1");
                playinv.RemoveFromInventory(gearItem);
                break;
            case "p2":
                if (GameManager.Instance.hildegardGear != null)
                {
                    UnequipStatChanges(GameManager.Instance.hildegardGear);
                    playinv.AddToInventory(GameManager.Instance.hildegardGear);
                }
                GameManager.Instance.hildegardGear = gearItem;
                ApplyStatChanges(gear, "p2");
                playinv.RemoveFromInventory(gearItem);
                break;
        }

        pauseMenu.UpdateStatScreen();
        UpdatePanel();
    }

    public void UnequipGear()
    {
        PlayerInventory playinv = GameManager.Instance.GetComponent<PlayerInventory>();
        if (pauseMenu == null) return;

        switch (pauseMenu.activePlayer)
        {
            case "p0":
                if (GameManager.Instance.karlotGear != null)
                {
                    UnequipStatChanges(GameManager.Instance.karlotGear);
                    playinv.AddToInventory(GameManager.Instance.karlotGear);
                    GameManager.Instance.karlotGear = null;
                }
                break;
            case "p1":
                if (GameManager.Instance.catalinaGear != null)
                {
                    UnequipStatChanges(GameManager.Instance.catalinaGear);
                    playinv.AddToInventory(GameManager.Instance.catalinaGear);
                    GameManager.Instance.catalinaGear = null;
                }
                break;
            case "p2":
                if (GameManager.Instance.hildegardGear != null)
                {
                    UnequipStatChanges(GameManager.Instance.hildegardGear);
                    playinv.AddToInventory(GameManager.Instance.hildegardGear);
                    GameManager.Instance.hildegardGear = null;
                }
                break;
        }

        pauseMenu.UpdateStatScreen();
        UpdatePanel();
    }

    private void ApplyStatChanges(Gear gear, string character)
    {
        switch (character)
        {
            case "p0":
                if (gear.changesHealth) GameManager.Instance.p0MaxHP += gear.healthChangedBy;
                if (gear.changesStrength) GameManager.Instance.p0STR += gear.strengthChangedBy;
                if (gear.changesAgility) GameManager.Instance.p0AGI += gear.agilityChangedBy;
                if (gear.changesLuck) GameManager.Instance.p0LCK += gear.luckChangedBy;
                if (gear.changesDefense) GameManager.Instance.p0DEF += gear.denfeseChangedBy;
                break;
            case "p1":
                if (gear.changesHealth) GameManager.Instance.p1MaxHP += gear.healthChangedBy;
                if (gear.changesStrength) GameManager.Instance.p1STR += gear.strengthChangedBy;
                if (gear.changesAgility) GameManager.Instance.p1AGI += gear.agilityChangedBy;
                if (gear.changesLuck) GameManager.Instance.p1LCK += gear.luckChangedBy;
                if (gear.changesDefense) GameManager.Instance.p1DEF += gear.denfeseChangedBy;
                break;
            case "p2":
                if (gear.changesHealth) GameManager.Instance.p2MaxHP += gear.healthChangedBy;
                if (gear.changesStrength) GameManager.Instance.p2STR += gear.strengthChangedBy;
                if (gear.changesAgility) GameManager.Instance.p2AGI += gear.agilityChangedBy;
                if (gear.changesLuck) GameManager.Instance.p2LCK += gear.luckChangedBy;
                if (gear.changesDefense) GameManager.Instance.p2DEF += gear.denfeseChangedBy;
                break;
        }
    }

    private void UnequipStatChanges(GameObject gearItem)
    {
        Gear gear = gearItem.GetComponent<Gear>();
        if (gear == null) return;

        switch (pauseMenu.activePlayer)
        {
            case "p0":
                if (gear.changesHealth) GameManager.Instance.p0MaxHP -= gear.healthChangedBy;
                if (gear.changesStrength) GameManager.Instance.p0STR -= gear.strengthChangedBy;
                if (gear.changesAgility) GameManager.Instance.p0AGI -= gear.agilityChangedBy;
                if (gear.changesLuck) GameManager.Instance.p0LCK -= gear.luckChangedBy;
                if (gear.changesDefense) GameManager.Instance.p0DEF -= gear.denfeseChangedBy;
                break;
            case "p1":
                if (gear.changesHealth) GameManager.Instance.p1MaxHP -= gear.healthChangedBy;
                if (gear.changesStrength) GameManager.Instance.p1STR -= gear.strengthChangedBy;
                if (gear.changesAgility) GameManager.Instance.p1AGI -= gear.agilityChangedBy;
                if (gear.changesLuck) GameManager.Instance.p1LCK -= gear.luckChangedBy;
                if (gear.changesDefense) GameManager.Instance.p1DEF -= gear.denfeseChangedBy;
                break;
            case "p2":
                if (gear.changesHealth) GameManager.Instance.p2MaxHP -= gear.healthChangedBy;
                if (gear.changesStrength) GameManager.Instance.p2STR -= gear.strengthChangedBy;
                if (gear.changesAgility) GameManager.Instance.p2AGI -= gear.agilityChangedBy;
                if (gear.changesLuck) GameManager.Instance.p2LCK -= gear.luckChangedBy;
                if (gear.changesDefense) GameManager.Instance.p2DEF -= gear.denfeseChangedBy;
                break;
        }
    }
}
