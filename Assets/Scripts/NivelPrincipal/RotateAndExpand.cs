using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndExpand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale +
            new Vector3(0.010f * (Mathf.Sin(Time.time * 2)), 0.010f * (Mathf.Sin(Time.time * 2)), 0);

        //transform.Rotate(0, 100 * Time.deltaTime, 0);
    }
}
