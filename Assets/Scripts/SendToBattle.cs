using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendToBattle : MonoBehaviour
{
    //enemy stats
    public GameObject enemyPFB1;
    public GameObject enemyPFB2;
    public GameObject enemyPFB3;

    public string uniqueID;
    public string thisScene;
    public bool enemyDefeated = false;

    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;
        if (GameManager.Instance.overworldEnemyID == uniqueID)
        {
            enemyDefeated = GameManager.Instance.overworldEnemyKilled;
            if (enemyDefeated == true)
            {
                killme();
                GameManager.Instance.resetThings();
            }
        }
        else if( uniqueID == "boar_village-1" && GameManager.Instance.tutorialFinished == true)
        {
            killme();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colliding with something: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("witchOW") || collision.gameObject.CompareTag("fighterOW") || collision.gameObject.CompareTag("tankOW"))
        {
            if ( uniqueID == "boar_village-1")
            {
                //GameManager.Instance.overworldEnemyID = uniqueID;
                GameManager.Instance.p1Position = collision.gameObject.GetComponent<Transform>().position;
                SceneManager.LoadScene("BattleTutorial");
            }
            else
            {
                //Debug.Log("Colliding with Player");
                // Store the enemy data in the Game Manager (all caps variables are game manager, lower case are fed in)
                GameManager.Instance.enemy_prefab[0] = enemyPFB1;
                if (enemyPFB2 != null)
                {
                    GameManager.Instance.enemy_prefab[1] = enemyPFB2;
                }
                if (enemyPFB3 != null)
                {
                    GameManager.Instance.enemy_prefab[2] = enemyPFB3;
                }
                GameManager.Instance.overworldEnemyID = uniqueID;

                //Store the player data
                GameManager.Instance.previousScene = thisScene;
                
                GameManager.Instance.p1Position = collision.gameObject.GetComponent<Transform>().position;
                //GameManager.Instance.playerInventory = collision.gameObject.GetComponent<PlayerInventory>();
                //GameManager.Instance.player_prefab[0] = collision.gameObject.GetComponent<PlayerStats>().character1Prefab;
                //GameManager.Instance.player_prefab[1] = collision.gameObject.GetComponent<PlayerStats>().character2Prefab;
                //GameManager.Instance.player_prefab[2] = collision.gameObject.GetComponent<PlayerStats>().character3Prefab;

                // Load the battle scene
                SceneManager.LoadScene("BattleScene");
            }
        }
    }
    public void killme()
    {
        gameObject.SetActive(false);
    }
}
