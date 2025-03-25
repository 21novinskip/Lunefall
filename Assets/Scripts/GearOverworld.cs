using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearOverworld : MonoBehaviour
{
    public GameObject gearPrefab;
    private Gear gearDetails;
    private bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        gearDetails = gearPrefab.GetComponent<Gear>();
        gameObject.GetComponent<SpriteRenderer>().sprite = gearDetails.gearIcon;
        Activate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colided with item");
        if (collision.gameObject.CompareTag("witchOW") || collision.gameObject.CompareTag("fighterOW") || collision.gameObject.CompareTag("tankOW"))
        {
            Debug.Log("Player hit");
            GameManager.Instance.GetComponent<PlayerInventory>().AddToInventory(gearPrefab);
            collected = true;
            Activate();
        }
    }
    void Activate()
    {
        if (collected == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Item Active.");
        }
    }
}
