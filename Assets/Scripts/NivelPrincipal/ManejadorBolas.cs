using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorBolas : MonoBehaviour
{

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, coll.contacts[0].normal);
        gameObject.GetComponent<Rigidbody2D>().velocity = reflejado;
    }*/

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }

    // when the ball collides with something
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Burbuja")
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time + " tag: " + col.gameObject.tag);

            //Calculate reflection velocity
            Vector2 normal = col.gameObject.transform.position - gameObject.transform.position;
            Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, normal);
            gameObject.GetComponent<Rigidbody2D>().velocity = reflejado.normalized * 100;

            foreach (Transform b in col.gameObject.gameObject.transform)
            {
                if(b.gameObject.gameObject.name == "Peso")
                {
                    int peso = int.Parse(b.gameObject.gameObject.GetComponent<TextMesh>().text) -1;
                    if (peso > 0)
                    {
                        b.gameObject.gameObject.GetComponent<TextMesh>().text = peso.ToString();
                    } else
                    {
                        Destroy(col.gameObject);
                    }
                }
            }
        }
        
    }
}
