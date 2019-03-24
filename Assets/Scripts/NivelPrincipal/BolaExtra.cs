﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BolaExtra : MonoBehaviour {

    public Text NumBolasPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bola")
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            int newNumBolasPlayer = int.Parse(NumBolasPlayer.text.ToString().Replace("x", "")) + 1;
            NumBolasPlayer.text = "x" + newNumBolasPlayer.ToString();
        }
    }
}
