using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //STATS
    [Header("Character 1 Stats")]
    public GameObject character1Prefab;
    public string c1_unit_name;
    public int c1_unit_level;
    public int c1_max_hp;
    public int c1_current_hp;
    public int c1_max_ap;
    public int c1_current_ap;
    public int c1_strength;
    public int c1_speed;
    public int c1_luck;
    public int c1_defense;

    [Header("Character 2 Stats")]
    public GameObject character2Prefab;
    public string c2_unit_name;
    public int c2_unit_level;
    public int c2_max_hp;
    public int c2_current_hp;
    public int c2_max_ap;
    public int c2_current_ap;
    public int c2_strength;
    public int c2_speed;
    public int c2_luck;
    public int c2_defense;

    [Header("Character 3 Stats")]
    public GameObject character3Prefab;
    public string c3_unit_name;
    public int c3_unit_level;
    public int c3_max_hp;
    public int c3_current_hp;
    public int c3_max_ap;
    public int c3_current_ap;
    public int c3_strength;
    public int c3_speed;
    public int c3_luck;
    public int c3_defense;

    void Start()
    {
    //Looks for the Unit script in each Player Character and assigns those stat values to these variables.

    //By having the code do this here instead of writing down the stats in these slots, 
    //we keep the door open to let the player switch who acts first in battle.
    //Battle order always goes P1->P2->P3->E1->E2->E3-> so with this system we can add a feature
    //where the player can switch which character slots into which position.
        

        var c1Stats = character1Prefab.GetComponent<Unit>();
        var c2Stats = character2Prefab.GetComponent<Unit>();
        var c3Stats = character3Prefab.GetComponent<Unit>();
    }
}
