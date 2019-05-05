using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllumiCoinExtra : MonoBehaviour {

    public Text NumCoinsPlayer;

    public Transform animCoinAdd1;

    public AudioSource audioSource;
    public AudioClip sound_coin_extra;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bola")
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            Transform animCoin = Instantiate(animCoinAdd1, col.gameObject.transform.position, Quaternion.identity);
            //Text[] textos = animCoin.GetComponentsInChildren<Text>();
            //Debug.Log("texto: " + textos[0].text);
            animCoin.gameObject.tag = "AnimCoin+1";

            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_coin_extra, 1f);
            }

            int newNumCoinsPlayer = int.Parse(NumCoinsPlayer.text.ToString()) + 1;
            NumCoinsPlayer.text = newNumCoinsPlayer.ToString();
        }
    }
}
