using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ManejadorPanel : MonoBehaviour
{

    public GameObject panelPausa;
    public GameObject panelGameOver;
    public GameObject panelShop;
    public GameObject panelMoving;
    public GameObject panelBoosters;

    public Text illumiCoins;
    public Text insufficientCoins;

    public AudioSource audioSource;
    public AudioClip sound_ui_button_ok;
    public AudioClip sound_ui_button_ko;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void activarPanelShop()
    {
        panelShop.SetActive(true);
        panelMoving.SetActive(false); //it can be open when changing panel
        Time.timeScale = 0;
        updateBudgetsToPanelShop();

        playUISound_OK();
    }

    public void desactivarPanelShop()
    {
        panelShop.SetActive(false);
        ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
        Time.timeScale = 1;

        playUISound_OK();
    }

    public void activarPanelPausa()
    {
        panelPausa.SetActive(true);
        panelMoving.SetActive(false); //it can be open when changing panel
        Time.timeScale = 0;

        playUISound_OK();
    }

    public void desactivarPanelPausa()
    {
        panelPausa.SetActive(false);
        ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
        Time.timeScale = 1;

        playUISound_OK();
    }

    public void activarPanelBoosters()
    {
        if (ManejadorDisparo.estadoPlayer == ManejadorDisparo.EstadoPlayer.READY)
        {
            panelBoosters.SetActive(true);
            panelMoving.SetActive(false); //it can be open when changing panel
            Time.timeScale = 0;
            playUISound_OK();
        } else
        {
            playUISound_KO();
        }
        
    }

    public void desactivarPanelBoosters()
    {
        panelBoosters.SetActive(false);
        ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
        Time.timeScale = 1;

        playUISound_OK();
    }

    public void mainMenu()
    {
        playUISound_OK();
        SceneManager.LoadScene("MainMenu");
    }

    public void Reiniciar()
    {
        playUISound_OK();
        SceneManager.LoadScene("NivelPrincipal-portrait");
        Time.timeScale = 1;
    }

    public void EvitarGameOver()
    {
        int numCoins = int.Parse(illumiCoins.text.ToString());
        if (numCoins >= 20) //TODO: coste dependerá de la dificultad del momento
        {
            numCoins -= 20;
            illumiCoins.text = numCoins.ToString();
            eliminarBurbujaMasBaja();
            panelGameOver.SetActive(false);
            ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
            ManejadorDisparo.gameOverActivated = false;
            ManejadorDisparo.changeMusic = true;
            ManejadorDisparo.getDataController().getPlayerProgress().stateHistory = ManejadorDisparo.lastStateBeforeGameOver;//TODO: last state???
            Time.timeScale = 1;
            playUISound_OK();
        } else
        {
            insufficientCoins.text = "Not enough IllumiCoins";
            playUISound_KO();
        }
    }

    void eliminarBurbujaMasBaja()
    {
        GameObject[] burbujas = GameObject.FindGameObjectsWithTag("Burbuja");
        GameObject burbujaAEliminar = null;

        foreach (GameObject burbujaAux in burbujas)
        {
            if (burbujaAEliminar == null)
            {
                burbujaAEliminar = burbujaAux;
            }
            else if (burbujaAux.transform.position.y < burbujaAEliminar.transform.position.y) //TODO: bola fuera del canvas no se cae (con función bien hecha)
            {
                burbujaAEliminar = burbujaAux;
            }
        }

        if (burbujaAEliminar != null)
        {
            burbujaAEliminar.gameObject.SetActive(false);
            Destroy(burbujaAEliminar.gameObject);
        }
    }

    public void NextTurn()
    {
        if (ManejadorDisparo.estadoPlayer == ManejadorDisparo.EstadoPlayer.SHOOTING)
        {
            GameObject[] bolas = GameObject.FindGameObjectsWithTag("Bola");
            foreach (GameObject bolaAux in bolas)
            {
                if (bolaAux.transform.position.x < 1500) //TODO: bola fuera del canvas no se cae (con función bien hecha)
                {
                    bolaAux.gameObject.SetActive(false);
                    Destroy(bolaAux.gameObject);
                }
            }
            ManejadorDisparo.numBolasADisparar = ManejadorDisparo.numBolasDisparadas;
            playUISound_OK();
        } 
        else
        {
            playUISound_KO();
        }
        
    }

    public void updateBudgetsToPanelShop()
    {
        int numBoostSize = 0;
        int numBoostFuerza = 0;
        int numBoostRebote = 0;

        //Getting booster quantity by panelBooster
        Text[] txtAllBoosters = panelBoosters.GetComponentsInChildren<Text>();
        foreach (Text txtAux in txtAllBoosters)
        {
            if (txtAux.name == "Bola-boost-size-budget")
            {
                numBoostSize = int.Parse(txtAux.text.ToString());
            }
            else if (txtAux.name == "Bola-boost-fuerza-budget")
            {
                numBoostFuerza = int.Parse(txtAux.text.ToString());
            }
            else if (txtAux.name == "Bola-boost-rebote-budget")
            {
                numBoostRebote = int.Parse(txtAux.text.ToString());
            }
        }

        //updating txt in panel Shop
        Text[] txtAllShop = panelShop.GetComponentsInChildren<Text>();
        foreach (Text txtAux in txtAllShop)
        {
            if (txtAux.name == "Text-Num-Boost-Size")
            {
                txtAux.text = numBoostSize.ToString();
            }
            else if (txtAux.name == "Text-Num-Boost-Fuerza")
            {
                txtAux.text = numBoostFuerza.ToString(); 
            }
            else if (txtAux.name == "Text-Num-Boost-Rebote")
            {
                txtAux.text = numBoostRebote.ToString(); 
            }
        }
    }


    private void playUISound_OK()
    {
        if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
        {
            audioSource.PlayOneShot(sound_ui_button_ok, 1f);
        }
    }

    private void playUISound_KO()
    {
        if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
        {
            audioSource.PlayOneShot(sound_ui_button_ko, 1f);
        }
    }
}
