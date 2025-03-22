using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CatalinaCombos
{
    public string comboNameUI;
    public string comboRouteUI;
    [TextArea(3, 10)]
    public string comboDescriptionUI;
    public bool comboKnown;
}

public class MageCombos : MonoBehaviour
{
    public List<CatalinaCombos> CombosUI = new List<CatalinaCombos>();
    
    public GameObject pMember1;
    public GameObject pMember2;
    public GameObject pMember3;

    public Unit pUnit1;
    public Unit pUnit2;
    public Unit pUnit3;
    public Unit eUnit;

    public GameObject popup1;
    private Vector3 scaleChange;
    
    private Dictionary<string, Action> mageDictionary;

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
   
    public MageCombos()
    {
        mageDictionary = new Dictionary<string, Action>
        {
            { "LM", CriticalGlare },
            { "LLL", StagnantBlow },
            { "MLL", TragicBlow },
            { "HML", Overcharge },
        };
    }
    private void StagnantBlow()
    {
        Debug.Log("Stagnant Blow!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Stagnant Blow";
        if (eUnit.agilityMultiplier > eUnit.agilityMinimum)
        {
            eUnit.agilityMultiplier -= 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("agi");

        batSys.GetComponent<BattleSystem>().combo = null;
    }
    private void TragicBlow()
    {
        Debug.Log("Tragic Blow!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Tragic Blow";
        if (eUnit.luckMultiplier > eUnit.luckMinimum)
        {
            eUnit.luckMultiplier -= 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("lck");

        batSys.GetComponent<BattleSystem>().combo = null;
    }
    public void Overcharge()
    {
        Debug.Log("Overcharged!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Overcharged";
        pUnit2.rawIncrease = 3.5f;

        batSys.GetComponent<BattleSystem>().combo = null;
    }
    private void CriticalGlare()
    {
        Debug.Log("Critical Glare!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Critical Glare";
        if (pUnit1.luckMultiplier < pUnit1.luckMaximum)
        {
            pUnit1.luckMultiplier += 0.2f;
        }
        if (pUnit2.luckMultiplier < pUnit2.luckMaximum)
        {
            pUnit2.luckMultiplier += 0.2f;
        }
        if (pUnit3.luckMultiplier < pUnit3.luckMaximum)
        {
            pUnit3.luckMultiplier += 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("lck");
        batSys.GetComponent<BattleSystem>().combo = null;
    }
    public void ExecuteCombo(string comboStr)
    {
        if (mageDictionary.TryGetValue(comboStr, out var ability))
        {
            ability.Invoke(); // Call the method associated with the combo
            batSys.GetComponent<BattleSystem>().special_attack = true;
        }
        else
        {

        }
    }
    public IEnumerator OvchPopup()
    {
        popup1.transform.localScale += scaleChange;
        yield return new WaitForSeconds(2);
        popup1.transform.localScale -= scaleChange;
    }
}
