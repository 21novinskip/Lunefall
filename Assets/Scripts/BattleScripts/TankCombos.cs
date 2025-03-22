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
    public Unit eUnit;

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
        eUnit = batSys.GetComponent<BattleSystem>().targetUnit;
        //scaleChange = new Vector3(4.5f, 4.5f, 4.5f);
    }

    public TankCombos()
    {
        tankDictionary = new Dictionary<string, Action>
        {
            { "LM", Bulking },
            { "LLL", Encore },
            { "MLL", PunishingBlow },
            { "HML", Protector },
        };
    }

    private void Bulking()
    {
        Debug.Log("Bulking!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Bulking";
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
        batSys.GetComponent<BattleSystem>().combo = null;
    }
    private void Encore()
    {
        Debug.Log("Encore!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Envoke";
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
        batSys.GetComponent<BattleSystem>().combo = null;
    }
    private void PunishingBlow()
    {
        Debug.Log("Punishing Blow!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Punishing Blow";
        if (eUnit.attackMultiplier > eUnit.attackMinimum)
        {
            eUnit.attackMultiplier -= 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("atk");

        batSys.GetComponent<BattleSystem>().combo = null;
    }
    private void Protector()
    {
        Debug.Log("Protector of the Meek!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Protector";
        var protectr = ((5 - (pUnit1.currentHP / pUnit1.maxHP)) + (5 - (pUnit2.currentHP / pUnit2.maxHP)));
        pUnit3.rawIncrease = protectr;

        batSys.GetComponent<BattleSystem>().combo = null;
    }

    public void ExecuteCombo(string comboStr)
    {
        if (tankDictionary.TryGetValue(comboStr, out var ability))
        {
            ability.Invoke(); // Call the method associated with the combo
            batSys.GetComponent<BattleSystem>().special_attack = true;
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
