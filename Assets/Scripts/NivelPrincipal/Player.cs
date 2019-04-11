using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject panelMoving;

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
        if (ManejadorDisparo.estadoPlayer == ManejadorDisparo.EstadoPlayer.READY)
        {
            ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.MOVING;
            panelMoving.SetActive(true);
        } else
        {
            ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
            panelMoving.SetActive(false);
        }
        
    }
}
