using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManejadorDisparo : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
                         IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public GameObject lineaDisparo;
    public GameObject player;
    //public Scrollbar scrollBar;
    public GameObject handleScrollBar;
    public Transform bola;
    public Transform bolaExtra;
    public Transform illumiCoinExtra;
    public Transform burbuja;
    public Transform calavera;
    public Transform bolaSpinner;

    public Text textNumeroDeBolas;
    public Text textScore, textBestScore, illumiCoins;
    public int velocidadBolas;
    public enum EstadoPlayer { READY, SHOOTING, MOVING, POINTING, GAMEOVER, TALKING };
    static Animator animPlayer;

    static int velocidadBolasGlobal;

    public Text numBoostersBolaGrande;
    public Text numBoostersBolaFuezaX2;
    public Text numBoostersRebote;

    static bool[] boostersActivados;

    public AudioSource source;
    public AudioSource musicSource;

    public AudioClip sound_start;
    public AudioClip sound_using_booster;
    public AudioClip sound_not_using_booster;

    public AudioClip playingMusic;
    public AudioClip gameOverMusic;
    public AudioClip enemyTalkingMusic;
    public AudioClip levelPassed;


    LineRenderer lineRenderer;
    GameObject[] burbujas, bolas, bolasExtra, illumiCoinsExtra, calaveras, bolasSpinner;

    public static DataController dataController;

    float timeInicioDisparo, timeCambioPosDisparo;
    public static int numBolasADisparar, numBolasDisparadas;
    public static EstadoPlayer estadoPlayer;
    bool canShoot;
    public static bool gameOverActivated, changeMusic;
    private static bool isDragging;
    float xInicialPlayer;
    public static int lastStateBeforeGameOver;
      
    private ArrayList listaXsObjetosNuevos = new ArrayList();

    public GameObject panel_history_1;
    public GameObject panel_history_12;
    public GameObject panel_history_2;
    public GameObject panel_history_22;
    public GameObject panel_history_3;
    public GameObject panel_history_32;

    // Use this for initialization
    void Start()
    {
        dataController = getDataController();

        updateScore();
        textScore.text = "Score: " + 0;

        velocidadBolasGlobal = velocidadBolas;
        numBolasADisparar = GetNumBolasFromPlayer(2);
        lineaDisparo.SetActive(false);
        //textNumeroDeBolas.gameObject.SetActive(false);
        ManejadorBolas.SetXPrimeraBola(-9999);
        animPlayer = player.GetComponent<Animator>();
        
        estadoPlayer = EstadoPlayer.READY;
        boostersActivados = new bool[] {false,false,false,false };

        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(player.gameObject.transform.position.x,
                                    player.gameObject.transform.position.y,
                                    player.transform.position.z - 1));

        /*scrollBar.transform.position = new Vector3(player.gameObject.transform.position.x,
                                                   scrollBar.transform.position.y,
                                                   scrollBar.transform.position.z);
        scrollBar.gameObject.SetActive(true);
        scrollBar.value = 0.5f;*/
        xInicialPlayer = player.transform.position.x;

        //Controlling the history mode
        historyMode();
        playMusic();
    }

    public void historyMode()
    {
        panel_history_1.SetActive(false); //TODO: extern array of panels?
        panel_history_2.SetActive(false); //
        panel_history_3.SetActive(false); //

        switch (dataController.getPlayerProgress().stateHistory)
        {
            case 0:
                panel_history_1.SetActive(true); //
                estadoPlayer = EstadoPlayer.TALKING;
                Time.timeScale = 0;
                break;

            case 1: //TODO: hacer una estructura de datos que contenga relaciones de estados en modo historia (paneles vs estado, modo juego vs estados, estados vs rewards, etc...
            case 3:
            case 5: 
                estadoPlayer = EstadoPlayer.READY; //ready to continue playing
                Time.timeScale = 1;
                break;

            case 2:
                //panel_history_1.SetActive(false);
                panel_history_2.SetActive(true); //
                estadoPlayer = EstadoPlayer.TALKING;
                Time.timeScale = 0;
                break;

            case 4:
                panel_history_3.SetActive(true); //
                estadoPlayer = EstadoPlayer.TALKING;
                Time.timeScale = 0;
                break;
        }
    }

    public void switchPanel_1_to_12()
    {
        panel_history_1.SetActive(false); //
        panel_history_12.SetActive(true); //
    }

    public void switchPanel_2_to_22()
    {
        panel_history_2.SetActive(false); //
        panel_history_22.SetActive(true); //
    }

    public void switchPanel_3_to_32()
    {
        panel_history_3.SetActive(false); //
        panel_history_32.SetActive(true); //
    }

    public void startPlaying(int state) //from button "continue"
    {
        dataController.getPlayerProgress().stateHistory = state;
        lastStateBeforeGameOver = state;
        panel_history_1.SetActive(false); // TODO: extern array?
        panel_history_12.SetActive(false); // TODO: extern array?
        panel_history_2.SetActive(false); //
        panel_history_22.SetActive(false);
        panel_history_3.SetActive(false); //
        panel_history_32.SetActive(false);

        estadoPlayer = EstadoPlayer.READY;
        Time.timeScale = 1;
        playMusic();
        if (dataController.getOptionsConfig().soundsOn == 0) //TODO: constantes
        {
            source.PlayOneShot(sound_start, 1f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        //animPlayer.SetTrigger("lalala");
        bool isDispararBola = estadoPlayer == EstadoPlayer.SHOOTING
                            && numBolasDisparadas < numBolasADisparar
                            && (Time.time - timeInicioDisparo > 0.1 * numBolasDisparadas);
        //scrollBar.gameObject.SetActive(estadoPlayer == EstadoPlayer.READY || estadoPlayer == EstadoPlayer.POINTING);

        if (isDispararBola)
        {
            dispararBola();
            
        } else if (estadoPlayer == EstadoPlayer.GAMEOVER && !gameOverActivated)
        {
            lastStateBeforeGameOver = getDataController().getPlayerProgress().stateHistory;
            getDataController().getPlayerProgress().stateHistory = -1;
            historyMode();
            playMusic();
            gameOverActivated = true;
            //Debug.Log("game overrrrrr");
        }
        else
        {
            if (changeMusic) //change music due to avoiding game over ($$$)
            {
                playMusic();
                changeMusic = false;
            }
            
            if (!hayMasBolas() && numBolasDisparadas == numBolasADisparar)
            {
                estadoPlayer = EstadoPlayer.READY;
                isDragging = false;
                animPlayer.ResetTrigger("pointUp");
                animPlayer.SetTrigger("PlayerReady");
                listaXsObjetosNuevos.Clear();
                generarBurbujas(GetScore());
                if ((GetScore()) % 7 == 0)
                {
                    generarIllumiCoin();
                }

                if ((GetScore()) % 5 == 0 ) //Every x turns we create a new extra ball
                { 
                    generarBolaExtra();
                }

                //If state hostory is 3, then you are suffering the skull weapon
                if (dataController.getPlayerProgress().stateHistory >= 3 && (GetScore()) % 6 == 0)
                {
                    generarCalavera();
                }

                if (dataController.getPlayerProgress().stateHistory >= 5 && (GetScore()) % 7 == 0)
                {
                    generarBolaSpinner();
                }

                moverBurbujas();
                moverBolasExtra();
                moverIllumiCoins();
                moverCalaveras();
                moverBolasSpinner();
                updateScore();

                numBolasADisparar = GetNumBolasFromPlayer(numBolasADisparar);

                numBolasDisparadas = 0;
                boostersActivados = new bool[] { false, false, false, false };
                desactivarIconBoosts();
                if (ManejadorBolas.GetXPrimeraBola() != -9999f) {
                    float percentagePos = 1.1f; //10%
                    int maxX = Camera.main.pixelWidth + (Camera.main.pixelWidth - Mathf.RoundToInt(Camera.main.pixelWidth / percentagePos));
                    int minX = -10 + Camera.main.pixelWidth - Mathf.RoundToInt(Camera.main.pixelWidth / percentagePos);

                    if (ManejadorBolas.GetXPrimeraBola() > maxX)
                    {
                        player.transform.position = new Vector3(maxX, player.transform.position.y, player.transform.position.z);
                    }
                    else if (ManejadorBolas.GetXPrimeraBola() < minX)
                    {
                        player.transform.position = new Vector3(minX, player.transform.position.y, player.transform.position.z);
                    }
                    else {
                        player.transform.position = new Vector3(ManejadorBolas.GetXPrimeraBola(), player.transform.position.y, player.transform.position.z);
                    }

                    /*scrollBar.transform.position = new Vector3(player.gameObject.transform.position.x,
                                                   scrollBar.transform.position.y,
                                                   scrollBar.transform.position.z);
                    Player.isManualMove = false;
                    scrollBar.value = 0.5f;*/
                    //textNumeroDeBolas.gameObject.transform.position = new Vector2(player.transform.position.x - 100f/*TODO:ajustar a pantallas*/, textNumeroDeBolas.gameObject.transform.position.y);
                    lineRenderer.SetPosition(0, new Vector3(player.gameObject.transform.position.x /*+ (80)*/,
                                    player.gameObject.transform.position.y,
                                    player.transform.position.z - 1)); 
                    ManejadorBolas.SetXPrimeraBola(-9999f);
                    xInicialPlayer = player.transform.position.x;
                    Player.isManualMove = false;
                    timeCambioPosDisparo = Time.time;

                }

                //Achieving first goal
                if (dataController.getPlayerProgress().stateHistory == 1 
                    && GetScore() >= 25)/*TODO: 50*/
                {
                    lastStateBeforeGameOver = getDataController().getPlayerProgress().stateHistory;
                    dataController.getPlayerProgress().stateHistory = 2;
                    int newNumCoinsPlayer = int.Parse(illumiCoins.text.ToString()) + 10;
                    illumiCoins.text = newNumCoinsPlayer.ToString();
                    historyMode();
                    playMusic();
                }

                //Achieving second goal
                if (dataController.getPlayerProgress().stateHistory == 3
                    && GetScore() >= 60)/*TODO: 110*/
                {
                    lastStateBeforeGameOver = getDataController().getPlayerProgress().stateHistory;
                    dataController.getPlayerProgress().stateHistory = 4;
                    int newNumCoinsPlayer = int.Parse(illumiCoins.text.ToString()) + 11;
                    illumiCoins.text = newNumCoinsPlayer.ToString();
                    historyMode();
                    playMusic();
                }
            }
            else
            {
                if ((estadoPlayer != EstadoPlayer.SHOOTING) && !isDragging)
                {
                    //animPlayer.SetTrigger("PlayerReady");
                    
                    if (Time.time - timeCambioPosDisparo > 0.1)
                    {
                        Player.isManualMove = true;
                        //animPlayer.SetTrigger("PlayerReady");
                    }
                }
            }
        }      
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (estadoPlayer == EstadoPlayer.READY)
        {
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position));
            isDragging = true;
        }        
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && Time.timeScale > 0 && (estadoPlayer != EstadoPlayer.SHOOTING) && (estadoPlayer != EstadoPlayer.GAMEOVER))
        {
            //...
            //textNumeroDeBolas.gameObject.SetActive(true);
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position));
            Vector3 vectorLinea = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);

            //Plotting the line before shooting
            if (vectorLinea.y > 100) //TODO: que sea angulo y no altura absoluta
            {  
                lineaDisparo.SetActive(true);
                canShoot = true;

                if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("AnimationPlayerReady")) {
                    animPlayer.ResetTrigger("PlayerReady");
                    animPlayer.SetTrigger("pointUp"); //TODO: diferente animacion segun angulo
                }                    
                //animPlayer.GetComponent<Animation>().Stop();
                //Time.timeScale = 0;
                estadoPlayer = EstadoPlayer.POINTING;
            }
            else
            {
                lineaDisparo.SetActive(false);
                canShoot = false;
                //animPlayer.SetTrigger("PlayerReady");
                estadoPlayer = EstadoPlayer.READY;
            }
        }
        else
        {
            //animPlayer.SetTrigger("PlayerReady");
            Debug.Log("kepasaaa");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        timeInicioDisparo = Time.time;
        isDragging = false;
        if (canShoot && !hayMasBolas() && (estadoPlayer == EstadoPlayer.READY || estadoPlayer == EstadoPlayer.POINTING))//estadoPlayer != EstadoPlayer.GAMEOVER)
        {
            lineaDisparo.SetActive(false);
            estadoPlayer = EstadoPlayer.SHOOTING;
            numBolasDisparadas = 0;
            dispararBola();
        }
        
        eventData.pointerDrag = null;
        eventData.dragging = false;
        animPlayer = player.GetComponent<Animator>();
        //animPlayer.SetTrigger("PlayerReady");
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

                if (dataController.getOptionsConfig().soundsOn == 0) //TODO: constantes
                {
                    source.PlayOneShot(sound_not_using_booster, 1f);
                }

                numBoostersBolaGrande.text = numBoostersSizeBall.ToString();
            }
            else if (numBoostersSizeBall > 0)
            {
                boostersActivados[0] = true;
                numBoostersSizeBall--;
                iconBoost.SetActive(true);

                if (dataController.getOptionsConfig().soundsOn == 0) //TODO: constantes
                {
                    source.PlayOneShot(sound_using_booster, 1f);
                }

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

                if (dataController.getOptionsConfig().soundsOn == 0) //TODO: constantes
                {
                    source.PlayOneShot(sound_not_using_booster, 1f);
                }

                numBoostersBolaFuezaX2.text = numBoostersFuerzaX2.ToString();
            }
            else if (numBoostersFuerzaX2 > 0)
            {
                boostersActivados[1] = true;
                numBoostersFuerzaX2--;
                iconBoost.SetActive(true);

                if (dataController.getOptionsConfig().soundsOn == 0) //TODO: constantes
                {
                    source.PlayOneShot(sound_using_booster, 1f);
                }

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

                if (dataController.getOptionsConfig().soundsOn == 0) //TODO: constantes
                {
                    source.PlayOneShot(sound_not_using_booster, 1f);
                }

                numBoostersRebote.text = numRebotesSuelo.ToString();

            }
            else if (numRebotesSuelo > 0)
            {
                boostersActivados[2] = true;
                numRebotesSuelo--;
                iconBoost.SetActive(true);

                if (dataController.getOptionsConfig().soundsOn == 0) //TODO: constantes
                {
                    source.PlayOneShot(sound_using_booster, 1f);
                }               

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

        for (int i = 0; i < 2000; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            posicion = new Vector3(xRandom, Camera.main.pixelHeight - 1, 90);
            posicion = Camera.main.ScreenToWorldPoint(posicion);
            posicion.z = 91;
            if (!isColisionConObjetosDelJuego(posicion))
            {
                listaXsObjetosNuevos.Add(xRandom);
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
        for (int i=0; i < 2000; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            Vector3 randomPos = new Vector3(xRandom, Camera.main.pixelHeight - 1, 90);
            posicion = Camera.main.ScreenToWorldPoint(randomPos);
            posicion.z = 91;
            if (!isColisionConObjetosDelJuego(posicion))
            {
                listaXsObjetosNuevos.Add(xRandom);
                break;
            }
        }        

        Transform bolaExtraNew = Instantiate(bolaExtra, posicion, Quaternion.identity);
        bolaExtraNew.gameObject.tag = "BolaExtra";
    }

    public void generarCalavera() //Instantiate a bubble at a random "x" position
    {
        //bool positionValid = false;
        int xRandom = -9999;
        Vector3 posicion = new Vector3(-9999, -9999, 9999);
        for (int i = 0; i < 2000; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            Vector3 randomPos = new Vector3(xRandom, Camera.main.pixelHeight - 1, 90);
            posicion = Camera.main.ScreenToWorldPoint(randomPos);
            posicion.z = 91;
            if (!isColisionConObjetosDelJuego(posicion))
            {
                listaXsObjetosNuevos.Add(xRandom);
                break;
            }
        }

        Transform calaveraNew = Instantiate(calavera, posicion, Quaternion.identity);
        calaveraNew.gameObject.tag = "Calavera";

        foreach (Transform b in calaveraNew.gameObject.gameObject.transform)
        {
            if (b.gameObject.gameObject.name == "Peso")
            {
                int peso = int.Parse(b.gameObject.gameObject.GetComponent<TextMesh>().text) + (dataController.getPlayerProgress().stateHistory - 3);
                b.gameObject.gameObject.GetComponent<TextMesh>().text = peso.ToString();
            }
        }
    }

    void generarBolaSpinner()
    {
        //bool positionValid = false;
        int xRandom = -9999;
        Vector3 posicion = new Vector3(-9999, -9999, 9999);
        for (int i = 0; i < 2000; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            Vector3 randomPos = new Vector3(xRandom, Camera.main.pixelHeight - 1, 90);
            posicion = Camera.main.ScreenToWorldPoint(randomPos);
            posicion.z = 91;
            if (!isColisionConObjetosDelJuego(posicion))
            {
                listaXsObjetosNuevos.Add(xRandom);
                break;
            }
        }

        Transform bolaSpinnerNew = Instantiate(bolaSpinner, posicion, Quaternion.identity);
        bolaSpinnerNew.gameObject.tag = "BolaSpinner";
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
        for (int i = 0; i < 2000; i++)
        {
            xRandom = (int)Random.Range(0 + Camera.main.pixelWidth / 10, Camera.main.pixelWidth - (Camera.main.pixelWidth / 10));
            posicion = new Vector3(xRandom, Camera.main.pixelHeight - 1, 1);
            posicion = Camera.main.ScreenToWorldPoint(posicion);
            posicion.z = 91;
          
            if (!isColisionConObjetosDelJuego(posicion) /*&& Mathf.Abs(xRandom - xBurbujaAnt) > 155*/)
            {
                listaXsObjetosNuevos.Add(xRandom);
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
        /*burbujas = GameObject.FindGameObjectsWithTag("Burbuja");
        foreach (GameObject burbuja in burbujas)
        {
            float dist = Vector3.Distance(burbuja.transform.position, posicionNuevoObjeto);
            if (dist < 155) //I don't like this number, but essay & error gave me
            {
                return true;
            }
        }

        calaveras = GameObject.FindGameObjectsWithTag("Calavera");
        foreach (GameObject skull in calaveras)
        {
            float dist = Vector3.Distance(skull.transform.position, posicionNuevoObjeto);
            if (dist < 155) //I don't like this number, but essay & error gave me
            {
                return true;
            }
        }

        bolasSpinner = GameObject.FindGameObjectsWithTag("BolaSpinner");
        foreach (GameObject bola in bolasSpinner)
        {
            float dist = Vector3.Distance(bola.transform.position, posicionNuevoObjeto);
            if (dist < 155) //I don't like this number, but essay & error gave me
            {
                return true;
            }
        }

        bolasExtra = GameObject.FindGameObjectsWithTag("BolaExtra");
        foreach (GameObject bola in bolasExtra)
        {
            float dist = Vector3.Distance(bola.transform.position, posicionNuevoObjeto);
            if (dist < 100)
            {
                return true;
            }
        }

        illumiCoinsExtra = GameObject.FindGameObjectsWithTag("IllumiCoinExtra");
        foreach (GameObject coin in illumiCoinsExtra)
        {
            float dist = Vector3.Distance(coin.transform.position, posicionNuevoObjeto);
            if (dist < 100)
            {
                return true;
            }
        }*/

        foreach(int xObjAnt in listaXsObjetosNuevos)
        {
            Vector3 posicionObjAnt = new Vector3(xObjAnt, posicionNuevoObjeto.y, posicionNuevoObjeto.z);
            float dist = Vector3.Distance(posicionObjAnt, posicionNuevoObjeto);
            if (dist < 150)
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

    public void moverCalaveras()
    {
        calaveras = GameObject.FindGameObjectsWithTag("Calavera");
        foreach (GameObject skull in calaveras)
        {
            //Moving objects one "step" to the floor
            skull.GetComponent<Rigidbody2D>().MovePosition(new Vector3(skull.transform.position.x,
                                                        skull.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                        skull.transform.position.z));
        }
    }

    public void moverBolasSpinner()
    {
        bolasSpinner = GameObject.FindGameObjectsWithTag("BolaSpinner");
        foreach (GameObject spinner in bolasSpinner)
        {
            //Moving objects one "step" to the floor
            spinner.GetComponent<Rigidbody2D>().MovePosition(new Vector3(spinner.transform.position.x,
                                                        spinner.transform.position.y - 100, //TODO: adaptar a diferentes plataformas y resoluciones
                                                        spinner.transform.position.z));
        }
    }

    //Getting the number of balls we will shoot 
    private int GetNumBolasFromPlayer(int numAnt)
    {
        int numBolas = 0;
        //Text[] textos = //player.GetComponentsInChildren<Text>();

        /*if (estadoPlayer != EstadoPlayer.SHOOTING)
        {*/
        try
        {
            string strNumBolas = textNumeroDeBolas.text.ToString().Replace("x", ""); //TODO: que pasa si hay más de un componente texto?
            numBolas = int.Parse(strNumBolas);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString()); //?
            numBolas = numAnt; 
        }
            
        /*} */       

        return numBolas;
    }

    int GetScore()
    {
        return int.Parse(textScore.text.ToString().Replace("Score: ", ""));
    }

    void updateScore()
    {
        
        textScore.text = "Score: " + (GetScore()+1).ToString();
        dataController.submitNewPlayerScore(GetScore());

        textBestScore.text = "Best: " + dataController.getBestScore().ToString();
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

    public static DataController getDataController()
    {
        if (dataController == null)
        {
            dataController = new DataController();
            dataController.loadOptionsConfig();
            dataController.loadPlayerProgress();
        }

        return dataController;
    }

    public void playMusic()
    {
        musicSource.Stop();

        if (dataController.getOptionsConfig().musicOn == 0)
        {            
            switch (dataController.getPlayerProgress().stateHistory)
            {
                case 0: //starting history
                case 2:
                case 4:
                    //musicSource.PlayOneShot(enemyTalkingMusic, 0.5f);
                    musicSource.clip = enemyTalkingMusic;
                    break;
                case 1: //playing
                case 3:
                case 5:
                    musicSource.clip = playingMusic;
                    break;
                case -1: //Game over
                    musicSource.clip = gameOverMusic;
                    break;
                //case 3: //Congratulations!
                //    break;
            }
            musicSource.loop = true;
            musicSource.Play();
        } else {
            musicSource.Stop();
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
