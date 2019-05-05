using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorPanels : MonoBehaviour {

    public GameObject panelCreditos;
    public GameObject panelOpciones;

    private static DataController dataController;

    public AudioSource audioSource;
    public AudioSource musicSource;

    public AudioClip sound_ui_button_ok;
    public AudioClip music_main_menu;

    // Use this for initialization
    void Start () {
        dataController = new DataController();

        playMusic();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Quitting the game and returning to Windows
    public void Salir()
    {
        Application.Quit();
        playUISound_OK();
    }

    //Open the credits panel
    public void AbrirPanelCreditos()
    {
        panelCreditos.SetActive(true);
        playUISound_OK();
    }

    //Close the credits panel
    public void CerrarPanelCreditos()
    {
        panelCreditos.SetActive(false);
        playUISound_OK();
    }

    //Open the options panel
    public void AbrirPanelOpciones()
    {
        dataController.loadOptionsConfig();
        loadOptionsContentPane1();
        panelOpciones.SetActive(true);
        playUISound_OK();
    }

    public void loadOptionsContentPane1()
    {
        playUISound_OK();

        Dropdown[] dropDowns = panelOpciones.GetComponentsInChildren<Dropdown>();
        foreach (Dropdown dd in dropDowns)
        {
            if (dd.name == "Dropdown-Music")
            {
                dd.value = DataController.optionsConfig.musicOn;
            }
            else if (dd.name == "Dropdown-Sound-Effects")
            {
                dd.value = DataController.optionsConfig.soundsOn;
            }
        }
    }

    public void loadOptionsContentPane2()
    {
        playUISound_OK();

        InputField[] inputs = panelOpciones.GetComponentsInChildren<InputField>();
        foreach (InputField inputF in inputs)
        {
            if (inputF.name == "Input-Name-Player")
            {
                inputF.text = DataController.optionsConfig.userName;
            }
        }

        Dropdown[] dropDowns = panelOpciones.GetComponentsInChildren<Dropdown>();
        foreach (Dropdown dd in dropDowns)
        {
            if (dd.name == "Dropdown-Difficulty")
            {
                dd.value = DataController.optionsConfig.difficulty;
            }
        }
    }

    //Close the options panel
    public void CerrarPanelOpciones()
    {
        dataController.saveOptionsConfig();
        playUISound_OK();
        panelOpciones.SetActive(false);
    }

    public void saveMusicOnOff(Dropdown dropDown)
    {
        //dropDown.value = dataController.
        DataController.optionsConfig.musicOn = dropDown.value;
        playMusic();
    }

    public void saveSoundsOnOff(Dropdown dropDown)
    {
        //dropDown.value = dataController.
        DataController.optionsConfig.soundsOn = dropDown.value;
    }

    public void saveUserName(InputField newName)
    {
        //dropDown.value = dataController.
        DataController.optionsConfig.userName = newName.text.ToString();
    }

    public void saveDifficulty(Dropdown dropDown)
    {
        //dropDown.value = dataController.
        DataController.optionsConfig.difficulty = dropDown.value;
    }

    public void Play()
    {
        playUISound_OK();
        SceneManager.LoadScene("NivelPrincipal-portrait");
        Time.timeScale = 1;
    }

    public void playUISound_OK()
    {
        if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
        {
            audioSource.PlayOneShot(sound_ui_button_ok, 1f);
        }
    }

    public void playMusic()
    {
        if (ManejadorDisparo.getDataController().getOptionsConfig().musicOn == 0) //TODO: constantes
        {
            musicSource.clip = music_main_menu;
            musicSource.Play();
        } else if(musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

}
