using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject settings_window;
    public string activePlayer;

    public Slider strSlider;
    public TMP_Text strText;
    public Slider agiSlider;
    public TMP_Text agiText;
    public Slider lckSlider;
    public TMP_Text lckText;
    public Slider defSlider;
    public TMP_Text defText;
    public Image menuPortrait;
    public Sprite Karlot;
    public Sprite Catalina;
    public Sprite Hildegard;
    public Image gearSlot;
    public TMP_Text itemName;
    public TMP_Text gearDescript;
    public GameObject comboButtonPrefab;
    public GameObject contentPanel;
    private List<GameObject> combo_list = new List<GameObject>();

    void Start()
    {
        ShowKarlotStats();
    }

    public void ShowKarlotStats()
    {
        //This is basic stuff that sets up Karlot in the menu
        var karlot_combos = GameManager.Instance.karlotBattlePrefab.GetComponent<FighterCombos>();
        activePlayer = "p0";
        //Shows his portrait and Stats
        menuPortrait.sprite = Karlot;
        strSlider.value = GameManager.Instance.p0STR;
        strText.text = GameManager.Instance.p0STR.ToString();
        agiSlider.value = GameManager.Instance.p0AGI;
        agiText.text = GameManager.Instance.p0AGI.ToString();
        lckSlider.value = GameManager.Instance.p0LCK;
        lckText.text = GameManager.Instance.p0LCK.ToString();
        defSlider.value = GameManager.Instance.p0DEF;
        defText.text = GameManager.Instance.p0DEF.ToString();

        if (GameManager.Instance.karlotGear != null)
        {
            gearSlot.sprite = GameManager.Instance.karlotGear.GetComponent<Gear>().gearIcon;
            itemName.text = GameManager.Instance.karlotGear.GetComponent<Gear>().gearName;
            gearDescript.text = GameManager.Instance.karlotGear.GetComponent<Gear>().gearDescription;
        }
        else
        {
            gearSlot.sprite = null;
            itemName.text = "NO GEAR";
            gearDescript.text = "Click on the gear slot to equip gear.";
        }

        if (combo_list.Count > 0)
        {
            foreach (GameObject panel in combo_list)
            {
                Destroy(panel);
            }
            combo_list.Clear();
        }
        for (int i = 0; i < karlot_combos.CombosUI.Count; i++)
        {
            if (karlot_combos.CombosUI[i].comboKnown == true)
            {
                GameObject newPanel = Instantiate(comboButtonPrefab, contentPanel.transform);
                combo_list.Add(newPanel);

                var buttext = combo_list[i].GetComponent<UIComboDetails>();
                buttext.comboNameText.text = karlot_combos.CombosUI[i].comboNameUI;
                buttext.comboRouteText.text = karlot_combos.CombosUI[i].comboRouteUI;
                buttext.comboDescriptionText.text = karlot_combos.CombosUI[i].comboDescriptionUI;
            }
        }
    }
    public void ShowCatalinaStats()
    {
        var catalina_combos = GameManager.Instance.catalinaBattlePrefab.GetComponent<MageCombos>();
        activePlayer = "p1";

        menuPortrait.sprite = Catalina;

        strSlider.value = GameManager.Instance.p1STR;
        strText.text = GameManager.Instance.p1STR.ToString();
        agiSlider.value = GameManager.Instance.p1AGI;
        agiText.text = GameManager.Instance.p1AGI.ToString();
        lckSlider.value = GameManager.Instance.p1LCK;
        lckText.text = GameManager.Instance.p1LCK.ToString();
        defSlider.value = GameManager.Instance.p1DEF;
        defText.text = GameManager.Instance.p1DEF.ToString();

        if (GameManager.Instance.catalinaGear != null)
        {
            gearSlot.sprite = GameManager.Instance.catalinaGear.GetComponent<Gear>().gearIcon;
            itemName.text = GameManager.Instance.catalinaGear.GetComponent<Gear>().gearName;
            gearDescript.text = GameManager.Instance.catalinaGear.GetComponent<Gear>().gearDescription;
        }
        else
        {
            gearSlot.sprite = null;
            itemName.text = "NO GEAR";
            gearDescript.text = "Click on the gear slot to equip gear.";
        }

        if (combo_list.Count > 0)
        {
            foreach (GameObject panel in combo_list)
            {
                Destroy(panel);
            }
            combo_list.Clear();
        }
        for (int i = 0; i < catalina_combos.CombosUI.Count; i++)
        {
            if (catalina_combos.CombosUI[i].comboKnown == true)
            {
                GameObject newPanel = Instantiate(comboButtonPrefab, contentPanel.transform);
                combo_list.Add(newPanel);

                var buttext = combo_list[i].GetComponent<UIComboDetails>();
                buttext.comboNameText.text = catalina_combos.CombosUI[i].comboNameUI;
                buttext.comboRouteText.text = catalina_combos.CombosUI[i].comboRouteUI;
                buttext.comboDescriptionText.text = catalina_combos.CombosUI[i].comboDescriptionUI;
            }
        }
    }
    public void ShowHildegardStats()
    {
        var hildegard_combos = GameManager.Instance.hildegardBattlePrefab.GetComponent<TankCombos>();
        activePlayer = "p2";

        menuPortrait.sprite = Hildegard;

        strSlider.value = GameManager.Instance.p2STR;
        strText.text = GameManager.Instance.p2STR.ToString();
        agiSlider.value = GameManager.Instance.p2AGI;
        agiText.text = GameManager.Instance.p2AGI.ToString();
        lckSlider.value = GameManager.Instance.p2LCK;
        lckText.text = GameManager.Instance.p2LCK.ToString();
        defSlider.value = GameManager.Instance.p2DEF;
        defText.text = GameManager.Instance.p2DEF.ToString();

        if (GameManager.Instance.hildegardGear != null)
        {
            gearSlot.sprite = GameManager.Instance.hildegardGear.GetComponent<Gear>().gearIcon;
            itemName.text = GameManager.Instance.hildegardGear.GetComponent<Gear>().gearName;
            gearDescript.text = GameManager.Instance.hildegardGear.GetComponent<Gear>().gearDescription;
        }
        else
        {
            gearSlot.sprite = null;
            itemName.text = "NO GEAR";
            gearDescript.text = "Click on the gear slot to equip gear.";
        }

        if (combo_list.Count > 0)
        {
            foreach (GameObject panel in combo_list)
            {
                Destroy(panel);
            }
            combo_list.Clear();
        }
        for (int i = 0; i < hildegard_combos.CombosUI.Count; i++)
        {
            if (hildegard_combos.CombosUI[i].comboKnown == true)
            {
                GameObject newPanel = Instantiate(comboButtonPrefab, contentPanel.transform);
                combo_list.Add(newPanel);

                var buttext = combo_list[i].GetComponent<UIComboDetails>();
                buttext.comboNameText.text = hildegard_combos.CombosUI[i].comboNameUI;
                buttext.comboRouteText.text = hildegard_combos.CombosUI[i].comboRouteUI;
                buttext.comboDescriptionText.text = hildegard_combos.CombosUI[i].comboDescriptionUI;
            }
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenSettings()
    {
        settings_window.SetActive(true);
    }
    public void CloseSettings()
    {
        settings_window.SetActive(false);
    }
    public void QuitGame()
    {
        // If running in the Unity Editor, this won't quit but will print a message.
        #if UNITY_EDITOR
        Debug.Log("Game is exiting (This won't close the game in the Unity Editor).");
        #else
        Application.Quit(); // Quits the application
        #endif
    }
}
