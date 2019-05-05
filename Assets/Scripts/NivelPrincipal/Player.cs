using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject player;
    public GameObject handleScrollBar;
   
    public GameObject lineaDisparo;
    Animator animPlayer;
    LineRenderer lineRenderer;

    float positionXPlayerAnt;
    public static bool isManualMove;

    // Start is called before the first frame update
    void Start()
    {
        animPlayer = ManejadorDisparo.getAnimPlayer();
        lineRenderer = lineaDisparo.GetComponent<LineRenderer>();
        positionXPlayerAnt = player.transform.position.x;

        isManualMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movePlayer()
    {
        
        if (!isManualMove)
        {
            return;
        }

        animPlayer = ManejadorDisparo.getAnimPlayer();
        player.transform.position = new Vector3(handleScrollBar.transform.position.x, 
                                                        player.transform.position.y, 
                                                        player.transform.position.z);

        lineRenderer.SetPosition(0, new Vector3(player.gameObject.transform.position.x,
                                    player.gameObject.transform.position.y,
                                    player.transform.position.z - 1));

        //Deciding the animation to run due to dragging direction when moving  
        float direction = handleScrollBar.transform.position.x - positionXPlayerAnt;
        if (direction > 0)
        {
            animPlayer.SetTrigger("ReadyToMoveRight");
            //animPlayer.SetTrigger("PlayerReady");
        }
        else if (direction < 0)
        {
            animPlayer.SetTrigger("ReadyToMoveLeft");
            //animPlayer.SetTrigger("PlayerReady");
        }
        /*else
        {
            animPlayer.SetTrigger("PlayerReady");
        }*/

        positionXPlayerAnt = player.transform.position.x;
        //Debug.Log("MOVING BAR!!!");
        
        animPlayer.SetTrigger("PlayerReady");
    }
}
