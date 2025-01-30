/**
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//The States of the battle
public enum BattleStateB{START, PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, ENEMYTURN1, ENEMYTURN2, ENEMYTURN3, WON, LOST, PASSING, ANIMATING, RESET}

public class BattleSystemB : MonoBehaviour
{
    private Vector3 scaleChange;
//all the things we need
    public static BattleSystem Instance;
[Header("Player Prefabs (fed from gamemanager)")]
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    private GameObject newPlayer1;
    private GameObject newPlayer2;
    private GameObject newPlayer3;
[Header("Enemy Prefabs (fed from gamemanager)")]
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    private GameObject newEnemy1;
    private GameObject newEnemy2;
    private GameObject newEnemy3;
[Header("Prefab Spawn Locations")]
    public GameObject playerSpawn1;
    public GameObject playerSpawn2;
    public GameObject playerSpawn3;
    public GameObject enemySpawn1;
    public GameObject enemySpawn2;
    public GameObject enemySpawn3;
[Header("UI Menus")]
    public GameObject playerMenu1;
    public GameObject playerMenu2;
    public GameObject playerMenu3;

    public Slider playerHPBar1;
    public Slider playerHPBar2;
    public Slider playerHPBar3;
    public Slider enemyHPBar1;
[Header("UI Text")]
    public TMP_Text P1Text;
    public TMP_Text P2Text;
    public TMP_Text P3Text;
    public GameObject P1AP;
    private APIndicator apInd1;
    public GameObject P2AP;
    private APIndicator apInd2;
    public GameObject P3AP;
    private APIndicator apInd3;
    private APIndicator currentInd;
    public TMP_Text P1HP;
    public TMP_Text P2HP;
    public TMP_Text P3HP;
    //public TMP_Text E1Text;
    //public TMP_Text E2Text;
    //public TMP_Text E3Text;
    public TMP_Text E1AP;
    public TMP_Text E2AP;
    public TMP_Text E3AP;

    Unit playerUnit1;
    Unit playerUnit2;
    Unit playerUnit3;

    Unit enemyUnit;
    Unit enemyUnit2;
    Unit enemyUnit3;

    Unit targetUnit;

    Unit activeUnit;
    private GameObject activeMenu;
    private Animator playerAnim1;
    private Animator playerAnim2;
    private Animator playerAnim3;
    private Animator turnAnim;

    private bool p1Acted = false;
    private bool p2Acted = false;
    private bool p3Acted = false;

[Header("Please dont touch (probably ok if you touch it tho)")]
    private bool wePlaying = false;
    public BattleState state;
    public BattleState lastState;
    private String combo;
    public GameObject target;

[Header("Sounds")]
public AudioSource snd_Light;
public AudioSource snd_LightM;
public AudioSource snd_LightP;
public AudioSource snd_LightE;
public AudioSource snd_Medium;
public AudioSource snd_MediumM;
public AudioSource snd_MediumP;
public AudioSource snd_MediumE;
public AudioSource snd_Heavy;
public AudioSource snd_HeavyM;
public AudioSource snd_HeavyP;
public AudioSource snd_HeavyE;


//Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        Debug.Log("State changed to: " + state);
        turnAnim = GetComponent<Animator>();
        Debug.Log("Battle is Starting...");
        SetupBattle();
    }
//Assigning objects and components to variables for ease of use    
    void SetupBattle()
    {
        InitializeUnits();
        InitializeUI();
        StartCoroutine(playerPhase());
    }
    void InitializeUnits()
    {
        playerPrefab1 = GameManager.Instance.player_prefab1;
        playerPrefab2 = GameManager.Instance.player_prefab2;
        playerPrefab3 = GameManager.Instance.player_prefab3;

        apInd1 = P1AP.GetComponent<APIndicator>();
        apInd2 = P2AP.GetComponent<APIndicator>();
        apInd3 = P3AP.GetComponent<APIndicator>();

        if (playerSpawn1 != null)
        {
            newPlayer1 = Instantiate(playerPrefab1, playerPrefab1.transform.position, Quaternion.identity);
            newPlayer1.tag = "player1";
            Debug.Log("Player Character 1 spawned!");
        }
        if (playerSpawn2 != null)
        {
            newPlayer2 = Instantiate(playerPrefab2, playerPrefab2.transform.position, Quaternion.identity);
            newPlayer2.tag = "player2";
            Debug.Log("Player Character 2 spawned!");
        }
        if (playerSpawn3 != null)
        {
            newPlayer3 = Instantiate(playerPrefab3, playerPrefab3.transform.position, Quaternion.identity);
            newPlayer3.tag = "player3";
            Debug.Log("Player Character 3 spawned!");
        }
        if (enemySpawn1 != null && enemyPrefab1 != null)
        {
            newEnemy1 = Instantiate(enemyPrefab1, enemySpawn1.transform.position, Quaternion.identity);
            Debug.Log("Enemy 1 spawned!");
        }
        if (enemySpawn2 != null && enemyPrefab2 != null)
        {
            newEnemy2 = Instantiate(enemyPrefab2, enemyPrefab2.transform.position, Quaternion.identity);
            Debug.Log("Enemy 1 spawned!");
        }
        if (enemySpawn3 != null && enemyPrefab3 != null)
        {
            newEnemy3 = Instantiate(enemyPrefab3, enemyPrefab3.transform.position, Quaternion.identity);
            Debug.Log("Enemy 1 spawned!");
        }
        playerUnit1 = newPlayer1.GetComponent<Unit>();
        playerUnit2 = newPlayer2.GetComponent<Unit>();
        playerUnit3 = newPlayer3.GetComponent<Unit>();
        playerAnim1 = newPlayer1.GetComponentInChildren<Animator>();
        playerAnim2 = newPlayer2.GetComponentInChildren<Animator>();
        playerAnim3 = newPlayer3.GetComponentInChildren<Animator>();
        
        enemyUnit = newEnemy1.GetComponent<Unit>();
        enemyUnit.currentHP = enemyUnit.maxHP;
        if (newEnemy2 != null)
        {
            enemyUnit2 = newEnemy2.GetComponent<Unit>();
            enemyUnit2.currentHP = enemyUnit2.maxHP;
        }
        if (newEnemy3 != null)
        {
            enemyUnit3 = newEnemy3.GetComponent<Unit>();
            enemyUnit3.currentHP = enemyUnit3.maxHP;
        }
    }
    void InitializeUI()
    {
        playerHPBar1.maxValue = playerUnit1.maxHP;
        playerHPBar2.maxValue = playerUnit2.maxHP;
        playerHPBar3.maxValue = playerUnit3.maxHP;

        enemyHPBar1.maxValue = enemyUnit.maxHP;

        //Sets Units current #of actions to = their max #of actions
        playerUnit1.currentAP = playerUnit1.maxAP;
        playerUnit2.currentAP = playerUnit2.maxAP;
        playerUnit3.currentAP = playerUnit3.maxAP;
        //Gets our UI text info
        P1Text.text = playerUnit1.unitName;
        P2Text.text = playerUnit2.unitName;
        P3Text.text = playerUnit3.unitName;
        //adds icons 1
        /**
        if(playerUnit1.maxAP >= 4)
        {
            apInd1.icon4.SetActive(true);
        }
        if(playerUnit1.maxAP >= 5)
        {
            apInd1.icon5.SetActive(true);
        }
        if(playerUnit1.maxAP >= 6)
        {
            apInd1.icon6.SetActive(true);
        }
        if(playerUnit1.maxAP == 7)
        {
            apInd1.icon7.SetActive(true);
        }
        //adds icons 2
        if(playerUnit2.maxAP >= 4)
        {
            apInd2.icon4.SetActive(true);
        }
        if(playerUnit2.maxAP >= 5)
        {
            apInd2.icon5.SetActive(true);
        }
        if(playerUnit2.maxAP >= 6)
        {
            apInd2.icon6.SetActive(true);
        }
        if(playerUnit2.maxAP == 7)
        {
            apInd2.icon7.SetActive(true);
        }
        //adds icons 3
        if(playerUnit3.maxAP >= 4)
        {
            apInd3.icon4.SetActive(true);
        }
        if(playerUnit3.maxAP >= 5)
        {
            apInd3.icon5.SetActive(true);
        }
        if(playerUnit3.maxAP >= 6)
        {
            apInd3.icon6.SetActive(true);
        }
        if(playerUnit3.maxAP == 7)
        {
            apInd3.icon7.SetActive(true);
        }
 
    }
//These Functions set up certain elements for each player unit's turn
    IEnumerator playerPhase()
    {
        playerMenu1.transform.localScale = new Vector3(18.0f, 18.0f, 0.0f);
        playerMenu2.transform.localScale = new Vector3(18.0f, 18.0f, 0.0f);
        playerMenu3.transform.localScale = new Vector3(18.0f, 18.0f, 0.0f);
        //state = BattleState.ANIMATING;
        Debug.Log("State changed to: " + state);
        turnAnim.SetTrigger("PlayerTurn");
        Debug.Log("Triggerd Player Phase Animation.");
        yield return new WaitForSeconds(1.2f);
        wePlaying = true;
        yield return StartCoroutine(HandlePlayerTurn());
    }

    private IEnumerator HandlePlayerTurn()
    {
        if (playerUnit1.currentHP > 0 && !p1Acted)
        {
            playerTurn1();
            p1Acted = true;
            yield break;
        }
        else if (playerUnit2.currentHP > 0 && !p2Acted)
        {
            playerTurn2();
            p2Acted = true;
            yield break;
        }
        else if (playerUnit3.currentHP > 0 && !p3Acted)
        {
            playerTurn3();
            p3Acted = true;
            yield break;
        }
        else
        {
            Debug.Log("No active player units to take turns.");
        }
    }
    void playerTurn1()
    {
        Debug.Log("Has Player 1 acted?" + p1Acted);
        Debug.Log("Starting Player Character 1's Turn.");        
        state = BattleState.PLAYERTURN1;
        Debug.Log("State changed to: " + state);
        activeUnit = playerUnit1;
        //scaleChange = new Vector3(3.0f, 3.0f, 0.0f);
        activeMenu = playerMenu1;
        //activeMenu.transform.localScale += scaleChange;
        //Debug.Log("Menu Scale UP");
        currentInd = apInd1;
    }
    void playerTurn2()
    {
        Debug.Log("Starting Player Character 2's Turn.");        
        state = BattleState.PLAYERTURN2;
        Debug.Log("State changed to: " + state);
        activeUnit = playerUnit2;
        //scaleChange = new Vector3(3.0f, 3.0f, 0.0f);
        activeMenu = playerMenu2;
        //activeMenu.transform.localScale += scaleChange;
        //Debug.Log("Menu Scale UP");
        currentInd = apInd2;
    }
    void playerTurn3()
    {
        Debug.Log("Starting Player Character 3's Turn.");        
        state = BattleState.PLAYERTURN3;
        Debug.Log("State changed to: " + state);
        activeUnit = playerUnit3;
        //scaleChange = new Vector3(3.0f, 3.0f, 0.0f);
        activeMenu = playerMenu3;
        //activeMenu.transform.localScale += scaleChange;
        //Debug.Log("Menu Scale UP");
        currentInd = apInd3;
    }
    private IEnumerator enemyPhase()
    {
        Debug.Log("Beginning Enemy Phase.");
        //state = BattleState.ANIMATING;
        Debug.Log("State changed to: " + state);
        turnAnim.SetTrigger("EnemyTurn");
        yield return new WaitForSeconds(1.2f);
        wePlaying = false;
        if (enemyUnit.currentHP > 0)
        {
            Debug.Log("Calling enemyTurn(1)");
            enemyTurn1();
        }
        else if (enemyUnit2 != null && enemyUnit2.currentHP > 0)
        {
            Debug.Log("Calling enemyTurn(2)");
            enemyTurn2();
        }
        else if (enemyUnit3 != null && enemyUnit3.currentHP > 0)
        {
            Debug.Log("Calling enemyTurn(3)");
            enemyTurn3();
        }
    }
    void enemyTurn1()
    {
        Debug.Log("Starting Enemy 1's Turn.");
        state = BattleState.ENEMYTURN1;
        Debug.Log("State changed to: " + state);
        activeUnit = enemyUnit;
        activeMenu = null;
        currentInd = null;
        enemyAction();
    }
    void enemyTurn2()
    {
        Debug.Log("Starting Enemy 2's Turn.");
        state = BattleState.ENEMYTURN2;
        Debug.Log("State changed to: " + state);
        activeUnit = enemyUnit;
        activeMenu = null;
        currentInd = null;
        enemyAction();
    }
    void enemyTurn3()
    {
        Debug.Log("Starting Enemy 3's Turn.");
        state = BattleState.ENEMYTURN3;
        Debug.Log("State changed to: " + state);
        activeUnit = enemyUnit;
        activeMenu = null;
        currentInd = null;
        enemyAction();
    }
    void enemyAction()
    {
        activeUnit.TryGetComponent<DefaultEnemyAI>(out DefaultEnemyAI myAI);
        while (activeUnit.currentAP > 0)
        {
            var targetting = myAI.myTurn();
            var searching = true;

            while (searching == true)
            {
                if(targetting == 1 && playerUnit1.currentHP > 0)
                {
                    targetUnit = playerUnit1;
                    searching = false;
                }
                else if(targetting == 2 && playerUnit2.currentHP > 0)
                {
                    targetUnit = playerUnit2;
                    searching = false;
                }
                else if(targetting == 3 && playerUnit3.currentHP > 0)
                {
                    targetUnit = playerUnit3;
                    searching = false;
                }
                else
                {
                    targetting = myAI.myTurn();
                }
            }
            Debug.Log(activeUnit.unitName + " is targetting " + targetUnit.unitName);

            var attacking = myAI.myAttack();

//IS THIS WHERE THE ENEMY DOES DAMAGE???????????????????????????????????????????????????????????????????????????????????????????????????????????
            if (attacking == 3)
            {
                Damage(6,3);
                snd_HeavyE.Play();
            }
            else if (attacking == 2)
            {
                Damage(4,2);
                snd_MediumE.Play();
            }
            else if (attacking == 1)
            {
                Damage(2,1);
                snd_LightE.Play();
            }
        }
        next();
    }
    IEnumerator ResetIcons()
    {
        Debug.Log("Waiting to reset.");
        backToTop();

        yield return null;

    }
    void backToTop()
    {
        Debug.Log("Resetting.");
        p1Acted = false;
        p2Acted = false;
        p3Acted = false;
        playerUnit1.currentAP = playerUnit1.maxAP;
        Animator[] p1Animators = apInd1.GetComponentsInChildren<Animator>();
        foreach (Animator animator in p1Animators)
        {
            animator.SetTrigger("IconReset");
        }
        playerUnit2.currentAP = playerUnit2.maxAP;
        Animator[] p2Animators = apInd2.GetComponentsInChildren<Animator>();
        foreach (Animator animator in p2Animators)
        {
            animator.SetTrigger("IconReset");
        }
        playerUnit3.currentAP = playerUnit3.maxAP;
        Animator[] p3Animators = apInd3.GetComponentsInChildren<Animator>();
        foreach (Animator animator in p3Animators)
        {
            animator.SetTrigger("IconReset");
        }
        enemyUnit.currentAP = enemyUnit.maxAP;
        Debug.Log("Starting playerPhase from backToTop");
        StartCoroutine(playerPhase());
    }
//This function is called at the end of each unit's turn, it moves into the next on the list.
    void next()
    {
       Debug.Log("Next has been called.");
       //if (activeMenu != null)
        //{
            //activeMenu.transform.localScale -= scaleChange;
        //}
        if (state == BattleState.PLAYERTURN1)
        {
            if (playerUnit2.currentHP > 0)
            {
                playerTurn2();
                Debug.Log("Passing to Player Character 2");
            }
            else if (playerUnit3.currentHP > 0)
            {
                playerTurn3();
                Debug.Log("Passing to Player Character 3");
            }
            else
            {
                StartCoroutine(enemyPhase());
                Debug.Log("Passing to Enemy");
            }
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            if (playerUnit3.currentHP > 0)
            {
                playerTurn3();
                Debug.Log("Passing to Player Character 3");
            }
            else
            {
                StartCoroutine(enemyPhase());
                Debug.Log("Passing to Enemy");
            }
        }
        else if (state == BattleState.PLAYERTURN3)
        {
            StartCoroutine(enemyPhase());
            Debug.Log("Passing to Enemy");
        }
        else if (state == BattleState.ENEMYTURN1)
        {
            if (enemyUnit2 != null && enemyUnit2.currentHP > 0)
            {
                enemyTurn2();
            }
            else if (enemyUnit3 != null && enemyUnit3.currentHP > 0)
            {
                enemyTurn3();
            }
            else
            {
                StartCoroutine(ResetIcons());
            }
        }
        else if (state == BattleState.ENEMYTURN2)
        {
            if (enemyUnit3 != null && enemyUnit3.currentHP > 0)
            {
                enemyTurn3();
            }
            else 
            {
                StartCoroutine(ResetIcons());
            }
        }
        else if (state == BattleState.ENEMYTURN3)
        {
            StartCoroutine(ResetIcons());
        }

    }

    void IconChange()
    {

        // Loop through the icons and set triggers based on currentAP
        if (currentInd != null)
        {
            
            for (int i = 0; i < currentInd.icons.Length; i++)
            {
                if (currentInd.icons[i].activeInHierarchy && activeUnit.currentAP <= i)
                {
                    currentInd.icons[i].GetComponent<Animator>().SetTrigger("IconSpent");
                }
            }
        }
    }
//Main Battle Loop Here
    void Update()
    {        
        if(playerUnit1.currentHP <= 0 && playerUnit2.currentHP <= 0 && playerUnit3.currentHP <= 0)
        {
            state = BattleState.LOST;
            Debug.Log("State changed to: " + state);
            GameManager.Instance.endBattle();
        }
        if(enemyUnit2 != null && enemyUnit3 != null)
        {
            if(enemyUnit.currentHP <= 0 && enemyUnit2.currentHP <= 0 && enemyUnit3.currentHP <= 0)
            {
                state = BattleState.WON;
                Debug.Log("State changed to: " + state);
                GameManager.Instance.endBattle();
            }
        }
        else if(enemyUnit3 != null)
        {
            if(enemyUnit.currentHP <= 0 && enemyUnit2.currentHP <= 0)
            {
                state = BattleState.WON;
                Debug.Log("State changed to: " + state);
                GameManager.Instance.endBattle();
            }
        }
        else if(enemyUnit.currentHP <= 0)
        {
            state = BattleState.WON;
            Debug.Log("State changed to: " + state);
            GameManager.Instance.endBattle();
        }
        //This locks player inputs to their turn
        if (state != BattleState.ENEMYTURN1 && state != BattleState.ANIMATING)
        { 
//IMPORTANT!!!!!! I've hardcoded it so that the player auto-targets enemy1. This should be removed when we get multiple enemies in
            targetUnit = enemyUnit;

            //Passes turn to next in order. Currently serves no practical function, but might be important later.
            if (Input.GetButtonDown("Cancel"))
            {
                combo = null;
                Debug.Log("Pass");
                next();
            }
            //Auto-ends turn if out of action points
            if (activeUnit.currentAP <= 0 || activeUnit.currentHP <= 0)
            {
                combo = null;
                next();
            }
            //Light Attack
            if (activeUnit.currentAP >= 1)
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    combo += ("L");
                    Debug.Log(combo);
                    if (state == BattleState.PLAYERTURN1)
                    {                        
                        if (playerAnim1 != null)
                        {
                            playerAnim1.SetTrigger("TryLight");
                        }
                        snd_Light.Play();

                    }
                    else if (state == BattleState.PLAYERTURN2)
                    { 
                        if (playerAnim2 != null)
                        {
                            playerAnim2.SetTrigger("TryLightM");
                        }
                        snd_LightM.Play();
                    }
                    else if (state == BattleState.PLAYERTURN3)
                    { 
                        if (playerAnim3 != null)
                        {
                            playerAnim3.SetTrigger("TryLightP");
                        }
                        snd_LightP.Play();
                    }
                    Damage(2,1);
                }     
            } 
            //Medium Attack
            if (activeUnit.currentAP >= 2)
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    combo += ("M");
                    Debug.Log(combo);
                    if (state == BattleState.PLAYERTURN1)
                    { 
                        if (playerAnim1 != null)
                        {
                            playerAnim1.SetTrigger("TryMedium");
                        }
                        snd_Medium.Play();
                    }
                    else if (state == BattleState.PLAYERTURN2)
                    { 
                        if (playerAnim2 != null)
                        {
                            playerAnim2.SetTrigger("TryMediumM");
                        }
                        snd_MediumM.Play();
                    }
                    else if (state == BattleState.PLAYERTURN3)
                    { 
                        if (playerAnim3 != null)
                        {
                            playerAnim3.SetTrigger("TryMediumP");
                        }
                        snd_MediumP.Play();
                    }
                    Damage(4,2);
                }     
            } 
            //Heavy Attack
            if (activeUnit.currentAP >= 3)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    combo += ("H");
                    Debug.Log(combo);
                    if (state == BattleState.PLAYERTURN1)
                    { 
                        if (playerAnim1 != null)
                        {
                            playerAnim1.SetTrigger("TryHeavy");
                        }
                        snd_Heavy.Play();
                    }
                    else if (state == BattleState.PLAYERTURN2)
                    { 
                        if (playerAnim2 != null)
                        {
                            playerAnim2.SetTrigger("TryHeavyM");
                        }
                        snd_HeavyM.Play();
                    }
                    else if (state == BattleState.PLAYERTURN3)
                    { 
                        if (playerAnim3 != null)
                        {
                            playerAnim3.SetTrigger("TryHeavyP");
                        }
                        snd_HeavyP.Play();
                    }
                    Damage(6,3);
                }     
            }  
        }
        //Updated the UI
        P1HP.text = playerUnit1.currentHP.ToString() + " HP";
        P2HP.text = playerUnit2.currentHP.ToString() + " HP";
        P3HP.text = playerUnit3.currentHP.ToString() + " HP";

        enemyHPBar1.value = enemyUnit.currentHP;
        
        playerHPBar1.value = playerUnit1.currentHP;
        playerHPBar2.value = playerUnit2.currentHP;
        playerHPBar3.value = playerUnit3.currentHP;
    }
    
    //The part that does the damage dealing.
    void Damage(int rawDamage, int apCost)
    {
        //Looks to see if unity has a combo dictionary attatched, then tries to check the current combo against the dictionary.
        if (activeUnit.TryGetComponent<FighterCombos>(out FighterCombos fdict))
        {
            fdict.ExecuteCombo(combo);
        }
        else if (activeUnit.TryGetComponent<MageCombos>(out MageCombos mdict))
        {
            mdict.ExecuteCombo(combo);
        }
        else if (activeUnit.TryGetComponent<TankCombos>(out TankCombos tdict))
        {
            tdict.ExecuteCombo(combo);
        }
        //Damage is calculated. Currently takes:
            //The average of the Attack's Base Power (rawDamage) and the unit's Strength stat.
                //Big, one-time buffs like the Witch's Overcharge will apply here rather than at the end (rawIncrease).
            //The Squareroot of the target's Defense stat is subtracted from this number. 
                //sqrt is used here so that def isn't suuuper overwhelming.
            //This is all multiplied by the user's Attack Multiplier, which ranges from 0.2 to 1.8.
                //Target defense multipliers will function similar and go here as well.
        var trueDamage = Mathf.Round(activeUnit.attackMultiplier*(((activeUnit.rawIncrease*(float)rawDamage) + activeUnit.Strength)/2) - Mathf.Sqrt(targetUnit.Defense));
        if (trueDamage < 0)
        {
            //In case you ever deal less than 1 damage.
            trueDamage = 0;
        }
        //Applies and resets.
        if (targetUnit.currentHP - (int)trueDamage < 0)
        {
            targetUnit.currentHP = 0;
        }
        else
        {
            targetUnit.currentHP -= (int)trueDamage;
        }
        Debug.Log(trueDamage+"Damage Dealt!");
        activeUnit.currentAP -= apCost;
        activeUnit.rawIncrease = 1.0f;
        if (wePlaying == true)
        {
            IconChange();
        }
        else
        {

        }
    }
}
**/