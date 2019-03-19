using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejadorPanel : MonoBehaviour {

    public GameObject panelPausa;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void activarPanelPausa()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0;
    }

    public void desactivarPanelPausa()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene("NivelPrincipal");
        Time.timeScale = 1;
    }
}
