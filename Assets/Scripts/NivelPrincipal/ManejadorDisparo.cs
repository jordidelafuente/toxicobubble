using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public enum EstadoPlayer { READY, SHOOTING, MOVING };

    static int velocidadBolasGlobal;

    public Text numBoostersBolaGrande;
    public Text numBoostersBolaFuezaX2;
    public Text numBoostersRebote;

    static bool[] boostersActivados;

    LineRenderer lineRenderer;
    GameObject[] burbujas, bolas, bolasExtra, illumiCoinsExtra;
        
    float timeInicioDisparo;
    int numBolasADisparar, numBolasDisparadas;
    public static EstadoPlayer estadoPlayer;
    bool canShoot;
    float xInicialPlayer;

    // Use this for initialization
    void Start()
    {
        velocidadBolasGlobal = velocidadBolas;
        numBolasADisparar = GetNumBolasFromPlayer();
        lineaDisparo.SetActive(false);
        textNumeroDeBolas.gameObject.SetActive(false);
        ManejadorBolas.SetXPrimeraBola(-9999);
        
        estadoPlayer = EstadoPlayer.READY;
        boostersActivados = new bool[] {false,false,false,false };

        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));
        xInicialPlayer = player.transform.position.x;
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
                    boostersActivados = new bool[] { false, false, false, false };
                    desactivarIconBoosts();
                    if (ManejadorBolas.GetXPrimeraBola() != -9999f) {
                        player.transform.position = new Vector3(ManejadorBolas.GetXPrimeraBola(), player.transform.position.y, player.transform.position.z);
                        textNumeroDeBolas.gameObject.transform.position = new Vector2(player.transform.position.x - 100f/*TODO:ajustar a pantallas*/, textNumeroDeBolas.gameObject.transform.position.y);
                        lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));                      
                        ManejadorBolas.SetXPrimeraBola(-9999f);
                        xInicialPlayer = player.transform.position.x;
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
        if (estadoPlayer == EstadoPlayer.READY && Time.timeScale > 0)
        {
            //...
            textNumeroDeBolas.gameObject.SetActive(true);
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position));
            Vector3 vectorLinea = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);

            //Plotting the line before shooting
            if (vectorLinea.y > 100) //TODO: que sea angulo y altura absoluta
            {  
                lineaDisparo.SetActive(true);
                canShoot = true;
            }
            else
            {
                lineaDisparo.SetActive(false);
                canShoot = false;
            }
        } else if (estadoPlayer == EstadoPlayer.MOVING)
        {
            //Debug.Log("moving!!!!");
            canShoot = false;
            if (Camera.main.ScreenToWorldPoint(eventData.position).x  >= xInicialPlayer-100f
               && Camera.main.ScreenToWorldPoint(eventData.position).x <= xInicialPlayer + 100f)
            {
                player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x,
                                            player.transform.position.y,
                                            player.transform.position.z);
                lineRenderer.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1));
            }
           // Debug.Log("x);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        timeInicioDisparo = Time.time;

        if (canShoot && !hayMasBolas())
        {
            lineaDisparo.SetActive(false);
            estadoPlayer = EstadoPlayer.SHOOTING;
            dispararBola();
        }
    }

    public void boostBolaMasGrande(GameObject iconBoost)
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            int numBoostersSizeBall = int.Parse(numBoostersBolaGrande.text);

            if (boostersActivados[0] == true)
            {
                boostersActivados[0] = false;
                numBoostersSizeBall++;
                iconBoost.SetActive(false);
                numBoostersBolaGrande.text = numBoostersSizeBall.ToString();
            }
            else if (numBoostersSizeBall > 0)
            {
                boostersActivados[0] = true;
                numBoostersSizeBall--;
                iconBoost.SetActive(true);
                numBoostersBolaGrande.text = numBoostersSizeBall.ToString();
            }
        }
    }

    public void boostBolaFuerzaX2(GameObject iconBoost)
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            int numBoostersFuerzaX2 = int.Parse(numBoostersBolaFuezaX2.text);
            if (boostersActivados[1] == true)
            {
                boostersActivados[1] = false;
                numBoostersFuerzaX2++;
                iconBoost.SetActive(false);
                numBoostersBolaFuezaX2.text = numBoostersFuerzaX2.ToString();
            }
            else if (numBoostersFuerzaX2 > 0)
            {
                boostersActivados[1] = true;
                numBoostersFuerzaX2--;
                iconBoost.SetActive(true);
                numBoostersBolaFuezaX2.text = numBoostersFuerzaX2.ToString();
            }
        }
    }

    public void boostBolaRebote(GameObject iconBoost)
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            int numRebotesSuelo = int.Parse(numBoostersRebote.text);
            if (boostersActivados[2] == true)
            {
                boostersActivados[2] = false;
                numRebotesSuelo++;
                iconBoost.SetActive(false);
                numBoostersRebote.text = numRebotesSuelo.ToString();
            }
            else if (numRebotesSuelo > 0)
            {
                boostersActivados[2] = true;
                numRebotesSuelo--;
                iconBoost.SetActive(true);
                numBoostersRebote.text = numRebotesSuelo.ToString();
            }
        }
    }

    bool hayMasBolas()
    {
        bolas = GameObject.FindGameObjectsWithTag("Bola");
        if (bolas != null && bolas.Length > 0)
        {
            return true;
        }
        return false;
    }

    public void generarIllumiCoin()
    {
        //Instantiate a bubble at a random "x,y" position
        int xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
        Vector3 posicion = new Vector3(xRandom, Camera.main.pixelHeight - 1, 90);
        posicion = Camera.main.ScreenToWorldPoint(posicion);
        posicion.z = 90;

        Transform coinExtraNew = Instantiate(illumiCoinExtra, posicion, Quaternion.identity);
        coinExtraNew.gameObject.tag = "IllumiCoinExtra";
    }

    public void generarBolaExtra()
    {
        //Instantiate a bubble at a random "x" position
        int xRandom = (int)Random.Range(0+ Camera.main.pixelWidth / 10, Camera.main.pixelWidth-(Camera.main.pixelWidth/10)); 

        Vector3 randomPos = new Vector3(xRandom, Camera.main.pixelHeight-1, 90);

        Vector3 posicion = Camera.main.ScreenToWorldPoint(randomPos);
        posicion.z = 90;

        Transform bolaExtraNew = Instantiate(bolaExtra, posicion, Quaternion.identity);
        bolaExtraNew.gameObject.tag = "BolaExtra";
    }

    void generarBurbujas(int score)
    {
        int numBurbujas = 2;

        if (score % 5 == 0 || score % 11 == 0 /*|| score % 17 == 0*/)
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
        int xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
        Vector3 posicion = new Vector3(xRandom, Camera.main.pixelHeight - 1, 1);
                posicion = Camera.main.ScreenToWorldPoint(posicion);
                posicion.z = 90;

        Transform burbujaNueva = Instantiate(burbuja, posicion, Quaternion.identity);
        burbujaNueva.gameObject.tag = "Burbuja";

        int pesoDeseado = numBolasADisparar * 2; //TODO: función que también tenga en cuenta la puntuación 
        
        foreach (Transform b in burbujaNueva.gameObject.gameObject.transform)
        {
            if (b.gameObject.gameObject.name == "Peso")
            {
                b.gameObject.gameObject.GetComponent<TextMesh>().text = pesoDeseado.ToString();
            }
        }
    }

    public void moverBurbujas()
    {
        burbujas = GameObject.FindGameObjectsWithTag("Burbuja");
        foreach (GameObject burbuja in burbujas)
        {
            //Moving bubbles one "step" to the floor
            burbuja.GetComponent<Rigidbody2D>().MovePosition(
                                            new Vector3(burbuja.transform.position.x,
                                                        burbuja.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                        burbuja.transform.position.z));
        }
    }

    public void moverBolasExtra()
    {
        bolasExtra = GameObject.FindGameObjectsWithTag("BolaExtra");
        foreach (GameObject bolaAux in bolasExtra)
        {
            //Moving bubbles one "step" to the floor
            bolaAux.GetComponent<Rigidbody2D>().MovePosition(
                                            new Vector3(bolaAux.transform.position.x,
                                                        bolaAux.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                        bolaAux.transform.position.z));

        }
    }
    public void moverIllumiCoins()
    {
        illumiCoinsExtra = GameObject.FindGameObjectsWithTag("IllumiCoinExtra");
        foreach (GameObject coin in illumiCoinsExtra)
        {
            //Moving bubbles one "step" to the floor
            coin.GetComponent<Rigidbody2D>().MovePosition(new Vector3(coin.transform.position.x,
                                                        coin.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                        coin.transform.position.z));
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
        
        textScore.text = "Score: " + (GetScore()+1).ToString();
    }

    public void dispararBola()
    {        
        Transform bolaAux = Instantiate(bola, player.transform.position, Quaternion.identity);
        Vector3 vAux = lineRenderer.GetPosition(1) - player.transform.position;
        bolaAux.GetComponent<Rigidbody2D>().velocity = vAux.normalized * velocidadBolas; 
        bolaAux.gameObject.tag = "Bola";
        if (boostersActivados[0] == true)
        {
            bolaAux.gameObject.transform.localScale = bolaAux.gameObject.transform.localScale * 4;               
        }
        numBolasDisparadas++;
    }

    void desactivarIconBoosts()
    {
        GameObject[] iconBoosts = GameObject.FindGameObjectsWithTag("IconBoost");
        foreach (GameObject icon in iconBoosts)
        {
            icon.SetActive(false);
        }
    }

    public static int getVelocidadBolas()
    {
        return velocidadBolasGlobal;
    }

    public static bool[] getBoostersActivados()
    {
        if (boostersActivados == null)
        {
            return new bool[] { false, false, false, false };
        }
        return boostersActivados;
    }

    public static void setEstadoPlayer(EstadoPlayer estado)
    {
        estadoPlayer = estado;
    }

    public static EstadoPlayer getEstadoPlayer(EstadoPlayer estado)
    {
        return estadoPlayer;
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
