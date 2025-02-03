using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum BattleState{PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, ENEMYTURN1, ENEMYTURN2, ENEMYTURN3, WON, LOST, PLAYERPHASE, ENEMYPHASE, RESET}

public class BattleSystem : MonoBehaviour
{
    private Vector3 scaleChange;
    public static BattleSystem Instance;
[Header("The character prefabs. These will be fed from the GameManager so best to leave them empty.")]
    public GameObject[] playerPrefab;
    public GameObject[] enemyPrefab;
    private GameObject[] newPlayer;
    private GameObject[] newEnemy;
    public Unit[] playerUnit;
    public Unit[] enemyUnit;
    private Unit activeUnit;
    private Unit targetUnit;
[Header("Points the characters will spawn. Always have 3 in the playerSpawnPoint.")]
    public Transform[] playerSpawnPoint;
    public Transform[] enemySpawnPoint;
[Header("UI Thingies")]
    public GameObject[] playerMenu;
    public Slider[] playerHPBar;
    private GameObject activeMenu;
    public TMP_Text[] playerNameText;
    public GameObject[] apIndicator;
    public GameObject[] enemyMenu;
    public Slider[] enemyHPBar;
    public SpriteRenderer[] markers;
    private GameObject activeIndicator;
    public GameObject[] faceBallObjects;
    private GameObject myFace;
    public GameObject AudioManager;
[Header("Animators.")]
    private Animator[] playerAnimator;
    private Animator[] enemyAnimator;
    private Animator activeAnimator;
    private Animator turnAnim;
    public GameObject critAnimator;
    public GameObject missAnimator;
    public Animator CAManimator;
    //delete this later
    public bool special_attack = false;

[Header("Please dont touch (probably ok if you touch it tho)")]
    public BattleState state;
    public BattleState lastState;
    private bool wePlaying = false;
    private String combo;
    public int totalEXP;
[Header("EndScreen")]
    public Slider p0StrengthSlider;
    public Slider p0AgilitySlider;
    public Slider p0LuckSlider;
    public Slider p0DefenseSlider;

    public Slider p1StrengthSlider;
    public Slider p1AgilitySlider;
    public Slider p1LuckSlider;
    public Slider p1DefenseSlider;

    public Slider p2StrengthSlider;
    public Slider p2AgilitySlider;
    public Slider p2LuckSlider;
    public Slider p2DefenseSlider;

    public GameObject levelUpScreen;

    public bool CombatUpdates = false;
    void Start()
    {
        CAManimator = Camera.main.GetComponent<Animator>();

    //Spawns player characters
        int playerSpawnCount = (playerPrefab.Length);
        newPlayer = new GameObject[playerSpawnCount];
        playerAnimator = new Animator[playerSpawnCount];
        turnAnim = GetComponent<Animator>();
    //Instantiates players into battle 
        for (int i = 0; i < playerSpawnCount; i++)
        {
            playerPrefab[i] = GameManager.Instance.player_prefab[i];
            newPlayer[i] = Instantiate(playerPrefab[i], playerSpawnPoint[i].position, playerSpawnPoint[i].rotation);
    //Assigns statsheet to variable playerUnit[0], playerUnit[1], playerUnit[2] 
            playerUnit[i] = newPlayer[i].GetComponent<Unit>();
        }
        ApplyStats();
        for (int i = 0; i < playerSpawnCount; i++)
        {
            faceBallObjects[i].GetComponent<SpriteRenderer>().sprite = playerUnit[i].neutralPortrait;
    //Assigns animator to variable playerAnimator[0], playerAnimator[1], playerAnimator[2] 
            playerAnimator[i] = newPlayer[i].GetComponentInChildren<Animator>();
            playerHPBar[i].maxValue = playerUnit[i].maxHP;
            playerHPBar[i].value = playerUnit[i].currentHP;
            playerNameText[i].text = playerUnit[i].unitName;
    //Gives player objects relevant names and tags
            newPlayer[i].name = "Player_" + (i);
            newPlayer[i].tag = "player" + (i);
    //Sets up the AP icons
            var apInd = apIndicator[i].GetComponent<APIndicator>();
            //checks what the player's max AP is (up to 7) and activates corosponding icons
            for (int a = 0; a < playerUnit[i].maxAP; a++)
            {
                apInd.icons[a].SetActive(true);
            }
            if (CombatUpdates) {Debug.Log(playerUnit[i].unitName + " spawned as " + playerPrefab[i].name + "!");}
        }
        checkHP();
    //Spawns enemy Characters
        int enemySpawnCount = Mathf.Min(enemyPrefab.Length, enemySpawnPoint.Length);
        newEnemy = new GameObject[enemySpawnCount];
        markers = new SpriteRenderer[enemySpawnCount];
        enemyAnimator = new Animator[enemySpawnCount];
        for (int i = 0; i < enemySpawnCount; i++)
        {
            //extra check to make for multiple enemies
            if (GameManager.Instance.enemy_prefab[i] != null)
            {
                enemyPrefab[i] = GameManager.Instance.enemy_prefab[i];
            }
            if (enemyPrefab[i] != null)
            {
                newEnemy[i] = Instantiate(enemyPrefab[i], enemySpawnPoint[i].position, enemySpawnPoint[i].rotation);
            //Assigns statsheet to variable enemyUnit[0], enemyUnit[1], enemyUnit[2]
                enemyUnit[i] = newEnemy[i].GetComponent<Unit>();
                totalEXP += enemyUnit[i].expOnDeath;
                enemyHPBar[i] = newEnemy[i].GetComponentInChildren<Slider>();
                markers[i] = newEnemy[i].GetComponentInChildren<SpriteRenderer>();
//Boar King audio 
                if (enemyUnit[0].unitName == "BoarKing")
                {
                    AudioManager.GetComponent<MusicManager>().Situation = 2;
                }

            //Assigns animator to variable enemyAnimator[0], enemyAnimator[1], enemyAnimator[2] 
                enemyAnimator[i] = newEnemy[i].GetComponentInChildren<Animator>();
            //Sets enemy's HP to max and sets up HP Bar
                enemyHPBar[i].maxValue = enemyUnit[i].maxHP;
                enemyHPBar[i].value = enemyUnit[i].currentHP;
                enemyUnit[i].currentHP = enemyUnit[i].maxHP;
            //Gives enemy object(s) relevant names and tags
                enemyUnit[i].name = "enemyUnit" + (i);
                newEnemy[i].name = "Enemy_" + (i);
                newEnemy[i].tag = "enemy" + (i);
                if (CombatUpdates) {Debug.Log(enemyUnit[i].unitName + " spawned as " + enemyPrefab[i].name + "!");}
            }
        }
    //since all of our setup is done, we can move into the battle.
        StartCoroutine(PlayerPhase());
    }
//This is played first at the begining of battle, and then at the start of every player turn
//If something related to players needs to be reset every turn, it should go here.
    IEnumerator PlayerPhase()
    {
        targetUnit = enemyUnit[0]; //defaults to targeting the only enemy we have
        state = BattleState.PLAYERPHASE; //changes the battlestate, which is relevant for getting inputs.
        if (CombatUpdates) {Debug.Log("Starting Player Phase.");}
        turnAnim.SetTrigger("PlayerTurn");
        //This for statement resets the spent AP icons for any alive players
        for (int i = 0; i < playerPrefab.Length; i++)
        {
            if (playerUnit[i].isDead == false)
            {
                if (CombatUpdates) {Debug.Log("Reseting " + playerUnit[i].unitName + "'s AP.");}
                playerUnit[i].currentAP = playerUnit[i].maxAP;
                Animator[] APanimators = apIndicator[i].GetComponentsInChildren<Animator>();
                foreach (Animator anim in APanimators)
                {
                    if (anim != null && anim.gameObject.activeInHierarchy)
                    {
                        anim.SetTrigger("IconReset");
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                if (CombatUpdates) {Debug.Log(playerUnit[i].unitName + "is dead.");}
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.2f);
        wePlaying = true;
        //Calls Next(), which will move the turn to the earliest player who is alive.
        Next();
    }
//Related to PC1's turn
    IEnumerator PlayerTurn1()
    {
        if (CombatUpdates) {Debug.Log("Player Turn 1 Started.");}
        //This all stuff that is for only the active player
        state = BattleState.PLAYERTURN1;
        activeUnit = playerUnit[0];
        activeAnimator = playerAnimator[0];
        activeIndicator = apIndicator[0];
        activeMenu = playerMenu[0];
        myFace = faceBallObjects[0];
        //Embiggens the menu
        scaleChange = new Vector3(3.0f, 3.0f, 0.0f);
        activeMenu.transform.localScale += scaleChange;
        var poschange = new Vector3(38f, 0f , 0f);
        activeMenu.transform.localPosition += poschange; 
        //Asks for player input if they have AP left
        while (activeUnit.currentAP > 0)
        { 
            yield return StartCoroutine(PlayerInput());
            if (activeUnit.currentAP <= 0)
            {
                break;
            }
            yield return null;
        }
        Next();
    }
//Related to PC2's turn
    IEnumerator PlayerTurn2()
    {
        if (CombatUpdates) {Debug.Log("Player Turn 2 Started.");}
        //This all stuff that is for only the active player
        state = BattleState.PLAYERTURN2;
        activeUnit = playerUnit[1];
        activeAnimator = playerAnimator[1];
        activeIndicator = apIndicator[1];
        activeMenu = playerMenu[1];
        //Embiggens the menu
        scaleChange = new Vector3(3.0f, 3.0f, 0.0f);
        activeMenu.transform.localScale += scaleChange;
        var poschange = new Vector3(38f, 0f , 0f);
        activeMenu.transform.localPosition += poschange; 
        //Asks for player input if they have AP left
        while (activeUnit.currentAP > 0)
        { 
            yield return StartCoroutine(PlayerInput());
            if (activeUnit.currentAP <= 0)
            {
                break;
            }
            yield return null;
        }
        Next();
    }
//Related to PC3's turn
    IEnumerator PlayerTurn3()
    {
        if (CombatUpdates) {Debug.Log("Player Turn 3 Started.");}
        //This all stuff that is for only the active player
        state = BattleState.PLAYERTURN3;
        activeUnit = playerUnit[2];
        activeAnimator = playerAnimator[2];
        activeIndicator = apIndicator[2];
        activeMenu = playerMenu[2];
        //Embiggens the menu
        scaleChange = new Vector3(3.0f, 3.0f, 0.0f);
        activeMenu.transform.localScale += scaleChange;
        var poschange = new Vector3(38f, 0f , 0f);
        activeMenu.transform.localPosition += poschange; 
        //Asks for player input if they have AP left
        while (activeUnit.currentAP > 0)
        { 
            yield return StartCoroutine(PlayerInput());
            if (activeUnit.currentAP <= 0)
            {
                break;
            }
            yield return null;
        }
        Next();
    }
//This is played at the start of every enemy turn.
//If something related to enemies needs to be reset every turn, it should go here.
    IEnumerator EnemyPhase()
    {
        wePlaying = false;
        if (CombatUpdates) {Debug.Log("Starting Enemy Phase.");}
        state = BattleState.ENEMYPHASE;
        activeIndicator = null;
        turnAnim.SetTrigger("EnemyTurn");
        yield return new WaitForSeconds(1.2f);
        Next();
    }
//Related to EC3's turn
    IEnumerator EnemyTurn1()
    {
        if (CombatUpdates) {Debug.Log("Enemy Turn 1 Started.");}
        state = BattleState.ENEMYTURN1;
        activeUnit = enemyUnit[0];
        activeAnimator = enemyAnimator[0];
        activeUnit.TryGetComponent<DefaultEnemyAI>(out DefaultEnemyAI myAI); //sets AI as myAI
        bool targetPicked = false;
        while (targetPicked == false)
        {
            int t =  myAI.myTurn(); //calls targetting function from myAI
            if (playerUnit[t].isDead == false)//if the target is valid, sselects it. If not, try targetting again
            {
                targetPicked = true;
                targetUnit = playerUnit[t];
                break;
            }
            yield return null;
        }
        activeAnimator.SetTrigger("E_TryLight");

        /*
        // Wait until the animation starts playing
        while (!activeAnimator.GetCurrentAnimatorStateInfo(0).IsName("BoarKing_Attack"))
        {
            yield return null;
        }

        // Wait until the animation finishes playing
        AnimatorStateInfo stateInfo = activeAnimator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.normalizedTime < 1f)
        {
            stateInfo = activeAnimator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        */
    //enemy deals damage here for now.
        activeUnit.snd_Heavy.Play();
        DealDamage(6);
        Next();
        yield return null;


    }
    //These two coroutines don't matter right now. Might not ever at this rate.
    IEnumerator EnemyTurn2()
    {
        if (CombatUpdates) {Debug.Log("Enemy Turn 1 Started.");}
        state = BattleState.ENEMYTURN2;
        activeUnit = enemyUnit[1];
        Next();
        yield return null;
    }
    IEnumerator EnemyTurn3()
    {
        if (CombatUpdates) {Debug.Log("Enemy Turn 1 Started.");}
        state = BattleState.ENEMYTURN3;
        activeUnit = enemyUnit[2];
        Next();
        yield return null;
    }

    IEnumerator PlayerInput()
    {
        var inputGotten = false; //Sets the player not having input anything.
        if (CombatUpdates) {Debug.Log(activeUnit.unitName + "'s AP is currently " + activeUnit.currentAP);}
        if (CombatUpdates) {Debug.Log("Waiting for " + activeUnit.unitName + "'s input.");}
        while (inputGotten == false && wePlaying) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Fire1") && activeUnit.currentAP >= 3)
            {
                inputGotten = true;
                activeUnit.currentAP -= 3;
                if (special_attack == true)
                {
                    activeAnimator.SetBool("isSpecial", true);
                    //CAManimator.SetTrigger("WS1");
                    special_attack = false;
                }
                activeAnimator.SetTrigger("TryHeavy");
                yield return null; // Waits one frame to ensure the animation state updates
                combo += ("H"); //Adds this to our combo, relevant for later in the deal damage part.
                activeUnit.snd_Heavy.Play();
                if (CombatUpdates) {Debug.Log(activeUnit.unitName + " has " + activeUnit.currentAP + " AP left.");}
                float animationLength = activeAnimator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(animationLength);
                DealDamage(6);
                break;
            }
            else if (Input.GetButtonDown("Fire2") && activeUnit.currentAP >= 2)
            {
                inputGotten = true;
                activeUnit.currentAP -= 2;
                if (special_attack == true)
                {
                    activeAnimator.SetBool("isSpecial", true);
                    //CAManimator.SetTrigger("WS1");
                    special_attack = false;
                }
                activeAnimator.SetTrigger("TryMedium");
                yield return null; // Waits one frame to ensure the animation state updates
                combo += ("M");//Adds this to our combo, relevant for later in the deal damage part.
                activeUnit.snd_Medium.Play();
                if (CombatUpdates) {Debug.Log(activeUnit.unitName + " has " + activeUnit.currentAP + " AP left.");}
                float animationLength = activeAnimator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(animationLength);
                DealDamage(4);
                break;
            }
            else if (Input.GetButtonDown("Fire3") && activeUnit.currentAP >= 1)
            {
                inputGotten = true;
                activeUnit.currentAP -= 1;
                if (special_attack == true)
                {
                    activeAnimator.SetBool("isSpecial", true);
                    //CAManimator.SetTrigger("WS1");
                    special_attack = false;
                }
                activeAnimator.SetTrigger("TryLight");
                yield return null; // Waits one frame to ensure the animation state updates
                activeUnit.snd_Light.Play();
                if (CombatUpdates) {Debug.Log(activeUnit.unitName + " has " + activeUnit.currentAP + " AP left.");}
                combo += ("L");//Adds this to our combo, relevant for later in the deal damage part.
                float animationLength = activeAnimator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(animationLength);
                DealDamage(2);
                break;
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                inputGotten = true;
                var healAmount = activeUnit.currentAP;
                activeUnit.currentAP -= activeUnit.currentAP;
                //heals you.
                activeUnit.currentHP += Mathf.Min((activeUnit.maxHP - activeUnit.currentHP),(healAmount * 10));
                StartCoroutine(ChangeIcon()); //needs to be called whenever AP changes
                checkHP(); //needs to be called whenever HP changes
                yield return new WaitForSeconds(0.1f);
                break;
            }
            yield return null;
        }
    }
    void Next()
    {
        //Resets combo string, activeUnit, and menu to a default state.
        combo = null;
        activeUnit = null;
        if (activeMenu != null)
        {
            scaleChange = new Vector3(3.0f, 3.0f, 0.0f);
            activeMenu.transform.localScale -= scaleChange;
            var poschange = new Vector3(38f, 0f , 0f);
            activeMenu.transform.localPosition -= poschange; 
            activeMenu = null;
        }
        switch (state) //Depending on what phase you are currently in, this checks the possible next steps and picks the highest priority
        {
            case BattleState.PLAYERPHASE:
                if (!playerUnit[0].isDead)
                {
                    StartCoroutine(PlayerTurn1());
                }
                else if (!playerUnit[1].isDead)
                {
                    StartCoroutine(PlayerTurn2());
                }
                else if (!playerUnit[2].isDead)
                {
                    StartCoroutine(PlayerTurn3());
                }
                else
                {
                    if (CombatUpdates) {Debug.Log("Everyone player is dead but the battle didn't end. Oh no!");}
                }
                break;
            case BattleState.PLAYERTURN1:
                if (!playerUnit[1].isDead)
                {
                    StartCoroutine(PlayerTurn2());
                }
                else if (!playerUnit[2].isDead)
                {
                    StartCoroutine(PlayerTurn3());
                }
                else
                {
                    StartCoroutine(EnemyPhase());
                }
                break;
            case BattleState.PLAYERTURN2:
                if (!playerUnit[2].isDead)
                {
                    StartCoroutine(PlayerTurn3());
                }
                else
                {
                    StartCoroutine(EnemyPhase());
                }
                break;
            case BattleState.PLAYERTURN3:
                StartCoroutine(EnemyPhase());
                break;
            case BattleState.ENEMYPHASE:
                if (!enemyUnit[0].isDead)
                {
                    StartCoroutine(EnemyTurn1());
                }
                else if (enemyUnit[1] != null && !enemyUnit[1].isDead)
                {
                    StartCoroutine(EnemyTurn2());
                }
                else if (enemyUnit[2] != null && !enemyUnit[2].isDead)
                {
                    StartCoroutine(EnemyTurn3());
                }
                else
                {
                    if (CombatUpdates) {Debug.Log("Everyone enemy is dead but the battle didn't end. Oh no!");}
                }
                break;
            case BattleState.ENEMYTURN1:
                if (enemyUnit[1] != null && !enemyUnit[1].isDead)
                {
                    StartCoroutine(EnemyTurn2());
                }
                else if (enemyUnit[2] != null && !enemyUnit[2].isDead)
                {
                    StartCoroutine(EnemyTurn3());
                }
                else
                {
                    StartCoroutine(PlayerPhase());
                }
                break;
            case BattleState.ENEMYTURN2:
                if (enemyUnit[2] != null && !enemyUnit[2].isDead)
                {
                    StartCoroutine(EnemyTurn3());
                }
                else
                {
                    StartCoroutine(PlayerPhase());
                }
                break;
            case BattleState.ENEMYTURN3:
                StartCoroutine(PlayerPhase());
                break;
        }
    }
    float Hit() //determines the % chance of an attack hitting.
    {
        //Hitrate is-> 
        //90 + (attacker Agility - defender Agility)%
        //Minimum of 5%, maximum of 95%
        float HitRate = (Mathf.Clamp(90 + activeUnit.Agility - targetUnit.Agility , 5, 95))*activeUnit.agilityMultiplier;//multiplies by any speed buffs/debufs
        if (CombatUpdates) {Debug.Log("Hit Rate is " + HitRate + "%");}
        return HitRate;
    }
    float Crit() // detirmines the % chance of an attack critting.
    {
        //Critrate is ->
        //half attacker luck - half defender luck %
        ////Minimum of 5%, maximum of 95%
        float CritRate = (Mathf.Clamp(activeUnit.Luck/2 - targetUnit.Luck/2, 5, 95))*activeUnit.luckMultiplier;//multiplies by any luck buffs/debufs
        if (CombatUpdates) {Debug.Log("Crit Rate is " + CritRate + "%");}
        return CritRate;
    }
    void DealDamage(int rawDamage)
    {
        bool isHit = UnityEngine.Random.Range(0f, 100f) <= Hit(); //check if we hit
        bool isCrit = UnityEngine.Random.Range(0f, 100f) <= Crit(); //check if we crit
        //uses the string combo and each player's combo list to check if anything activates
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
        if (isHit == true)
        {
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
            if (isCrit == true)//doubles damage for crits
            {
                trueDamage *= 2;
                if (CombatUpdates) {Debug.Log("Critical Hit!");}
                critAnimator.GetComponent<Animator>().SetTrigger("Crit");
            }
            //Applies and resets.
            if (targetUnit.currentHP - (int)trueDamage < 0)
            {
                targetUnit.currentHP = 0;//prevents overkill
            }
            else
            {
                targetUnit.currentHP -= (int)trueDamage;//applies damage
            }
            if (CombatUpdates) {Debug.Log(trueDamage+"Damage Dealt!");}
            if (CombatUpdates) {Debug.Log("Attack x" + playerUnit[0].attackMultiplier);}
        }
        else//for things that happen if you miss
        {
            if (CombatUpdates) {Debug.Log("Attack Missed.");}
            missAnimator.GetComponent<Animator>().SetTrigger("Miss");
        }
        activeUnit.rawIncrease = 1.0f; //resets any one-time damage changes
        checkHP();//sees if we need to change hp bars
        if (wePlaying == true)
        {
            StartCoroutine(ChangeIcon());//changes AP icons
        }
        else
        {

        }
    }

    IEnumerator ChangeIcon()
    {
        for (int i = 0; i < activeIndicator.GetComponent<APIndicator>().icons.Length; i++)
        {
            if (activeIndicator.GetComponent<APIndicator>().icons[i].activeInHierarchy && activeUnit.currentAP <= i)
            {
                activeIndicator.GetComponent<APIndicator>().icons[i].GetComponent<Animator>().SetTrigger("IconSpent");
            }  
        }
        yield break;
    }
    //checkHP should be called any time you expect HP to be changed
    public void checkHP()
    {
        for (int i = 0; i < playerHPBar.Length; i++)
        {
            playerHPBar[i].value = playerUnit[i].currentHP;//changes player hpbars to reflect current HP
            if (playerUnit[i].currentHP <= 0)
            {
                if (playerUnit[i].isDead == false)
                {
                    //dead faces
                    playerAnimator[i].SetTrigger("Death");
                    faceBallObjects[i].GetComponent<SpriteRenderer>().sprite = playerUnit[i].deadPortrait;
                }
                playerUnit[i].isDead = true;//a player who isDead is locked out of any actions and is counted towards the check of game over

            }
            //puts them back to their default portraits if revived
            else if (playerUnit[i].currentHP >= 0 && faceBallObjects[i].GetComponent<SpriteRenderer>().sprite == playerUnit[i].deadPortrait)
            {
                faceBallObjects[i].GetComponent<SpriteRenderer>().sprite = playerUnit[i].neutralPortrait;
            }
        }   
    //hp checks for enemies.
        if (enemyUnit[0] != null)
        {
            enemyHPBar[0].value = enemyUnit[0].currentHP;
            if (enemyUnit[0].currentHP <= 0)
            {
                enemyUnit[0].isDead = true;
            }
        }
        if (enemyUnit[1] != null)
        {
            enemyHPBar[1].value = enemyUnit[1].currentHP;
            if (enemyUnit[1].currentHP <= 1)
            {
                enemyUnit[1].isDead = true;
            }
        }
        if (enemyUnit[2] != null)
        {
            enemyHPBar[2].value = enemyUnit[2].currentHP;
            if (enemyUnit[2].currentHP <= 2)
            {
                enemyUnit[2].isDead = true;
            }
        }
    }
    public void ChangeBuff(string stat)// input the stat (atk, def, agi, lck) and this will change the icon for all players to reflect their current buff level.
    {
        //for (int i = 0; i < playerPrefab.Length; i++)
        //{
            switch (stat) //obtuse code for swithching the buff's icon. 
            {
                case "atk":
                    playerMenu[0].GetComponent<menu>().atkIcon.GetComponent<Animator>().SetTrigger(playerUnit[0].attackMultiplier.ToString());
                    playerMenu[1].GetComponent<menu>().atkIcon.GetComponent<Animator>().SetTrigger(playerUnit[1].attackMultiplier.ToString());
                    playerMenu[2].GetComponent<menu>().atkIcon.GetComponent<Animator>().SetTrigger(playerUnit[2].attackMultiplier.ToString());
                    break;
                case "def":
                    playerMenu[0].GetComponent<menu>().defIcon.GetComponent<Animator>().SetTrigger(playerUnit[0].defenseMultiplier.ToString());
                    playerMenu[1].GetComponent<menu>().defIcon.GetComponent<Animator>().SetTrigger(playerUnit[1].defenseMultiplier.ToString());
                    playerMenu[2].GetComponent<menu>().defIcon.GetComponent<Animator>().SetTrigger(playerUnit[2].defenseMultiplier.ToString());
                    break;
                case "agi":
                    playerMenu[0].GetComponent<menu>().agiIcon.GetComponent<Animator>().SetTrigger(playerUnit[0].agilityMultiplier.ToString());
                    playerMenu[1].GetComponent<menu>().agiIcon.GetComponent<Animator>().SetTrigger(playerUnit[1].agilityMultiplier.ToString());
                    playerMenu[2].GetComponent<menu>().agiIcon.GetComponent<Animator>().SetTrigger(playerUnit[2].agilityMultiplier.ToString());
                    break;
                case "lck":
                    playerMenu[0].GetComponent<menu>().lckIcon.GetComponent<Animator>().SetTrigger(playerUnit[0].luckMultiplier.ToString());
                    playerMenu[1].GetComponent<menu>().lckIcon.GetComponent<Animator>().SetTrigger(playerUnit[1].luckMultiplier.ToString());
                    playerMenu[2].GetComponent<menu>().lckIcon.GetComponent<Animator>().SetTrigger(playerUnit[2].luckMultiplier.ToString());
                    break;
                default:
                    break;
            }
            
        //}
    }
    public void leveluptest()
    {
        playerUnit[0].LevelUp();
        playerUnit[1].LevelUp();
        playerUnit[3].LevelUp();
    }
    void Update()
    {
        if (enemyUnit[0].isDead)//brings us to overworld if won
        {
            state = BattleState.WON;
            GameManager.Instance.endBattle();
            //StartCoroutine(EndScreen());
        }
        if (playerUnit[0].isDead && playerUnit[1].isDead && playerUnit[2].isDead)//brings us to title screen if lost
        {
            state = BattleState.LOST;
            GameManager.Instance.endBattle();
        }    
    }
    void ApplyStats()
    {
        playerUnit[0].maxHP = GameManager.Instance.p0MaxHP;
        playerUnit[0].currentHP = GameManager.Instance.p0HP;
        playerUnit[0].maxAP = GameManager.Instance.p0AP;
        playerUnit[0].Strength = GameManager.Instance.p0STR;
        playerUnit[0].Agility = GameManager.Instance.p0AGI;
        playerUnit[0].Luck = GameManager.Instance.p0LCK;
        playerUnit[0].Defense = GameManager.Instance.p0DEF;

        playerUnit[1].maxHP = GameManager.Instance.p1MaxHP;
        playerUnit[1].currentHP = GameManager.Instance.p1HP;
        playerUnit[1].maxAP = GameManager.Instance.p1AP;
        playerUnit[1].Strength = GameManager.Instance.p1STR;
        playerUnit[1].Agility = GameManager.Instance.p1AGI;
        playerUnit[1].Luck = GameManager.Instance.p1LCK;
        playerUnit[1].Defense = GameManager.Instance.p1DEF;

        playerUnit[2].maxHP = GameManager.Instance.p2MaxHP;
        playerUnit[2].currentHP = GameManager.Instance.p2HP;
        playerUnit[2].maxAP = GameManager.Instance.p2AP;
        playerUnit[2].Strength = GameManager.Instance.p2STR;
        playerUnit[2].Agility = GameManager.Instance.p2AGI;
        playerUnit[2].Luck = GameManager.Instance.p2LCK;
        playerUnit[2].Defense = GameManager.Instance.p2DEF;
    }
}