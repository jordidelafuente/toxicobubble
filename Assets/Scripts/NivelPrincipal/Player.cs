using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        } else
        {
            ManejadorDisparo.estadoPlayer = ManejadorDisparo.EstadoPlayer.READY;
        }
        
    }
}
