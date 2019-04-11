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

    public Text illumiCoins;
    public Text insufficientCoins;

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
    }

    public void desactivarPanelShop()
    {
        panelShop.SetActive(false);
        ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
        Time.timeScale = 1;
    }

    public void activarPanelPausa()
    {
        panelPausa.SetActive(true);
        panelMoving.SetActive(false); //it can be open when changing panel
        Time.timeScale = 0;
    }

    public void desactivarPanelPausa()
    {
        panelPausa.SetActive(false);
        ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
        Time.timeScale = 1;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Reiniciar()
    {
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
            Time.timeScale = 1;
        } else
        {
            insufficientCoins.text = "Not enough IllumiCoins";
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
        GameObject[] bolas = GameObject.FindGameObjectsWithTag("Bola");
        foreach (GameObject bolaAux in bolas)
        {
            if (bolaAux.transform.position.x < 1500) //TODO: bola fuera del canvas no se cae (con función bien hecha)
            {
                bolaAux.gameObject.SetActive(false);
                Destroy(bolaAux.gameObject);
            }
        }
    }
}
