using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public Text txtNumBoostSize;
    public Text txtNumBoostFuerza;
    public Text txtNumBoostRebote;
    public Text txtNumIllumiCoin;

    public Text txtNumBoostSizeShop;
    public Text txtNumBoostFuerzaShop;
    public Text txtNumBoostReboteShop;
    //public Text txtNumIllumiCoinShop;

    // Start is called before the first frame update
    void Start()
    {
        /*txtNumBoostSizeShop.text = txtNumBoostSize.text.ToString();
        txtNumBoostFuerzaShop.text = txtNumBoostFuerza.text.ToString();
        txtNumBoostRebote.text = txtNumBoostRebote.text.ToString();*/
    }

    // Update is called once per frame
    void Update()
    {
        /*txtNumBoostSizeShop.text = txtNumBoostSize.text.ToString();
        txtNumBoostFuerzaShop.text = txtNumBoostFuerza.text.ToString();
        txtNumBoostRebote.text = txtNumBoostRebote.text.ToString();*/
    }

    public void comprarBoostSize(int price)
    {
        int numBoostSize = int.Parse(txtNumBoostSize.text);
        int numIllumiCoins = int.Parse(txtNumIllumiCoin.text);
        if (numIllumiCoins >= price)
        {
            numIllumiCoins -= price;
            numBoostSize ++;
            txtNumIllumiCoin.text = numIllumiCoins.ToString();
            txtNumBoostSize.text = numBoostSize.ToString();
            txtNumBoostSizeShop.text = numBoostSize.ToString();
        }
    }

    public void venderBoostSize(int price)
    {
        int numBoostSize = int.Parse(txtNumBoostSize.text);
        int numIllumiCoins = int.Parse(txtNumIllumiCoin.text);
        if (numBoostSize >= 1)
        {
            numIllumiCoins += price;
            numBoostSize--;
            txtNumIllumiCoin.text = numIllumiCoins.ToString();
            txtNumBoostSize.text = numBoostSize.ToString();
            txtNumBoostSizeShop.text = numBoostSize.ToString();
        }
    }

    public void comprarBoostFuerza(int price)
    {
        int numBoostFuerza = int.Parse(txtNumBoostFuerza.text);
        int numIllumiCoins = int.Parse(txtNumIllumiCoin.text);
        if (numIllumiCoins >= price)
        {
            numIllumiCoins -= price;
            numBoostFuerza++;
            txtNumIllumiCoin.text = numIllumiCoins.ToString();
            txtNumBoostFuerza.text = numBoostFuerza.ToString();
            txtNumBoostFuerzaShop.text = numBoostFuerza.ToString();
        }
    }

    public void venderBoostFuerza(int price)
    {
        int numBoostFuerza = int.Parse(txtNumBoostFuerza.text);
        int numIllumiCoins = int.Parse(txtNumIllumiCoin.text);
        if (numBoostFuerza >= 1)
        {
            numIllumiCoins += price;
            numBoostFuerza--;
            txtNumIllumiCoin.text = numIllumiCoins.ToString();
            txtNumBoostFuerza.text = numBoostFuerza.ToString();
            txtNumBoostFuerzaShop.text = numBoostFuerza.ToString();
        }
    }

    public void comprarBoostRebote(int price)
    {
        int numBoostRebote = int.Parse(txtNumBoostRebote.text);
        int numIllumiCoins = int.Parse(txtNumIllumiCoin.text);
        if (numIllumiCoins >= price)
        {
            numIllumiCoins -= price;
            numBoostRebote++;
            txtNumIllumiCoin.text = numIllumiCoins.ToString();
            txtNumBoostRebote.text = numBoostRebote.ToString();
            txtNumBoostReboteShop.text = numBoostRebote.ToString();
        }
    }

    public void venderBoostRebote(int price)
    {
        int numBoostRebote = int.Parse(txtNumBoostRebote.text);
        int numIllumiCoins = int.Parse(txtNumIllumiCoin.text);
        if (numBoostRebote >= 1)
        {
            numIllumiCoins += price;
            numBoostRebote--;
            txtNumIllumiCoin.text = numIllumiCoins.ToString();
            txtNumBoostRebote.text = numBoostRebote.ToString();
            txtNumBoostReboteShop.text = numBoostRebote.ToString();
        }
    }

    /*public void updateBudgetsToPanelShop()
    { 
        txtNumBoostSizeShop.text = txtNumBoostSize.text;
        txtNumBoostFuerzaShop.text = txtNumBoostFuerza.text;
        txtNumBoostRebote.text = txtNumBoostRebote.text;
        //txtNumIllumiCoinShop = txtNumIllumiCoin; 
    }*/
}
