using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoTitulo : MonoBehaviour
{
    public Transform burbuja;
    float timeBolaAnterior;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeBolaAnterior > 1.5)
        {
            generarBurbuja();
            timeBolaAnterior = Time.time;
        }
        
    }

    void generarBurbuja()
    {
        int xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
        Vector3 posicion = new Vector3(xRandom, Camera.main.pixelHeight - 1, 0);
        posicion = Camera.main.ScreenToWorldPoint(posicion);
        posicion.z = 0;

        Transform burbujaNueva = Instantiate(burbuja, posicion, Quaternion.identity);
        burbujaNueva.tag = "BurbujaFondo";

        float scale = Random.Range(0.3f, 1f);
        burbujaNueva.transform.localScale = new Vector3(scale, scale, scale);
        burbujaNueva.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -1, 0);
    }
}
