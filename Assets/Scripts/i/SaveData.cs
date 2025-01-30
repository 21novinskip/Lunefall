using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSave", menuName = "Save/SaveData")]
public class SaveData : ScriptableObject
{
    public bool tutorialFinished;

    public int partyLevel;
    public int currentEXP;
    public int expToNext;
    [Header("Karlot Stats")]
    public int p0HP;
    public int p0MaxHP;
    public int p0AP;
    public int p0STR;
    public int p0AGI;
    public int p0LCK;
    public int p0DEF;
    [Header("Catalina Stats")]
    public int p1HP;
    public int p1MaxHP;
    public int p1AP;
    public int p1STR;
    public int p1AGI;
    public int p1LCK;
    public int p1DEF;
    [Header("Hildegard Stats")]
    public int p2HP;
    public int p2MaxHP;
    public int p2AP;
    public int p2STR;
    public int p2AGI;
    public int p2LCK;
    public int p2DEF;
    [Header("Overworld Details")]
    public Vector3 p1Position;
    public string overworldEnemyID;
    public bool overworldEnemyKilled;
    public string previousScene;
    public string previousSceneOW;
}