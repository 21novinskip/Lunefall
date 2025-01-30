using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uiThings : MonoBehaviour
{
    [Header ("Stuff")]
    public TMP_Text myMoney;
    public TMP_Text myMoney2;
    [Header("Buy Item 1")]
    public TMP_Text buy1Name;
    public TMP_Text buy1Price;
    public TMP_Text buy1Quant;
void Update()
{
//BuyList
    var comp = GetComponent<ShopInventory>();
    var myInv = GetComponent<PlayerInventory>();
    myMoney.text = "$" + myInv.currentMoney.ToString();
    if (comp.shopItem1Name != null)
    {
        buy1Name.text = comp.shopItem1Name.ToString();
        buy1Price.text = comp.shopItem1Price.ToString();
        buy1Quant.text = comp.shopItem1Quantity.ToString();
    }
    else
    {
        buy1Name.text = "";
        buy1Price.text = "";
        buy1Quant.text = "";
    }
}

}


