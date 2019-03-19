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
    bool nuevoTurno;

    // Use this for initialization
    void Start()
    {
        lineaDisparo.SetActive(false);
        textNumeroDeBolas.gameObject.SetActive(false);
        nuevoTurno = false;

        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineRenderer.SetPosition(1, eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Text texto = player.GetComponent<Text>();
        textNumeroDeBolas.gameObject.SetActive(true);

        //...
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

        GeneradorBolas();
        nuevoTurno = true;

        if (nuevoTurno)
        {
            moverBurbujas();
            textNumeroDeBolas.gameObject.SetActive(false);
        }
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
        Transform bolaAux = Instantiate(bola, player.transform.position, Quaternion.identity);
        bolaAux.GetComponent<Rigidbody2D>().velocity = new Vector2(200,200);
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
