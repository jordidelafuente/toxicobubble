﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suelo : MonoBehaviour {

    public GameObject panelGameOver;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "IllumiCoinExtra" 
               || col.gameObject.tag == "BolaExtra" 
               || col.gameObject.tag == "Calavera"
               || col.gameObject.tag == "BolaSpinner")
        {
            col.gameObject.SetActive(false);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Burbuja")
        {
            panelGameOver.SetActive(true);

            //Time.timeScale = 0;
            ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.GAMEOVER;
        }
    }
}
