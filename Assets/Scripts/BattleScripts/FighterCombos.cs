using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KarlotCombos
{
    public string comboNameUI;
    public string comboRouteUI;
    [TextArea(3, 10)]
    public string comboDescriptionUI;
    public bool comboKnown;
}

public class FighterCombos : MonoBehaviour
{
    public List<KarlotCombos> CombosUI = new List<KarlotCombos>();
    
    public GameObject pMember1;
    public GameObject pMember2;
    public GameObject pMember3;

    public Unit pUnit1;
    public Unit pUnit2;
    public Unit pUnit3;
    public Unit eUnit;
    
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
        eUnit = batSys.GetComponent<BattleSystem>().targetUnit;
        //scaleChange = new Vector3(4.5f, 4.5f, 4.5f);
    }

    public FighterCombos()
    {
        fighterDictionary = new Dictionary<string, Action>
        {
            { "LM", Inspiration },
            { "LLL", SwordDance },
            { "MLL", PiercingBlow },
            { "HML", MoonDefender },
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

        batSys.GetComponent<BattleSystem>().combo = null;
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

        batSys.GetComponent<BattleSystem>().combo = null;
    }
    private void PiercingBlow()
    {
        Debug.Log("Piercing Blow!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Piercing Blow";
        if (eUnit.defenseMultiplier > eUnit.defenseMinimum)
        {
            eUnit.defenseMultiplier -= 0.2f;
        }
        batSys.GetComponent<BattleSystem>().ChangeBuff("def");

        batSys.GetComponent<BattleSystem>().combo = null;
    }

    private void MoonDefender()
    {
        Debug.Log("Moon Defender!");
        batSys.GetComponent<BattleSystem>().currentSpecialAttack = "Moon Defender";
        var hpcalc = (int)(pUnit1.maxHP * (1f / 4f));
        Debug.Log("lost" + hpcalc + "hp");
        if ((pUnit1.currentHP - hpcalc) <= 0)
        {
            pUnit1.currentHP = 1;
        } 
        else
        {
            pUnit1.currentHP -= hpcalc;
            pUnit1.currentAP += 3;
            Animator[] APanimators = batSys.GetComponent<BattleSystem>().apIndicator[0].GetComponentsInChildren<Animator>();
            foreach (Animator anim in APanimators)
            {
                if (anim != null && anim.gameObject.activeInHierarchy)
                {
                    anim.SetTrigger("IconReset");
                }
            }
        }
    }

    public void ExecuteCombo(string comboStr)
    {
        if (fighterDictionary.TryGetValue(comboStr, out var ability))
        {
            batSys.GetComponent<BattleSystem>().special_attack = true;
            ability.Invoke(); // Call the method associated with the combo
        }
        else
        {

        }
    }
}
