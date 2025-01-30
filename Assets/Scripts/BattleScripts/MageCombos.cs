using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCombos : MonoBehaviour
{
    public GameObject pMember1;
    public GameObject pMember2;
    public GameObject pMember3;

    public Unit pUnit1;
    public Unit pUnit2;
    public Unit pUnit3;

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
        //scaleChange = new Vector3(4.5f, 4.5f, 4.5f);
    }  
   
    public MageCombos()
    {
        mageDictionary = new Dictionary<string, Action>
        {
            { "LM", Overcharge },
            {"LLL", CriticalGlare },
        };
    }

    public void Overcharge()
    {
        Debug.Log("Overcharged!");
        pUnit2.rawIncrease = 5.0f;
    
        DialogueManager dm = GameManager.Instance?.GetComponentInChildren<DialogueManager>();
        if (dm != null)
        {
            StartCoroutine(dm.StartBattlePopup(pUnit2.neutralPortrait, "Overcharged!", "Catalina strengthened her attack!"));
        }
        else
        {
            Debug.Log("DialogueManager not found on GameManager.");
        }

    }
    private void CriticalGlare()
    {
        Debug.Log("Critical Glare!");
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
        DialogueManager dm = GameManager.Instance?.GetComponentInChildren<DialogueManager>();
        if (dm != null)
        {
            StartCoroutine(dm.StartBattlePopup(pUnit2.neutralPortrait, "Critical Glare!", "Party luck increased!"));
        }
        else
        {
            Debug.Log("DialogueManager not found on GameManager.");
        }
    }
    public void ExecuteCombo(string comboStr)
    {
        if (mageDictionary.TryGetValue(comboStr, out var ability))
        {
            ability.Invoke(); // Call the method associated with the combo
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
