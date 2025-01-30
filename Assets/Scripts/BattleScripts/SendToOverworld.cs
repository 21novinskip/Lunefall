using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToOverworld : MonoBehaviour
{
    public Unit unit;
    //This will be put on enemy, so when ded calls the function
    void Start()
    {
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.currentHP <= 0)
        {
            GameManager.Instance.endBattle();
        }
    }
}
