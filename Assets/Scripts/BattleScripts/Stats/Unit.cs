using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [Header("Stat Information")]
    public string unitName;
    public int maxHP;
    public int maxAP;
    public int currentHP;
    public int currentAP;
    public int Strength;
    public int Agility;
    public int Luck;
    public int Defense;
    [Header("Level Information")]
    public int healthGrowth;
    public int strengthGrowth;
    public int agilityGrowth;
    public int luckGrowth;
    public int defenseGrowth;
    public int unitLevel;
    public int expOnDeath;
    [Header("Buffs & Multipliers")]
    public float rawIncrease = 1.0f;
    public float attackMultiplier = 1.0f;
    public float attackMaximum = 1.8f;
    public float attackMinimum = 0.2f;
    public float defenseMultiplier = 1.0f;
    public float defenseMaximum = 1.8f;
    public float defenseMinimum = 0.2f;
    public float agilityMultiplier = 1.0f;
    public float agilityMaximum = 1.8f;
    public float agilityMinimum = 0.2f;
    public float luckMultiplier = 1.0f;
    public float luckMaximum = 1.8f;
    public float luckMinimum = 0.2f;
    [Header("other")]
    public bool isDead = false;
    public int number;
    public Sprite neutralPortrait;
    public Sprite deadPortrait;
    [Header("Sounds")]
    public AudioSource snd_Light;
    public AudioSource snd_Medium;
    public AudioSource snd_Heavy;

    public void LevelUp()
    {
        unitLevel += 1;

        int healthGain = Mathf.RoundToInt(unitLevel * healthGrowth / 100f);
        Debug.Log(unitName + " gained " + healthGain + " HP!");
        maxHP += healthGain;

        int strGain = Mathf.RoundToInt(unitLevel * strengthGrowth / 100f);
        Debug.Log(unitName + " gained " + strGain + " Strength!");
        Strength += strGain; 
    
        int agiGain = Mathf.RoundToInt(unitLevel * agilityGrowth / 100f);
        Debug.Log(unitName + " gained " + agiGain + " Agility!");
        Agility += agiGain; 

        int lckGain = Mathf.RoundToInt(unitLevel * luckGrowth / 100f);
        Debug.Log(unitName + " gained " + lckGain + " Luck!");
        Luck += lckGain; 

        int defGain = Mathf.RoundToInt(unitLevel * defenseGrowth / 100f);
        Debug.Log(unitName + " gained " + defGain + " Defense!");
        Defense += defGain; 
        if (GameManager.Instance.partyLevel == 4 || GameManager.Instance.partyLevel == 7 || GameManager.Instance.partyLevel == 11 || GameManager.Instance.partyLevel == 16)
        {
            maxAP += 1;
        }
    }

    
}
