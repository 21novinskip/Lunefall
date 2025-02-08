using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivationCondition{BattleStart, PlayerPhase, BeginMyTurn, EndMyTurn, WhenAttacking, WhenHealing, WhenComboActive, WhenAttacked, WhenDefeating, WhenDefeated, EnemyPhase, BattleEnd, None,}

[System.Serializable]
public class GearDetails
{
    [Header("Gear details")]
    public string gearName;
    public Sprite gearIcon;
    public string geardDescription;
    public bool isEquipped;
    [Header("Does the gear change HP?")]
    public bool changesHealth;
    public int healthChangedBy;
    [Header("Does the gear change STR?")]
    public bool changesStrength;
    public int strengthChangedBy;
    [Header("Does the gear change AGI?")]
    public bool changesAgility;
    public int agilityChangedBy;
    [Header("Does the gear change LCK?")]
    public bool changesLuck;
    public int luckChangedBy;
    [Header("Does the gear change DEF?")]
    public bool changesDefense;
    public int denfeseChangedBy;
    [Header("Passive Effect")]
    public bool givesPassive;
    public ActivationCondition onWhatCondition;
}

public class Gear : MonoBehaviour
{

    public GearDetails gearDeatails;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
