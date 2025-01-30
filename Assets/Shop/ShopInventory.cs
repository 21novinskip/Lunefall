using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
public GameObject shopItem1;
public string shopItem1Name;
public int shopItem1Price;
public int shopItem1Quantity;

    void Update()
    {
        var i1Details = shopItem1.GetComponent<ItemDetails>();

        shopItem1Name = i1Details.itemName;
        shopItem1Price = i1Details.itemBuyPrice;
        shopItem1Quantity = i1Details.itemQuantity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colliding with something: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.enterShop();
        }
    }
}
