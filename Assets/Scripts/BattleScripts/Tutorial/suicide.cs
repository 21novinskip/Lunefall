using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suicide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.tutorialFinished == true && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }   
    }
}
