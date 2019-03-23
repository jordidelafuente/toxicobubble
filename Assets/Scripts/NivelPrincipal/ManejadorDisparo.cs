using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManejadorDisparo : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
                         IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public GameObject lineaDisparo;
    public GameObject player;
    public Transform bola;
    public Transform burbuja;
    public Text textNumeroDeBolas;
    public int velocidadBolas;
    enum EstadoPlayer { READY, SHOOTING };

    static int velocidadBolasGlobal;

    

    int tipoBola; //TODO: different types of balls by booster

    public GameObject panelGameOver;

    LineRenderer lineRenderer;
    GameObject[] burbujas, bolas;
    PointerEventData ultimaPosicionMosuse;
    
    float timeInicioDisparo;
    int numBolasADisparar, numBolasDisparadas;
    EstadoPlayer estadoPlayer;

    // Use this for initialization
    void Start()
    {
        velocidadBolasGlobal = velocidadBolas;
        numBolasADisparar = GetNumBolasFromPlayer();
        lineaDisparo.SetActive(false);
        textNumeroDeBolas.gameObject.SetActive(false);
        
        //heDisparado = false; //TODO:borrar
        estadoPlayer = EstadoPlayer.READY;

        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (estadoPlayer == EstadoPlayer.SHOOTING)
        {
            if (numBolasDisparadas < numBolasADisparar && (Time.time - timeInicioDisparo > 0.1))
            {
                dispararBola();
            } else
            {
                if (!hayMasBolas())
                {
                    generarBurbujas();
                    moverBurbujas();
                    estadoPlayer = EstadoPlayer.READY;
                    numBolasDisparadas = 0;
                }
            }
        }       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineRenderer.SetPosition(1, eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            //...
            textNumeroDeBolas.gameObject.SetActive(true);
            ultimaPosicionMosuse = eventData;

            //Plotting the line before shooting
            if (/*Camera.main.ScreenToWorldPoint(*/eventData.position/*)*/.y > 400)
            { //TODO: adaptar a diferentes plataformas y resoluciones
                lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position));
                lineaDisparo.SetActive(true);
            }
            else
            {
                lineaDisparo.SetActive(false);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        timeInicioDisparo = Time.time;

        if (!hayMasBolas())
        {
            lineaDisparo.SetActive(false);
            estadoPlayer = EstadoPlayer.SHOOTING;
            dispararBola();
            textNumeroDeBolas.gameObject.SetActive(false);
        }
    }

    bool hayMasBolas()
    {
        bolas = GameObject.FindGameObjectsWithTag("Bola");
        if (bolas != null && bolas.Length > 1)
        {
            return true;
        }
        return false;
    }

    public void generarBurbujas() //TODO: arreglar dificultad
    {
        int xRandom = (int)Random.Range(-700f, 725f); //TODO: ajustar a pantallas
        Vector2 posicion = new Vector2(xRandom, 500);
        Transform burbujaNueva = Instantiate(burbuja, posicion, Quaternion.identity);
    }

    public void moverBurbujas()
    {
        burbujas = GameObject.FindGameObjectsWithTag("Burbuja");
        foreach (GameObject burbuja in burbujas)
        {
            if (burbuja.transform.position.x < 1500) //TODO: burbuja fuera del canvas no se cae (con función bien hecha)
            {
                //Moving bubbles one "step" to the floor
                burbuja.transform.position = new Vector3(burbuja.transform.position.x,
                                                         burbuja.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                         burbuja.transform.position.z);

                bool tocaSuelo = burbuja.transform.position.y <= -228; //Todo: relacionar colliders de burbujas y suelo
                if (tocaSuelo)
                {
                    //Debug.Log("Tocando Suelo!!!");
                    panelGameOver.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    //Getting the number of balls we will shoot 
    private int GetNumBolasFromPlayer()
    {
        int numBolas = 0;
        Text[] textos = player.GetComponentsInChildren<Text>();
        string strNumBolas = textos[0].text.ToString().Replace("x", ""); //TODO: que pasa si hay más de un componente texto?
        numBolas = int.Parse(strNumBolas);

        return numBolas;
    }

    public void dispararBola()
    {        
        Transform bolaAux = Instantiate(bola, player.transform.position, Quaternion.identity);
        Vector3 vAux = lineRenderer.GetPosition(1) - player.transform.position;
        bolaAux.GetComponent<Rigidbody2D>().velocity = vAux.normalized * velocidadBolas; //TODO: recoger velocidad por parametro
                                                                                         /*Transform ultimaBola = bolaAux;                                                                     //StartCoroutine("Wait");
                                                                                         //float timeBolaAnt = Time.time;

                                                                                             Transform bolaAux2 = Instantiate(bola, player.transform.position, Quaternion.identity);
                                                                                             bolaAux2.GetComponent<Rigidbody2D>().velocity = vAux.normalized * velocidadBolas; //TODO: recoger velocidad por parametro
                                                                                             bolaAux = bolaAux2;
                                                                                         }    */

        numBolasDisparadas++;
    }

    public static int getVelocidadBolas()
    {
        return velocidadBolasGlobal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
    }

}
