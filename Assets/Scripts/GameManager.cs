using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    //public int gold = 100;
    public GameObject pause_screen;
    public GameObject shop_screen;
    public GAMESTATES GameState;
    private GAMESTATES LastState;
    public enum GAMESTATES
    {
        ROAM,
        PAUSE,
        COMBAT_TRANSITION,
        SHOP,
    }

    public bool tutorialFinished = false;//a flag that the tutorial has been finished
    public bool tutorialJUSTFinished = false;//a flag that you JUST finished the tutorial, a few things need to happen immediately after and never again

    public static GameManager Instance;
    public int partyLevel;
    public int currentEXP;
    public int expToNext;
    [Header("Karlot Stats")]
    public GameObject karlotBattlePrefab;
    public int p0HP;
    public int p0MaxHP;
    public int p0AP;
    public int p0STR;
    public int p0AGI;
    public int p0LCK;
    public int p0DEF;
    [Header("Catalina Stats")]
    public GameObject catalinaBattlePrefab;
    public int p1HP;
    public int p1MaxHP;
    public int p1AP;
    public int p1STR;
    public int p1AGI;
    public int p1LCK;
    public int p1DEF;
    [Header("Hildegard Stats")]
    public GameObject hildegardBattlePrefab;
    public int p2HP;
    public int p2MaxHP;
    public int p2AP;
    public int p2STR;
    public int p2AGI;
    public int p2LCK;
    public int p2DEF;

    public GameObject player1Overworld;
    public GameObject player2Overworld;
    public GameObject player3Overworld;

    public Vector3 p1Position = new Vector3(0, 0, 0);
    //Enemy 1:
    public Dictionary<string, bool> enemyStates = new Dictionary<string, bool>();
    public PlayerInventory playerInventory;
    public GameObject[] player_prefab;
    public GameObject[] enemy_prefab;
    public string overworldEnemyID;
    public bool overworldEnemyKilled;

    public string previousScene;
    public string previousSceneOW;
    public int expTally;

    private void Awake()
    {
        
        //audioSource.PlayOneShot(TownMusic);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartVillage")
        {
            if (Input.GetKeyDown(KeyCode.Backspace)) 
            {
                Debug.Log("go!");
                tutorialFinished = true;
                previousSceneOW = "StartVillage";
                SceneManager.LoadScene("Forest");
            }
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (GameState != GAMESTATES.PAUSE)
            {
                LastState = GameState;
                GameState = GAMESTATES.PAUSE;
                pause_screen.SetActive(true);
                pause_screen.GetComponent<PauseMenuButtons>().ShowKarlotStats();
            }
            else
            {
                GameState = LastState;
                pause_screen.SetActive(false);
            }
        }
        else if (GameState != GAMESTATES.PAUSE && pause_screen.activeInHierarchy == true)
        {
            pause_screen.SetActive(false);
        }
    }
    //triggers whenever a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameState = GAMESTATES.ROAM;
        //calls specific setup function based on which scene is loaded
        if (scene.name == "StartVillage")
        {
            StartVillageSetup();
        }
        if (scene.name == "Forest")
        {
            StartForestSetup();
        }
        //add as many if statements and functions as there are overworld scenes
    }

    void StartVillageSetup()
    {
        if (previousSceneOW == "Forest") //when coming back from forest
        {
            Instantiate(player1Overworld, new Vector3(15,-4,0), Quaternion.identity);//change these vector3 numbers to change spawnpoints
            Instantiate(player2Overworld, new Vector3(15,-4,0), Quaternion.identity);
            Instantiate(player3Overworld, new Vector3(15,-4,0), Quaternion.identity);
        }
        //this is where we set the "default spawn location" of players in a scene. 
        else if (p1Position == new Vector3(0, 0, 0))
        {
            Instantiate(player1Overworld, new Vector3(-14,7.5f,0), Quaternion.identity);//change these vector3 numbers to change spawnpoints
            Instantiate(player2Overworld, new Vector3(-18,7.5f,0), Quaternion.identity);
            Instantiate(player3Overworld, new Vector3(-16,7.5f,0), Quaternion.identity);
        }
        //Spawns p1 at the same spot as they were before battle, p2 & p3 nearby
        else
        {
            Instantiate(player1Overworld, p1Position, Quaternion.identity);
            Instantiate(player2Overworld, p1Position - new Vector3(0,0,0), Quaternion.identity);
            Instantiate(player3Overworld, p1Position - new Vector3(0,0,0), Quaternion.identity);
        }
        if (tutorialJUSTFinished == true)//specific to this scene
        {
            resetThings();
            tutorialJUSTFinished = false;
        }
        previousSceneOW = null;//set this back to nothing so it doesn't mess things up.
    }

    void StartForestSetup()
    {
        //this is where we set the "default spawn location" of players in a scene. 
        if (previousSceneOW == "StartVillage")
        {
            Instantiate(player1Overworld, new Vector3(-18,13.5f,0), Quaternion.identity);//change these vector3 numbers to change spawnpoints
            Instantiate(player2Overworld, new Vector3(-22,13.5f,0), Quaternion.identity);
            Instantiate(player3Overworld, new Vector3(-20,13.5f,0), Quaternion.identity);
        }
        //Spawns p1 at the same spot as they were before battle, p2 & p3 nearby
        else
        {
            Instantiate(player1Overworld, p1Position, Quaternion.identity);
            Instantiate(player2Overworld, p1Position - new Vector3(0,0,0), Quaternion.identity);
            Instantiate(player3Overworld, p1Position - new Vector3(0,0,0), Quaternion.identity);
        }
        previousSceneOW = null;//set this back to nothing so it doesn't mess things up.
    }


    public void enterShop()
    {
        if (GameState != GAMESTATES.SHOP)
            {
                LastState = GameState;
                GameState = GAMESTATES.SHOP;
                shop_screen.SetActive(true);
            }
            else
            {
                GameState = LastState;
                shop_screen.SetActive(false);
            }
    }
    public void exitShop()
    {
        if (GameState == GAMESTATES.SHOP)
            {
                LastState = GameState;
                GameState = GAMESTATES.ROAM;
                shop_screen.SetActive(false);
            }

    }
    public void resetThings()
    {
        p1Position = new Vector3(0,0,0);
        previousScene = null;
        enemy_prefab[0] = null;
        overworldEnemyID = null;
        playerInventory = null;
        player_prefab[0] = null;
        player_prefab[1] = null;
        player_prefab[2] = null;

        overworldEnemyKilled = false;
    }
    public void endBattle()
    {
        //player_prefab1 = BattleSystem.Instance.playerPrefab1;
        //player_prefab2 = BattleSystem.Instance.playerPrefab2;
        //player_prefab3 = BattleSystem.Instance.playerPrefab3;

        var bsys = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        if (bsys.state == BattleState.WON)
        {
            overworldEnemyKilled = true;
            currentEXP += bsys.totalEXP;
            Debug.Log(bsys.totalEXP + " EXP earned.");
            Debug.Log("Total EXP is " + currentEXP);
            if (currentEXP >= 100)
            {
                currentEXP -= 100;
                bsys.playerUnit[0].LevelUp();
                bsys.playerUnit[1].LevelUp();
                bsys.playerUnit[2].LevelUp();
                ApplyLevel();
            }
            else 
            {
            }
            SceneManager.LoadScene(previousScene);
        }
        else if (bsys.state == BattleState.LOST)
        {
            SceneManager.LoadScene("MainMenu");
            resetThings();
        }
    }
    public void ApplyLevel()
    {
        var bsys = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        partyLevel += 1;
        p0MaxHP = bsys.playerUnit[0].maxHP;
        p0HP = bsys.playerUnit[0].currentHP;
        p0AP = bsys.playerUnit[0].maxAP;
        p0STR = bsys.playerUnit[0].Strength;
        p0AGI = bsys.playerUnit[0].Agility;
        p0LCK = bsys.playerUnit[0].Luck;
        p0DEF = bsys.playerUnit[0].Defense;

        p1MaxHP = bsys.playerUnit[1].maxHP;
        p1HP = bsys.playerUnit[1].currentHP;
        p1AP = bsys.playerUnit[1].maxAP;
        p1STR = bsys.playerUnit[1].Strength;
        p1AGI = bsys.playerUnit[1].Agility;
        p1LCK = bsys.playerUnit[1].Luck;
        p1DEF = bsys.playerUnit[1].Defense;

        p2MaxHP = bsys.playerUnit[2].maxHP;
        p2HP = bsys.playerUnit[2].currentHP;
        p2AP = bsys.playerUnit[2].maxAP;
        p2STR = bsys.playerUnit[2].Strength;
        p2AGI = bsys.playerUnit[2].Agility;
        p2LCK = bsys.playerUnit[2].Luck;
        p2DEF = bsys.playerUnit[2].Defense;
    }
}
