using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : MonoBehaviour
{
    Collider nowCol;
    float maxScale = 4;
    Vector3 nowScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nowCol != null && nowCol.gameObject.tag == "Liquid")
        {
            if(nowScale.y < 4)
            {
                Transform mercury = transform.Find("mercurry");
                nowScale = mercury.localScale;
                nowScale.y += 0.001f;
                mercury.localScale = nowScale;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(nowCol == null)
            nowCol = other;
    }

    void OnTriggerExit(Collider other)
    {
        if(nowCol == other)
            nowCol = null;
    }
}
