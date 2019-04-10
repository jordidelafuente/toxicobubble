using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurbujaFondo : MonoBehaviour
{
    float timeCreacion;

    // Start is called before the first frame update
    void Start()
    {
        timeCreacion = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tag == "BurbujaFondo" && Time.time - timeCreacion > 15)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }   
    }
}
