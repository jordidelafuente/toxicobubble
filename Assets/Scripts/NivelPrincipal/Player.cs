using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject panelMoving;
    Animator animPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonMovePlayer()
    {
        animPlayer = ManejadorDisparo.getAnimPlayer();
        if (ManejadorDisparo.estadoPlayer == ManejadorDisparo.EstadoPlayer.READY)
        {
            //ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.MOVING;
            ManejadorDisparo.setEstadoPlayer(ManejadorDisparo.EstadoPlayer.MOVING);
            panelMoving.SetActive(true);
        } else
        {
            //ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
            ManejadorDisparo.setEstadoPlayer(ManejadorDisparo.EstadoPlayer.READY);
            panelMoving.SetActive(false);
            //animPlayer.SetTrigger("");
        }
        animPlayer.SetTrigger("PlayerReady");
    }
}
