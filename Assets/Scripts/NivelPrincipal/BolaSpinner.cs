using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaSpinner : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound_bubbling;

    int velocidadBolas;
    Vector3 direccion;

    // Start is called before the first frame update
    void Start()
    {
        velocidadBolas = ManejadorDisparo.getVelocidadBolas();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 500 * Time.deltaTime);
    }

    // when a ball is colliding with it
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bola")
        {
            
            if (ManejadorDisparo.getDataController().getOptionsConfig().soundsOn == 0) //TODO: constantes
            {
                audioSource.PlayOneShot(sound_bubbling, 1f);
            }

            direccion = new Vector3(Random.Range(-100,100), Random.Range(-100, 100), 0);

            col.GetComponent<Rigidbody2D>().velocity = direccion.normalized * velocidadBolas;
            col.isTrigger = false;
        }
    }
}
