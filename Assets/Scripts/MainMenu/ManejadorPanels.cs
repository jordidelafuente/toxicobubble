using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorPanels : MonoBehaviour {

    public GameObject panelCreditos;
    public GameObject panelOpciones;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Quitting the game and returning to Windows
    public void Salir()
    {
        Application.Quit();
    }

    //Open the credits panel
    public void AbrirPanelCreditos()
    {
        panelCreditos.SetActive(true);
    }

    //Close the credits panel
    public void CerrarPanelCreditos()
    {
        panelCreditos.SetActive(false);
    }

    //Open the options panel
    public void AbrirPanelOpciones()
    {
        panelOpciones.SetActive(true);
    }

    //Close the options panel
    public void CerrarPanelOpciones()
    {
        panelOpciones.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("NivelPrincipal-portrait");
        Time.timeScale = 1;
    }

}
