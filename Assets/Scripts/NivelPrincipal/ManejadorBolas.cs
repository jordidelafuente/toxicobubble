using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorBolas : MonoBehaviour
{
    int velocidadBolas;

    static float xPrimeraBola = -9999f;

    // Use this for initialization
    void Start () {
        velocidadBolas = ManejadorDisparo.getVelocidadBolas();
	}
	
	// Update is called once per frame
	void Update () {
        if (OutOfGameWindow()) 
        {
            Destroy(this.gameObject); 
        }
		
	}

    bool OutOfGameWindow()
    {
        int limiteArriba = 600;   //TODO: ajustar al tamaño de cada pantalla
        int limiteAbajo = -2000;   //
        int limiteIzquierda = -2000; //
        int limiteDerecha = 2000;  //

        if (this.transform.position.y > limiteArriba || this.transform.position.y < limiteAbajo)
        {
            return true;
        }

        if (this.transform.position.x > limiteDerecha || this.transform.position.x < limiteIzquierda)
        {
            return true;
        }

        return false;
    }

    /*void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, coll.contacts[0].normal);
        gameObject.GetComponent<Rigidbody2D>().velocity = reflejado;
    }*/

    // when the ball collides with something
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Burbuja")
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time + " tag: " + col.gameObject.tag);

            //Calculate reflection velocity
            Vector2 normal = col.gameObject.transform.position - gameObject.transform.position;
            Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, normal);
            gameObject.GetComponent<Rigidbody2D>().velocity = reflejado.normalized * velocidadBolas; //TODO: recoger velocidad por parametro

            foreach (Transform b in col.gameObject.gameObject.transform)
            {
                if (b.gameObject.gameObject.name == "Peso")
                {
                    int peso = int.Parse(b.gameObject.gameObject.GetComponent<TextMesh>().text) - 1;
                    if (peso > 0)
                    {
                        b.gameObject.gameObject.GetComponent<TextMesh>().text = peso.ToString();
                    }
                    else
                    {
                        col.gameObject.SetActive(false);
                        Destroy(col.gameObject);
                    }
                }
            }
        }
        else if (col.gameObject.tag == "Edificio")
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time + " tag: " + col.gameObject.tag);
            Vector2 normal = new Vector2(-1, 0); //..
            Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, normal);
            gameObject.GetComponent<Rigidbody2D>().velocity = reflejado.normalized * velocidadBolas; //TODO: recoger velocidad por parametro
        }
        else if (col.gameObject.tag == "Suelo")
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time + " tag: " + col.gameObject.tag);
            if (xPrimeraBola == -9999f)
            {
                xPrimeraBola = this.transform.position.x;
            }
            
            //TODO: si se usa powerup de suelo se le resta 1
            Destroy(this.gameObject);
        }
    }

    public static float GetXPrimeraBola()
    {
        return xPrimeraBola;
    }

    public static void SetXPrimeraBola(float x)
    {
        xPrimeraBola = x;
    }
}
