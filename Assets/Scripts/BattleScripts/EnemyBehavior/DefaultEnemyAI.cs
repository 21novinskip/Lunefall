using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyAI : MonoBehaviour
{
    public string targetunit;
    public GameObject targetObject;
    // Start is called before the first frame update
    public int myTurn()
    {
        var myTarget = Random.Range(0 , 3);

        //Debug.Log("I rolled a " + myTarget + " on my targeting roll!");
        return myTarget;
    }

    public int myAttack()
    {
        var pickedAttack = 0;
        var myStats = GetComponent<Unit>();
        if (myStats.currentAP >= 3)
        {
            pickedAttack = Random.Range(1 , 3);
            return pickedAttack;
        }
        else if (myStats.currentAP >= 2)
        {
            pickedAttack = Random.Range(1 , 2);
            return pickedAttack;
        }
        else
        {
            pickedAttack = 1;
            return pickedAttack;
        }
    }
}
