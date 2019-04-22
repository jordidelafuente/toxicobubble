﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorBolas : MonoBehaviour
{
    int velocidadBolas;
    int pesoDanoBola;
    int numRebotesPermitidos;

    public Transform explosionBurbuja;
    
    //public Animation explosion_burbuja;

    static float xPrimeraBola = -9999f;

    // Use this for initialization
    void Start () {
        velocidadBolas = ManejadorDisparo.getVelocidadBolas();

        if (ManejadorDisparo.getBoostersActivados()[2] == true)
        {
            numRebotesPermitidos = 1;
        } else
        {
            numRebotesPermitidos = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.tag == "Bola" && OutOfGameWindow()) 
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject); 
        }
		
	}

    bool OutOfGameWindow()
    {
        return !GetComponent<Renderer>().isVisible;
    }


    // when the ball collides with something
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Burbuja")
        {
            if (ManejadorDisparo.getBoostersActivados()[1] == true)
            {
                pesoDanoBola = 2;
            }
            else
            {
                pesoDanoBola = 1;
            }
            //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time + " tag: " + col.gameObject.tag);

            //START: Calculate reflection velocity
            Vector2 normal = col.gameObject.transform.position - gameObject.transform.position; //pos burbuja - pos bola
            if (col.gameObject.transform.position.x > gameObject.transform.position.x) // burbuja.x > bola.x
            {
                normal.x = Mathf.Abs(normal.x);
            } else
            {
                normal.x = Mathf.Abs(normal.x)*(-1);
            }

            if (col.gameObject.transform.position.y > gameObject.transform.position.y) // burbuja.y > bola.y
            {
                normal.y = Mathf.Abs(normal.y);
            }
            else
            {
                normal.y = Mathf.Abs(normal.y) * (-1);
            }
            Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, normal);
            //END: Calculate reflection velocity

            gameObject.GetComponent<Rigidbody2D>().velocity = reflejado.normalized * velocidadBolas; 

            foreach (Transform b in col.gameObject.gameObject.transform)
            {
                if (b.gameObject.gameObject.name == "Peso")
                {
                    int peso = int.Parse(b.gameObject.gameObject.GetComponent<TextMesh>().text) - pesoDanoBola;
                    if (peso > 0)
                    {
                        b.gameObject.gameObject.GetComponent<TextMesh>().text = peso.ToString();
                    }
                    else
                    {
                        col.gameObject.SetActive(false);
                        Destroy(col.gameObject);
                        Transform explosion = Instantiate(explosionBurbuja, col.gameObject.transform.position, Quaternion.identity);
                        explosion.gameObject.tag = "Explosion";
                        //explosion.GetComponent<Animation>().Play();
                        //Animation anim = Animation.

                    }
                }
            }
        }
        else if (col.gameObject.tag == "Edificio")
        {
            //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time + " tag: " + col.gameObject.tag);
            Vector2 normal = new Vector2(-1, 0); //..
            Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, normal);
            gameObject.GetComponent<Rigidbody2D>().velocity = reflejado.normalized * velocidadBolas; //TODO: recoger velocidad por parametro
        }
        else if (col.gameObject.tag == "Suelo")
        {
            if (numRebotesPermitidos > 0)
            {
                numRebotesPermitidos--;
                Vector2 normal = new Vector2(0, 1); //..
                Vector2 reflejado = Vector2.Reflect(gameObject.GetComponent<Rigidbody2D>().velocity, normal);
                gameObject.GetComponent<Rigidbody2D>().velocity = reflejado.normalized * velocidadBolas;
            }
            else
            {
                //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time + " tag: " + col.gameObject.tag);
                if (xPrimeraBola == -9999f)
                {
                    xPrimeraBola = this.transform.position.x;
                }

                //TODO: si se usa powerup de suelo se le resta 1
                Destroy(this.gameObject);
            }
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
