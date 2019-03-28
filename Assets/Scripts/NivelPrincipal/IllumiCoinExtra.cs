using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllumiCoinExtra : MonoBehaviour {

    public Text NumCoinsPlayer;

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
            int newNumCoinsPlayer = int.Parse(NumCoinsPlayer.text.ToString()) + 1;
            NumCoinsPlayer.text = newNumCoinsPlayer.ToString();
        }
    }
}
