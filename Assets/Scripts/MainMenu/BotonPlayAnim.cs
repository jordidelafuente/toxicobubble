using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonPlayAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale +
            new Vector3(0.005f * (Mathf.Sin(Time.time * 2)), 0.005f * (Mathf.Sin(Time.time * 2)), 0);
    }
}
