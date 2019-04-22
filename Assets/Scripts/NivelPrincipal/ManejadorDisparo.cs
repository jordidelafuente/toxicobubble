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
    public enum EstadoPlayer { READY, SHOOTING, MOVING, GAMEOVER };
    static Animator animPlayer;

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
    Vector2 _lastPosition;

    // Use this for initialization
    void Start()
    {
        velocidadBolasGlobal = velocidadBolas;
        numBolasADisparar = GetNumBolasFromPlayer();
        lineaDisparo.SetActive(false);
        textNumeroDeBolas.gameObject.SetActive(false);
        ManejadorBolas.SetXPrimeraBola(-9999);
        animPlayer = player.GetComponent<Animator>();
        
        estadoPlayer = EstadoPlayer.READY;
        boostersActivados = new bool[] {false,false,false,false };

        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(player.gameObject.transform.position.x /*+ (80*/,
                                    player.gameObject.transform.position.y/* - (50)*/,
                                    player.transform.position.z - 1)); //TODO: ajustar mejor
        xInicialPlayer = player.transform.position.x;
        //xInstanteAnterior = player.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        bool isDispararBola = estadoPlayer == EstadoPlayer.SHOOTING
                            && numBolasDisparadas < numBolasADisparar
                            && (Time.time - timeInicioDisparo > 0.1 * numBolasDisparadas);

        if (isDispararBola)
        {
            dispararBola();
        }
        else
        {
            transicionarAnimPlayer();
            if (!hayMasBolas() && numBolasDisparadas == numBolasADisparar)
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
                transicionarAnimPlayer();
                numBolasADisparar = GetNumBolasFromPlayer();
                textNumeroDeBolas.gameObject.SetActive(false);
                numBolasDisparadas = 0;
                boostersActivados = new bool[] { false, false, false, false };
                desactivarIconBoosts();
                if (ManejadorBolas.GetXPrimeraBola() != -9999f) {
                    player.transform.position = new Vector3(ManejadorBolas.GetXPrimeraBola(), player.transform.position.y, player.transform.position.z);
                    textNumeroDeBolas.gameObject.transform.position = new Vector2(player.transform.position.x - 100f/*TODO:ajustar a pantallas*/, textNumeroDeBolas.gameObject.transform.position.y);
                    lineRenderer.SetPosition(0, new Vector3(player.gameObject.transform.position.x /*+ (80)*/,
                                    player.gameObject.transform.position.y /*- (50)*/,
                                    player.transform.position.z - 1)); //TODO: ajustar mejor
                    ManejadorBolas.SetXPrimeraBola(-9999f);
                    xInicialPlayer = player.transform.position.x;
                    //xInstanteAnterior = player.transform.position.x;
                }
            }
        }      
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position));
        }

        _lastPosition = eventData.position;
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
            if (vectorLinea.y > 100) //TODO: que sea angulo y no altura absoluta
            {  
                lineaDisparo.SetActive(true);
                canShoot = true;
            }
            else
            {
                lineaDisparo.SetActive(false);
                canShoot = false;
            }

            //Sprite sprite = Resources.Load("soldier-sprites-left_48", typeof(Sprite)) as Sprite;
            //player.GetComponent<SpriteRenderer>().sprite = null; // sprite;
            //player.GetComponent<SpriteRenderer>().size = new Vector2(100,100);
            //player.GetComponent<Animator>().set;
            //player.GetComponent<SpriteRenderer>().sprite.

        } else if (estadoPlayer == EstadoPlayer.MOVING)
        {
            //Debug.Log("moving!!!!");
            canShoot = false;
            if (Camera.main.ScreenToWorldPoint(eventData.position).x  >= xInicialPlayer-100f
               && Camera.main.ScreenToWorldPoint(eventData.position).x <= xInicialPlayer + 100f)
            {
                Vector3 direction = eventData.position - _lastPosition;
                _lastPosition = eventData.position;
                player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x,
                                            player.transform.position.y,
                                            player.transform.position.z);
                
                lineRenderer.SetPosition(0, new Vector3(player.gameObject.transform.position.x ,
                                    player.gameObject.transform.position.y,
                                    player.transform.position.z - 1)); 

                //Deciding the animation to run due to dragging direction when moving               
                if (direction.x > 0)
                {
                    animPlayer.SetTrigger("ReadyToMoveRight");
                    animPlayer.SetTrigger("PlayerReady");
                } else if(direction.x < 0)
                {
                    animPlayer.SetTrigger("ReadyToMoveLeft");
                    animPlayer.SetTrigger("PlayerReady");
                } else
                {
                    animPlayer.SetTrigger("PlayerReady");
                }
            }
            // Debug.Log("x);
        } else
        {
            animPlayer.SetTrigger("PlayerReady");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        timeInicioDisparo = Time.time;

        if (canShoot && !hayMasBolas() && estadoPlayer != EstadoPlayer.GAMEOVER)
        {
            lineaDisparo.SetActive(false);
            estadoPlayer = EstadoPlayer.SHOOTING;
            numBolasDisparadas = 0;
            dispararBola();
        }
        //xInstanteAnterior = player.transform.position.x;
        transicionarAnimPlayer();
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

    public void generarIllumiCoin() //Instantiate a bubble at a random "x,y" position
    {
        
        //bool positionValid = false;
        int xRandom = -9999;
        Vector3 posicion = new Vector3(-9999, -9999, 9999);

        for (int i = 0; i < 100; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            posicion = new Vector3(xRandom, Camera.main.pixelHeight - 1, 90);
            posicion = Camera.main.ScreenToWorldPoint(posicion);
            posicion.z = 91;
            if (!isColisionConObjetosDelJuego(posicion))
            {
               break;
            }
        }        

        Transform coinExtraNew = Instantiate(illumiCoinExtra, posicion, Quaternion.identity);
        coinExtraNew.gameObject.tag = "IllumiCoinExtra";
    }

    public void generarBolaExtra() //Instantiate a bubble at a random "x" position
    {
        //bool positionValid = false;
        int xRandom = -9999;
        Vector3 posicion = new Vector3(-9999, -9999, 9999);
        for (int i=0; i<100; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            Vector3 randomPos = new Vector3(xRandom, Camera.main.pixelHeight - 1, 90);
            posicion = Camera.main.ScreenToWorldPoint(randomPos);
            posicion.z = 91;
            if (!isColisionConObjetosDelJuego(posicion))
            {
                break;
            }
        }        

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

    public void generarBurbuja() //Instantiate a bubble at a random "x" position
    {
        //bool positionValid = false;
        int xRandom = -9999;
        Vector3 posicion = new Vector3 (-9999,-9999,9999);
        for (int i = 0; i < 100; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            posicion = new Vector3(xRandom, Camera.main.pixelHeight - 1, 1);
            posicion = Camera.main.ScreenToWorldPoint(posicion);
            posicion.z = 91;
            if (!isColisionConObjetosDelJuego(posicion))
            {
                break;
            }
        }

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

    public bool isColisionConObjetosDelJuego(Vector3 posicionNuevoObjeto)
    {
        burbujas = GameObject.FindGameObjectsWithTag("Burbuja");
        foreach (GameObject burbuja in burbujas)
        {
            float dist = Vector3.Distance(burbuja.transform.position, posicionNuevoObjeto);
            if (dist < 150)
            {
                return true;
            }
        }

        bolasExtra = GameObject.FindGameObjectsWithTag("BolaExtra");
        foreach (GameObject bola in bolasExtra)
        {
            float dist = Vector3.Distance(bola.transform.position, posicionNuevoObjeto);
            if (dist < 75)
            {
                return true;
            }
        }

        illumiCoinsExtra = GameObject.FindGameObjectsWithTag("IllumiCoinExtra");
        foreach (GameObject coin in illumiCoinsExtra)
        {
            float dist = Vector3.Distance(coin.transform.position, posicionNuevoObjeto);
            if (dist < 75)
            {
                return true;
            }
        }

        return false;
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
    {   //...
        Transform bolaAux = Instantiate(bola, lineRenderer.GetPosition(0), Quaternion.identity);
        Vector3 vAux = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
        bolaAux.GetComponent<Rigidbody2D>().velocity = vAux.normalized * velocidadBolas; 
        bolaAux.gameObject.tag = "Bola";
        if (boostersActivados[0] == true)
        {
            bolaAux.gameObject.transform.localScale = bolaAux.gameObject.transform.localScale * 3;               
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
        boostersActivados = new bool[] { false, false, false, false };
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

    public static Animator getAnimPlayer()
    {
        return animPlayer;
    }

    void transicionarAnimPlayer()
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            animPlayer.SetTrigger("PlayerReady");
        }
        else if (estadoPlayer == EstadoPlayer.MOVING)
        {
            animPlayer.SetTrigger("PlayerReady");
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
