using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HildegardCombos
{
    public string comboNameUI;
    public string comboRouteUI;
    [TextArea(3, 10)]
    public string comboDescriptionUI;
    public bool comboKnown;
}

public class TankCombos : MonoBehaviour
{
    public List<HildegardCombos> CombosUI = new List<HildegardCombos>();
    
    public GameObject pMember1;
    public GameObject pMember2;
    public GameObject pMember3;

    public Unit pUnit1;
    public Unit pUnit2;
    public Unit pUnit3;

    public GameObject popup1;
    private Vector3 scaleChange;
    
    private Dictionary<string, Action> tankDictionary;

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

    public TankCombos()
    {
        tankDictionary = new Dictionary<string, Action>
        {
            { "LM", Bulking },
            {"LLL", Encore},
        };
    }

    private void Bulking()
    {
        Debug.Log("Bulking!");
        if (pUnit1.defenseMultiplier < pUnit1.defenseMaximum)
        {
            pUnit1.defenseMultiplier += 0.2f;
        }
        if (pUnit2.defenseMultiplier < pUnit2.defenseMaximum)
        {
            pUnit2.defenseMultiplier += 0.2f;
        }
        if (pUnit3.defenseMultiplier < pUnit3.defenseMaximum)
        {
            pUnit3.defenseMultiplier += 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("def");
        DialogueManager dm = GameManager.Instance?.GetComponentInChildren<DialogueManager>();
        if (dm != null)
        {
            StartCoroutine(dm.StartBattlePopup(pUnit3.neutralPortrait, "Bulking!", "Party defense increased!"));
        }
        else
        {
            Debug.Log("DialogueManager not found on GameManager.");
        }
    }
    private void Encore()
    {
        Debug.Log("Encore!");
        if (pUnit1.isDead == true)
        {
            pUnit1.isDead = false;
            pUnit1.currentHP = pUnit1.maxHP;
        }
        if (pUnit2.isDead == true)
        {
            pUnit2.isDead = false;
            pUnit2.currentHP = pUnit2.maxHP;
        }
        if (pUnit3.isDead == true)
        {
            pUnit3.isDead = false;
            pUnit3.currentHP = pUnit3.maxHP;
        }
        batSys.GetComponent<BattleSystem>().checkHP();
        DialogueManager dm = GameManager.Instance?.GetComponentInChildren<DialogueManager>();
        if (dm != null && ( pUnit1.isDead == true || pUnit2.isDead == true ))
        {
            StartCoroutine(dm.StartBattlePopup(pUnit3.neutralPortrait, "Enore!", "Revived dead party members!"));
        }
        else
        {
            StartCoroutine(dm.StartBattlePopup(pUnit3.neutralPortrait, "Enore!", "No one to revive!"));
        }
    }

    public void ExecuteCombo(string comboStr)
    {
        if (tankDictionary.TryGetValue(comboStr, out var ability))
        {
            ability.Invoke(); // Call the method associated with the combo
        }
        else
        {

        }
    }
    public IEnumerator BulkPopup()
    {
        popup1.transform.localScale += scaleChange;
        yield return new WaitForSeconds(2);
        popup1.transform.localScale -= scaleChange;
    }
}
