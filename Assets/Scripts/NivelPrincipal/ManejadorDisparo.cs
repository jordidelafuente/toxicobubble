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
    public Text textNumeroDeBolas;

    
    int tipoBola; //TODO: different types of balls by booster

    public GameObject panelGameOver;

    LineRenderer lineRenderer;
    GameObject[] burbujas;
    PointerEventData ultimaPosicionMosuse;
    bool nuevoTurno, heDisparado;
    float inicioDisparo;

    // Use this for initialization
    void Start()
    {
        lineaDisparo.SetActive(false);
        textNumeroDeBolas.gameObject.SetActive(false);
        nuevoTurno = false;
        heDisparado = false;

        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));
    }

    // Update is called once per frame
    void Update()
    {
        nuevoTurno = heDisparado && (Time.time - inicioDisparo > 1) && !hayMasBolas();
        if (nuevoTurno)
        {
            moverBurbujas();
            nuevoTurno = false;
            heDisparado = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineRenderer.SetPosition(1, eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Text texto = player.GetComponent<Text>();
        textNumeroDeBolas.gameObject.SetActive(true);
        ultimaPosicionMosuse = eventData;

        //Plotting the line before shooting
        if (/*Camera.main.ScreenToWorldPoint(*/eventData.position/*)*/.y > 400) { //TODO: adaptar a diferentes plataformas y resoluciones
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position));
            lineaDisparo.SetActive(true);
        } else
        {
            lineaDisparo.SetActive(false);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lineaDisparo.SetActive(false);
        inicioDisparo = Time.time;

        heDisparado = true;
        GeneradorBolas();
        textNumeroDeBolas.gameObject.SetActive(false);
    }

    bool hayMasBolas()
    {
        burbujas = GameObject.FindGameObjectsWithTag("Bola");
        if (burbujas != null && burbujas.Length > 1)
        {
            return true;
        }
        return false;
    }

    public void moverBurbujas()
    {
        burbujas = GameObject.FindGameObjectsWithTag("Burbuja");
        foreach (GameObject burbuja in burbujas)
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

    public void GeneradorBolas()
    {
        //Getting the number of balls we will shoot 
        int numBolas = 0;
        Text[] textos = player.GetComponentsInChildren<Text>();
        string strNumBolas = textos[0].text.ToString().Replace("x", ""); //TODO: que pasa si hay más de un componente texto?
        numBolas = int.Parse(strNumBolas);

        
        Transform bolaAux = Instantiate(bola, player.transform.position, Quaternion.identity);
        Vector3 vAux = lineRenderer.GetPosition(1) - player.transform.position;
        bolaAux.GetComponent<Rigidbody2D>().velocity = vAux.normalized * 500; //TODO: recoger velocidad por parametro
        Transform ultimaBola = bolaAux;                                                                     //StartCoroutine("Wait");
        //float timeBolaAnt = Time.time;

        for (int i=1; i< numBolas; i++)
        {
            /*if (Time.time - timeBolaAnt < 2)
            {
                i--;
                continue;
            }*/

            Transform bolaAux2 = Instantiate(bola, player.transform.position, Quaternion.identity);
            bolaAux2.GetComponent<Rigidbody2D>().velocity = vAux.normalized * 500; //TODO: recoger velocidad por parametro
            bolaAux = bolaAux2;
        }        
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
