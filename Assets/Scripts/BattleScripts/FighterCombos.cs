using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterCombos : MonoBehaviour
{
    public GameObject pMember1;
    public GameObject pMember2;
    public GameObject pMember3;

    public Unit pUnit1;
    public Unit pUnit2;
    public Unit pUnit3;
    
    public GameObject popup1;
    private Vector3 scaleChange;

    private Dictionary<string, Action> fighterDictionary;

    public GameObject batSys;

    void Start()
    {
        pMember1 = GameObject.FindWithTag("player0");
        pMember2 = GameObject.FindWithTag("player1");
        pMember3 = GameObject.FindWithTag("player2");

        pUnit1 = pMember1.GetComponent<Unit>();
        pUnit2 = pMember2.GetComponent<Unit>();
        pUnit3 = pMember3.GetComponent<Unit>();

        batSys = GameObject.FindWithTag("BattleSystem");
        //scaleChange = new Vector3(4.5f, 4.5f, 4.5f);
    }

    public FighterCombos()
    {
        fighterDictionary = new Dictionary<string, Action>
        {
            { "LM", Inspiration },
            { "LLL", SwordDance },
        };
    }

    private void Inspiration()
    {
        Debug.Log("Inspiration!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Inspiration";
        if (pUnit1.attackMultiplier < pUnit1.attackMaximum)
        {
            pUnit1.attackMultiplier += 0.2f;
        }
        if (pUnit2.attackMultiplier < pUnit2.attackMaximum)
        {
            pUnit2.attackMultiplier += 0.2f;
        }
        if (pUnit3.attackMultiplier < pUnit3.attackMaximum)
        {
            pUnit3.attackMultiplier += 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("atk");
        DialogueManager dm = GameManager.Instance?.GetComponentInChildren<DialogueManager>();
        if (dm != null)
        {
            StartCoroutine(dm.StartBattlePopup(pUnit1.neutralPortrait, "Inspiration!", "Party attack increased!"));
        }
        else
        {
            Debug.Log("DialogueManager not found on GameManager.");
        }
    }
    private void SwordDance()
    {
        Debug.Log("Sword Dance!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Sword Dance";
        if (pUnit1.agilityMultiplier < pUnit1.agilityMaximum)
        {
            pUnit1.agilityMultiplier += 0.2f;
        }
        if (pUnit2.agilityMultiplier < pUnit2.agilityMaximum)
        {
            pUnit2.agilityMultiplier += 0.2f;
        }
        if (pUnit3.agilityMultiplier < pUnit3.agilityMaximum)
        {
            pUnit3.agilityMultiplier += 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("agi");
        DialogueManager dm = GameManager.Instance?.GetComponentInChildren<DialogueManager>();
        if (dm != null)
        {
            StartCoroutine(dm.StartBattlePopup(pUnit1.neutralPortrait, "Sword Dance!", "Party agility increased!"));
        }
        else
        {
            Debug.Log("DialogueManager not found on GameManager.");
        }
    }

    public void ExecuteCombo(string comboStr)
    {
        if (fighterDictionary.TryGetValue(comboStr, out var ability))
        {
            ability.Invoke(); // Call the method associated with the combo
            batSys.GetComponent<BattleSystem>().special_attack = true;
        }
        else
        {

        }
    }
    public IEnumerator InspPopup()
    {
        popup1.transform.localScale += scaleChange;
        yield return new WaitForSeconds(2);
        popup1.transform.localScale -= scaleChange;
    }
}
