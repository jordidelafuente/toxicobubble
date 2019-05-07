using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController //: MonoBehaviour
{
    //public RoundData[] allRoundData;
    public static PlayerProgress playerProgress;

    public static OptionsConfig optionsConfig;
    
    // Start is called before the first frame update
   /* void Start()
    {
        DontDestroyOnLoad(gameObject);
        PlayerPrefs.DeleteAll();
        loadPlayerProgress();
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public RoundData getCurrentRoundData()
    {
        return allRoundData[0];
    }*/

    public void loadOptionsConfig()
    {
        optionsConfig = new OptionsConfig();
        if (PlayerPrefs.HasKey("musicOn"))
        {
            optionsConfig.musicOn = PlayerPrefs.GetInt("musicOn");
        }

        if (PlayerPrefs.HasKey("soundsOn"))
        {
            optionsConfig.soundsOn = PlayerPrefs.GetInt("soundsOn");
        }

        if (PlayerPrefs.HasKey("userName"))
        {
            optionsConfig.userName = PlayerPrefs.GetString("userName");
        }

        if (PlayerPrefs.HasKey("difficulty"))
        {
            optionsConfig.difficulty = PlayerPrefs.GetInt("difficulty");
        }
    }

    public void saveOptionsConfig()
    {
        PlayerPrefs.SetInt("musicOn", optionsConfig.musicOn);
        PlayerPrefs.SetInt("soundsOn", optionsConfig.soundsOn);
        PlayerPrefs.SetString("userName", optionsConfig.userName);
        PlayerPrefs.SetInt("difficulty", optionsConfig.difficulty);
    }

    public void loadPlayerProgress()
    {
        playerProgress = new PlayerProgress();
        //PlayerPrefs.DeleteKey("bestScore");

        if (PlayerPrefs.HasKey("bestScore"))
        {
            playerProgress.bestScore = PlayerPrefs.GetInt("bestScore");
        }

       /* if (PlayerPrefs.HasKey("stateHistory"))
        {
            playerProgress.stateHistory = PlayerPrefs.GetInt("stateHistory");
        }*/
    }

    private void savePlayerProgress()
    {
        PlayerPrefs.SetInt("bestScore", playerProgress.bestScore);
        PlayerPrefs.SetInt("stateHistory", playerProgress.stateHistory);
    }

    public void submitNewPlayerScore(int newScore)
    {
        if (newScore > playerProgress.bestScore)
        {
            playerProgress.bestScore = newScore;
            savePlayerProgress();
        }
    }

    public int getBestScore()
    {
        return playerProgress.bestScore;
    }

    public OptionsConfig getOptionsConfig()
    {
        return optionsConfig;
    }

    public PlayerProgress getPlayerProgress()
    {
        return playerProgress;
    }
}
