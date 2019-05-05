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

    public AudioSource audioSource;
    public AudioClip sound_comprar;
    public AudioClip sound_vender;
    public AudioClip sound_ui_button_ko;

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
            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_comprar, 1f);
            }
        } else
        {
            playUISound_KO();
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
            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_vender, 1f);
            }
        } else
        {
            playUISound_KO();
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
            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_comprar, 1f);
            }
        } else
        {
            playUISound_KO();
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
            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_vender, 1f);
            }
        } else
        {
            playUISound_KO();
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
            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_comprar, 1f);
            }
        } else
        {
            playUISound_KO();
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
            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_vender, 1f);
            }
        } else
        {
            playUISound_KO();
        }
    }

    public void playUISound_KO()
    {
        if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
        {
            audioSource.PlayOneShot(sound_ui_button_ko, 1f);
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
