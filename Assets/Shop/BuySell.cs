using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BuySell : MonoBehaviour
{
[Header ("Shop Sellables")]
public GameObject shopItem1;
public string shopItem1Name;
public int shopItem1Price;
public int shopItem1Quantity;
[Header ("Stuff")]
public TMP_Text myMoney;
public GameObject player;
[Header("UI Text")]
public TMP_Text buy1Name;
public TMP_Text buy1Price;
public TMP_Text buy1Quant;

    void Start()
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
            player = collision.gameObject;
            GameManager.Instance.enterShop();
        }
    }

    void Update()
        {
        //BuyList
            if (gameObject.activeInHierarchy == false)
            {
                player = null;
            }
           if (player != null)
            {
                var myInv = player.GetComponent<PlayerInventory>();
                myMoney.text = "$" + myInv.currentMoney.ToString();
                if (shopItem1Name != null)
                {
                    buy1Name.text = shopItem1Name.ToString();
                    buy1Price.text = shopItem1Price.ToString();
                    buy1Quant.text = shopItem1Quantity.ToString();
                }
                else
                {
                    buy1Name.text = "";
                    buy1Price.text = "";
                    buy1Quant.text = "";
                }
            }
        }

    public void Buy(string itemName)
    {
        var pInv = player.GetComponent<PlayerInventory>();

        var thisObj = GameObject.Find(itemName);
        if (pInv.slot1Name == shopItem1Name)
        {
            if (pInv.currentMoney >= shopItem1Price && shopItem1Quantity > 0)
            {
                pInv.currentMoney -= shopItem1Price;
                shopItem1Quantity -= 1;
                pInv.slot1Quantity += 1;
            }
        }
        else if (pInv.slot1Item == null)
        {
            if (pInv.currentMoney >= shopItem1Price && shopItem1Quantity > 0)
            {
                pInv.slot1Item = thisObj;
                pInv.currentMoney -= shopItem1Price;
                shopItem1Quantity -= 1;
                pInv.slot1Quantity += 1;
            }
        }
    }
}
