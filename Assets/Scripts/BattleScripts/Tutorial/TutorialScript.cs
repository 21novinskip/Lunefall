using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    public Dialogue dialogue;
    private Vector3 scaleChange;
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
    public Image[] markers;
    private GameObject activeIndicator;
    public GameObject[] faceBallObjects;
    private GameObject myFace;
    public GameObject AudioManager;
    public GameObject dialogueBox;
[Header("Animators.")]
    private Animator[] playerAnimator;
    private Animator[] enemyAnimator;
    private Animator activeAnimator;
    private Animator turnAnim;
    public GameObject critAnimator;
    public GameObject missAnimator;
[Header("Please dont touch (probably ok if you touch it tho)")]
    public BattleState state;
    public BattleState lastState;
    public String combo;
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
     
    public bool isDialogueActive = false;
 
    public float typingSpeed = 0.2f;
    void Start()
    {
    //Spawns player characters
        int playerSpawnCount = (playerPrefab.Length);
        newPlayer = new GameObject[playerSpawnCount];
        playerAnimator = new Animator[playerSpawnCount];
        turnAnim = GetComponent<Animator>();
    //Instantiates players into battle 
        for (int i = 0; i < playerSpawnCount; i++)
        {
            newPlayer[i] = Instantiate(playerPrefab[i], playerSpawnPoint[i].position, playerSpawnPoint[i].rotation);
    //Assigns statsheet to variable playerUnit[0], playerUnit[1], playerUnit[2] 
            playerUnit[i] = newPlayer[i].GetComponent<Unit>();
            faceBallObjects[i].GetComponent<Image>().sprite = playerUnit[i].neutralPortrait;
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
            Debug.Log(playerUnit[i].unitName + " spawned as " + playerPrefab[i].name + "!");
        }
    //Spawns enemy Characters
        int enemySpawnCount = Mathf.Min(enemyPrefab.Length, enemySpawnPoint.Length);
        newEnemy = new GameObject[enemySpawnCount];
        markers = new Image[enemySpawnCount];
        enemyAnimator = new Animator[enemySpawnCount];
        for (int i = 0; i < enemySpawnCount; i++)
        {
                newEnemy[i] = Instantiate(enemyPrefab[i], enemySpawnPoint[i].position, enemySpawnPoint[i].rotation);
            //Assigns statsheet to variable enemyUnit[0], enemyUnit[1], enemyUnit[2]
                enemyUnit[i] = newEnemy[i].GetComponent<Unit>();
                enemyHPBar[i] = newEnemy[i].GetComponentInChildren<Slider>();
                markers[i] = newEnemy[i].GetComponentInChildren<Image>();
//Boar King audio 

            //Assigns animator to variable enemyAnimator[0], enemyAnimator[1], enemyAnimator[2] 
                enemyAnimator[i] = newEnemy[i].GetComponentInChildren<Animator>();
                enemyUnit[i].maxHP = 30;
                enemyUnit[i].currentHP = 30;
            //Sets enemy's HP to max and sets up HP Bar
                enemyHPBar[i].maxValue = enemyUnit[i].maxHP;
                enemyHPBar[i].value = enemyUnit[i].currentHP;
                enemyUnit[i].currentHP = enemyUnit[i].maxHP;
            //Gives enemy object(s) relevant names and tags
                enemyUnit[i].name = "enemyUnit" + (i);
                newEnemy[i].name = "Enemy_" + (i);
                newEnemy[i].tag = "enemy" + (i);
                Debug.Log(enemyUnit[i].unitName + " spawned as " + enemyPrefab[i].name + "!");
        }
        playerUnit[1].currentHP = 1;
        checkHP();
        dialogueBox.SetActive(false);
        StartCoroutine(Tutorial());
    //since all of our setup is done, we can move into the battle.
    }

    IEnumerator Tutorial()
    {
        turnAnim.SetTrigger("PlayerTurn");
        yield return new WaitForSeconds(1.5F);
        dialogueBox.SetActive(true);
        characterName.text = "Hildegard";
        characterIcon.sprite = playerUnit[2].neutralPortrait;
        dialogueArea.text = "Karlot, this is what you’ve been training for. Attack quickly with 'J'";
        var inputGotten = false; //Sets the player not having input anything.

        activeUnit = playerUnit[0];
        activeMenu = playerMenu[0];
        activeAnimator = playerAnimator[0];
        activeIndicator = apIndicator[0];

        targetUnit = enemyUnit[0];

        scaleChange = new Vector3(0.16f, 0.16f, 0.0f);
        activeMenu.transform.localScale += scaleChange;
        var poschange = new Vector3(38f, 0f , 0f);
        activeMenu.transform.localPosition += poschange; 
        while (inputGotten == false) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Fire3"))
            {
                inputGotten = true;
                activeUnit.currentAP -= 1;
                activeAnimator.SetTrigger("TryLight");
                activeUnit.snd_Light.Play();
                combo += ("L");//Adds this to our combo, relevant for later in the deal damage part.
                DealDamage(2);
                yield return new WaitForSeconds(0.1f);//short rest, then we break
                break;
            }
            yield return null;
        }
        characterName.text = "Catalina";
        characterIcon.sprite = playerUnit[1].neutralPortrait;
        dialogueArea.text = "Come on, you can do more Actions than that! Hit 'J' two more times and let ‘em have it!";
        inputGotten = false;
        while (inputGotten == false) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Fire3"))
            {
                inputGotten = true;
                activeUnit.currentAP -= 1;
                activeAnimator.SetTrigger("TryLight");
                activeUnit.snd_Light.Play();
                combo += ("L");//Adds this to our combo, relevant for later in the deal damage part.
                DealDamage(2);
                yield return new WaitForSeconds(0.1f);//short rest, then we break
                break;
            }
            yield return null;
        }
        inputGotten = false;
        while (inputGotten == false) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Fire3"))
            {
                inputGotten = true;
                activeUnit.currentAP -= 1;
                activeAnimator.SetTrigger("TryLight");
                activeUnit.snd_Light.Play();
                combo += ("L");//Adds this to our combo, relevant for later in the deal damage part.
                DealDamage(2);
                yield return new WaitForSeconds(0.1f);//short rest, then we break
                break;
            }
            yield return null;
        }
        playerUnit[0].agilityMultiplier += 0.2f;
        playerUnit[1].agilityMultiplier += 0.2f;
        playerUnit[2].agilityMultiplier += 0.2f;
        ChangeBuff("agi");
        characterName.text = "Hildegard";
        characterIcon.sprite = playerUnit[2].neutralPortrait;
        dialogueArea.text = "Well done. The magic imbued in your three light attacks has improved our agility.";
        activeMenu.transform.localScale -= scaleChange;
        activeMenu.transform.localPosition -= poschange;

        activeUnit = playerUnit[1];
        activeMenu = playerMenu[1];
        activeAnimator = playerAnimator[1];
        activeIndicator = apIndicator[1];
        combo = null;
        yield return new WaitForSeconds(5.5f);

        activeMenu.transform.localScale += scaleChange;
        activeMenu.transform.localPosition += poschange; 

        characterName.text = "Catalina";
        characterIcon.sprite = playerUnit[1].neutralPortrait;
        dialogueArea.text = "Ack, I'm feeling faint! I need to use my Actions and heal with 'H'.";
        inputGotten = false;
        while (inputGotten == false) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Cancel"))
            {
                inputGotten = true;
                activeUnit.currentAP -= 3;
                activeUnit.currentHP = activeUnit.maxHP;
                StartCoroutine(ChangeIcon()); //needs to be called whenever AP changes
                checkHP(); //needs to be called whenever HP changes
                yield return new WaitForSeconds(0.1f);//short rest, then we break
                break;
            }
            yield return null;
        }
        activeMenu.transform.localScale -= scaleChange;
        activeMenu.transform.localPosition -= poschange;
        characterName.text = "Hildegard";
        characterIcon.sprite = playerUnit[2].neutralPortrait;
        dialogueArea.text = "Good idea. I know you prefer slinging your spells, but do remember you can end your turn and heal at any time.";
        activeUnit = playerUnit[2];
        activeMenu = playerMenu[2];
        activeAnimator = playerAnimator[2];
        activeIndicator = apIndicator[2];
        yield return new WaitForSeconds(5.5f);

        activeMenu.transform.localScale += scaleChange;
        activeMenu.transform.localPosition += poschange; 
        characterName.text = "Catalina";
        characterIcon.sprite = playerUnit[1].neutralPortrait;
        dialogueArea.text = "Yes, yes, I know I’m rusty but I didn’t forget everything. Now then, Hildegard, give that beast a smack with 'J' for me if you don’t mind.";
        inputGotten = false;
        while (inputGotten == false) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Fire3"))
            {
                inputGotten = true;
                activeUnit.currentAP -= 1;
                activeAnimator.SetTrigger("TryLight");
                activeUnit.snd_Light.Play();
                DealDamage(2);
                yield return new WaitForSeconds(0.1f);//short rest, then we break
                break;
            }
            yield return null;
        }
        characterName.text = "Karlot";
        characterIcon.sprite = playerUnit[0].neutralPortrait;
        dialogueArea.text = "Maybe you should use your two remaining Action Points on a stronger attack with 'K'?";
        inputGotten = false;
        while (inputGotten == false) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Fire2"))
            {
                inputGotten = true;
                activeUnit.currentAP -= 2;
                activeAnimator.SetTrigger("TryMedium");
                activeUnit.snd_Medium.Play();
                DealDamage(4);
                yield return new WaitForSeconds(0.1f);//short rest, then we break
                break;
            }
            yield return null;
        }
        yield return null;
        playerUnit[0].defenseMultiplier += 0.2f;
        playerUnit[1].defenseMultiplier += 0.2f;
        playerUnit[2].defenseMultiplier += 0.2f;
        ChangeBuff("def");
        activeMenu.transform.localScale -= scaleChange;
        activeMenu.transform.localPosition -= poschange;
        characterName.text = "Karlot";
        characterIcon.sprite = playerUnit[0].neutralPortrait;
        dialogueArea.text = "Your light and medium attacks together activated a spell to increase our defense.";
        activeUnit = playerUnit[0];
        activeMenu = playerMenu[0];
        activeAnimator = playerAnimator[0];
        activeIndicator = apIndicator[0];
        yield return new WaitForSeconds(5.5f);

        activeMenu.transform.localScale += scaleChange;
        activeMenu.transform.localPosition += poschange; 
        playerUnit[0].currentAP = 3;
        playerUnit[1].currentAP = 3;
        playerUnit[2].currentAP = 3;
        for (int i = 0; i < playerPrefab.Length; i++)
        {
            Animator[] APanimators = apIndicator[i].GetComponentsInChildren<Animator>();
            foreach (Animator anim in APanimators)
            {
                if (anim != null && anim.gameObject.activeInHierarchy)
                {
                    anim.SetTrigger("IconReset");
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        characterName.text = "Catalina";
        characterIcon.sprite = playerUnit[1].neutralPortrait;
        dialogueArea.text = "Indeed, it seems we should keep trying different attack combinations to see what might come of them. Now let's finish this battle!";
        inputGotten = false;
        while (inputGotten == false) //Starts a while loop that waits for the player to press something.
        {
            if (Input.GetButtonDown("Fire1")||Input.GetButtonDown("Fire2")||Input.GetButtonDown("Fire3"))
            {
                inputGotten = true;
                activeAnimator.SetTrigger("TryMedium");
                activeUnit.snd_Medium.Play();
                yield return new WaitForSeconds(1.2f);//short rest, then we break
                DealDamage(6);
                yield return new WaitForSeconds(0.1f);//short rest, then we break
                break;
            }
            yield return null;
        }
        GameManager.Instance.tutorialFinished = true;
        GameManager.Instance.tutorialJUSTFinished = true;
        SceneManager.LoadScene("StartVillage");
    }
    void DealDamage(int rawDamage)
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
        //Applies and resets.
        if (targetUnit.currentHP - (int)trueDamage < 0)
        {
            targetUnit.currentHP = 0;//prevents overkill
        }
        else
        {
            targetUnit.currentHP -= (int)trueDamage;//applies damage
        }
        Debug.Log(trueDamage+"Damage Dealt!");
        Debug.Log("Attack x" + playerUnit[0].attackMultiplier);

        activeUnit.rawIncrease = 1.0f; //resets any one-time damage changes
        checkHP();//sees if we need to change hp bars

        StartCoroutine(ChangeIcon());//changes AP icons

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
                    faceBallObjects[i].GetComponent<Image>().sprite = playerUnit[i].deadPortrait;
                }
                playerUnit[i].isDead = true;//a player who isDead is locked out of any actions and is counted towards the check of game over

            }
            //puts them back to their default portraits if revived
            else if (playerUnit[i].currentHP >= 0 && faceBallObjects[i].GetComponent<Image>().sprite == playerUnit[i].deadPortrait)
            {
                faceBallObjects[i].GetComponent<Image>().sprite = playerUnit[i].neutralPortrait;
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
}