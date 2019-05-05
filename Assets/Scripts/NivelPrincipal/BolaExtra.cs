using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BolaExtra : MonoBehaviour {

    public Text NumBolasPlayer;

    public Transform animBolaAdd1;

    public AudioSource audioSource;
    public AudioClip sound_bola_extra;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.tag == "BolaExtra" && Time.timeScale == 1 )
        {
            //Debug.Log("scale: " + transform.lossyScale);
            transform.localScale = transform.localScale + new Vector3(0.05f * (Mathf.Sin(Time.time * 2)), 0.05f * (Mathf.Sin(Time.time * 2)), 0);
        }      
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bola")
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            Transform animCoin = Instantiate(animBolaAdd1, col.gameObject.transform.position, Quaternion.identity);
            animCoin.gameObject.tag = "AnimBola+1";

            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_bola_extra, 1f);
            }

            int newNumBolasPlayer = int.Parse(NumBolasPlayer.text.ToString().Replace("x", "")) + 1;
            NumBolasPlayer.text = "x" + newNumBolasPlayer.ToString();
        }
    }
}
