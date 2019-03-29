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
    public Transform bolaExtra;
    public Transform illumiCoinExtra;
    public Transform burbuja;
    public Text textNumeroDeBolas;
    public Text textScore;
    public int velocidadBolas;
    enum EstadoPlayer { READY, SHOOTING, MOVING };

    static int velocidadBolasGlobal;

    

    int tipoBola; //TODO: different types of balls by booster

    LineRenderer lineRenderer;
    GameObject[] burbujas, bolas, bolasExtra, illumiCoinsExtra;
    //PointerEventData ultimaPosicionMosuse;
    
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
        ManejadorBolas.SetXPrimeraBola(-9999);
        
        estadoPlayer = EstadoPlayer.READY;

        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (estadoPlayer == EstadoPlayer.SHOOTING)
        {
            if (numBolasDisparadas < numBolasADisparar && (Time.time - timeInicioDisparo > 0.1*numBolasDisparadas))
            {
                dispararBola();
            } else
            {
                if (!hayMasBolas())
                {
                    generarBurbujas(GetScore());
                    if ((GetScore()) % 7 == 0)
                    {
                        generarIllumiCoin();
                    }

                    if ((GetScore()) % 5 == 0 ) //Every x turns we create a new extra ball
                    { 
                        generarBolaExtra();
                    }

                    moverBurbujas();
                    moverBolasExtra();
                    moverIllumiCoins();
                    updateScore();
                    estadoPlayer = EstadoPlayer.READY;
                    numBolasADisparar = GetNumBolasFromPlayer();
                    textNumeroDeBolas.gameObject.SetActive(false);
                    numBolasDisparadas = 0;
                    if (ManejadorBolas.GetXPrimeraBola() != -9999f) {
                        player.transform.position = new Vector3(ManejadorBolas.GetXPrimeraBola(), player.transform.position.y, player.transform.position.z);
                        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));                      
                        ManejadorBolas.SetXPrimeraBola(-9999f);
                    }
                }
            }
        }       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            //...
            textNumeroDeBolas.gameObject.SetActive(true);
            textNumeroDeBolas.gameObject.transform.position = new Vector2(player.transform.position.x-100f/*TODO:ajustar a pantallas*/, textNumeroDeBolas.gameObject.transform.position.y);

            //float angle = Vector3.Angle(lineRenderer.GetPosition(1), lineRenderer.GetPosition(0));
            //Debug.Log("Angulo: " + angle);

            //Plotting the line before shooting
            if (Camera.main.ScreenToWorldPoint(eventData.position).y > -50) 
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

    public void generarIllumiCoin()
    {
        //Instantiate a bubble at a random "x,y" position
        int xRandom = (int)Random.Range(-700f, 725f); //TODO: ajustar a pantallas
        int yRandom = (int)Random.Range(-700f, 725f); //TODO: ajustar a pantallas
        Vector3 posicion = new Vector3(xRandom, yRandom, 90);
        Transform coinExtraNew = Instantiate(illumiCoinExtra, posicion, Quaternion.identity); 
    }

    public void generarBolaExtra()
    {
        //Instantiate a bubble at a random "x" position
        int xRandom = (int)Random.Range(-700f, 725f); //TODO: ajustar a pantallas
        Vector3 posicion = new Vector3(xRandom, 500, 90);
        Transform bolaExtraNew = Instantiate(bolaExtra, posicion, Quaternion.identity);
    }

    void generarBurbujas(int score)
    {
        int numBurbujas = 1;

        if (score % 5 == 0)
        {
            numBurbujas++;
        }

        for(int i=0; i< numBurbujas; i++)
        {
            generarBurbuja();
        }
    }

    public void generarBurbuja() //TODO: balancear dificultad (recurrente)
    {
        //Instantiate a bubble at a random "x" position
        int xRandom = (int)Random.Range(-700f, 725f); //TODO: ajustar a pantallas
        Vector3 posicion = new Vector3(xRandom, 500, 90);
        Transform burbujaNueva = Instantiate(burbuja, posicion, Quaternion.identity);

        int pesoDeseado = numBolasADisparar + 1; //TODO: función que también tenga en cuenta la puntuación 
        
        foreach (Transform b in burbujaNueva.gameObject.gameObject.transform)
        {
            if (b.gameObject.gameObject.name == "Peso")
            {
                b.gameObject.gameObject.GetComponent<TextMesh>().text = pesoDeseado.ToString();
                b.gameObject.gameObject.transform.position = b.gameObject.transform.position;//?
            }
        }
    }

    public void moverBurbujas()
    {
        burbujas = GameObject.FindGameObjectsWithTag("Burbuja");
        foreach (GameObject burbuja in burbujas)
        {
            if (burbuja.transform.position.x < 1500) //TODO: burbuja fuera del canvas no se cae (con función bien hecha)
            {
                //Moving bubbles one "step" to the floor
                burbuja.GetComponent<Rigidbody2D>().MovePosition(
                                             new Vector3(burbuja.transform.position.x,
                                                         burbuja.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                         burbuja.transform.position.z));
            }
        }
    }

    public void moverBolasExtra()
    {
        bolasExtra = GameObject.FindGameObjectsWithTag("BolaExtra");
        foreach (GameObject bolaAux in bolasExtra)
        {
            if (bolaAux.transform.position.x < 1500) //TODO: burbuja fuera del canvas no se cae (con función bien hecha)
            {
                //Moving bubbles one "step" to the floor
                bolaAux.GetComponent<Rigidbody2D>().MovePosition(
                                             new Vector3(bolaAux.transform.position.x,
                                                         bolaAux.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                         bolaAux.transform.position.z));

          
            }
        }
    }
    public void moverIllumiCoins()
    {
        illumiCoinsExtra = GameObject.FindGameObjectsWithTag("IllumiCoinExtra");
        foreach (GameObject coin in illumiCoinsExtra)
        {
            if (coin.transform.position.x < 1500) //TODO: burbuja fuera del canvas no se cae (con función bien hecha)
            {
                //Moving bubbles one "step" to the floor
                coin.GetComponent<Rigidbody2D>().MovePosition(new Vector3(coin.transform.position.x,
                                                         coin.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                         coin.transform.position.z));
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

    int GetScore()
    {
        return int.Parse(textScore.text.ToString().Replace("Score: ", ""));
    }

    void updateScore()
    {
        int newScore = int.Parse(textScore.text.ToString().Replace("Score: ","")) + 1;
        textScore.text = "Score: " + newScore.ToString();
    }

    public void dispararBola()
    {        
        Transform bolaAux = Instantiate(bola, player.transform.position, Quaternion.identity);
        Vector3 vAux = lineRenderer.GetPosition(1) - player.transform.position;
        bolaAux.GetComponent<Rigidbody2D>().velocity = vAux.normalized * velocidadBolas; //TODO: recoger velocidad por parametro
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
