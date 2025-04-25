using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [System.Serializable]
    public class SavedBox
    {
        public float savedX;
        public float savedY;
        public float savedZ;
    }

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
    public GameObject karlotGear;
    [Header("Catalina Stats")]
    public GameObject catalinaBattlePrefab;
    public int p1HP;
    public int p1MaxHP;
    public int p1AP;
    public int p1STR;
    public int p1AGI;
    public int p1LCK;
    public int p1DEF;
    public GameObject catalinaGear;
    [Header("Hildegard Stats")]
    public GameObject hildegardBattlePrefab;
    public int p2HP;
    public int p2MaxHP;
    public int p2AP;
    public int p2STR;
    public int p2AGI;
    public int p2LCK;
    public int p2DEF;
    public GameObject hildegardGear;

    public GameObject player1Overworld;
    public GameObject player2Overworld;
    public GameObject player3Overworld;

    public Vector3 p1Position = new Vector3(0, 0, 0);
    //Enemy 1:
    public Dictionary<string, bool> enemyStates = new Dictionary<string, bool>();
    public GameObject[] player_prefab;
    public GameObject[] enemy_prefab;
    public string overworldEnemyID;
    public bool overworldEnemyKilled;

    [Header("Battlemusic. Don't put anything here.")]
    public AudioClip battleMusic;
    [Header("Overworld music. Put stuff here.")]
    public AudioClip titleMusic;
    public AudioClip startVillageMusic;
    public AudioClip forestMusic;
    public AudioClip graveyardMusic;
    public AudioClip tutorialMusic;

    public string previousScene;
    public string previousSceneOW;
    public int expTally;
    //public SceneTransition scenTr;

    public Image transImg;
    [Header("Pause Button stuff")]
    public GameObject pauseBackground;
    public GameObject pauseText;

    public GameObject sceneManagerObject;
    public SavedBox[] savedLocations;

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

                Color pCola = pauseBackground.GetComponent<Image>().color;
                pCola.a = 1;
                pauseBackground.GetComponent<Image>().color = pCola;
            }
            else
            {
                GameState = LastState;
                pause_screen.SetActive(false);

                Color pColb = pauseBackground.GetComponent<Image>().color;
                pColb.a = 0.4f;
                pauseBackground.GetComponent<Image>().color = pColb;
            }
        }
        else if (GameState != GAMESTATES.PAUSE && pause_screen.activeInHierarchy == true)
        {
            pause_screen.SetActive(false);
        }
    }
    IEnumerator FadeIn(Scene thisScene)
    {
        if (thisScene.name == "MainMenu" || thisScene.name == "cutScene")
        {
            transImg.gameObject.SetActive(false);
        }
        else 
        {
            while (transImg.color.a > 0)
            {
                Color newColor = transImg.color;
                newColor.a -= 0.005f;
                transImg.color = newColor;
                yield return null;
            }
            yield return null;
            transImg.gameObject.SetActive(false);
        }
    }
    //triggers whenever a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //puts the boxes in the right spots
        if ((scene.name != "BattleScene" && scene.name != "BattleTutorial") && savedLocations.Length > 0)
        {
            var sce_man = GameObject.FindWithTag("SceneManager").GetComponent<SceneTransformDetails>();
            for (int i = 0; i < savedLocations.Length; i++)
            {
                sce_man.sceneBoxes[i].boxObj.transform.position = new Vector3(
                    savedLocations[i].savedX,
                    savedLocations[i].savedY,
                    savedLocations[i].savedZ
                );
            }
            savedLocations = new SavedBox[0];
        }
        //Fade In
        if (!transImg.gameObject.activeInHierarchy)
        {
            transImg.gameObject.SetActive(true);
        }
        Color originalColor = transImg.color;
        originalColor.a = 1;
        transImg.color = originalColor;
        StartCoroutine(FadeIn(scene));
        
        GameState = GAMESTATES.ROAM;
        //StartCoroutine(scenTr.EnteringTransition());
        //calls specific setup function based on which scene is loaded
        if (scene.name == "MainMenu" || scene.name == "cutScene" || scene.name == "BattleTutorial" || scene.name == "endScene")
        {
            pauseBackground.SetActive(false);
            pauseText.SetActive(false);
        }
        else 
        {
            sceneManagerObject = GameObject.FindWithTag("SceneManager");
            pauseBackground.SetActive(true);
            pauseText.SetActive(true);
        }
        
        if (scene.name == "StartVillage")
        {
            StartVillageSetup();
        }
        if (scene.name == "Forest")
        {
            StartForestSetup();
        }
        if (scene.name == "GraveyardMT")
        {
            StartGraveyardSetup();
        }
        if (scene.name == "BossRoom")
        {
            StartBossSetup();
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
            Instantiate(player1Overworld, new Vector3(-14,9f,0), Quaternion.identity);//change these vector3 numbers to change spawnpoints
            Instantiate(player2Overworld, new Vector3(-18,9f,0), Quaternion.identity);
            Instantiate(player3Overworld, new Vector3(-16,9f,0), Quaternion.identity);
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

    void StartGraveyardSetup()
    {
        //this is where we set the "default spawn location" of players in a scene. 
        if (previousSceneOW == "Forest")
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
    void StartBossSetup()
    {
        //this is where we set the "default spawn location" of players in a scene. 
        if (previousSceneOW == "GraveyardMT")
        {
            Instantiate(player1Overworld, new Vector3(-12, 14, 0), Quaternion.identity);
            Instantiate(player2Overworld, new Vector3(-13, 14, 0), Quaternion.identity);
            Instantiate(player3Overworld, new Vector3(-14, 14, 0), Quaternion.identity);
            previousSceneOW = null;
        }
        
        else
        {
            Instantiate(player1Overworld, new Vector3(-6, 48, 0), Quaternion.identity);
            Instantiate(player2Overworld, new Vector3(-7, 48, 0), Quaternion.identity);
            Instantiate(player3Overworld, new Vector3(-8, 48, 0), Quaternion.identity);
        }

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
        //playerInventory = null;
        //player_prefab[0] = null;
        //player_prefab[1] = null;
        //player_prefab[2] = null;

        overworldEnemyKilled = false;
        //AudioManager.GetComponent<MusicManager>().Situation = 0;
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
        pauseText.GetComponent<Animator>().SetTrigger("lvlup");
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
