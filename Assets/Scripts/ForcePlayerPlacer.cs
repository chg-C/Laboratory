using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePlayerPlacer : MonoBehaviour
{
    public void ForcePlace()
    {
        var p = GameObject.FindWithTag("Player");

        p.transform.position = transform.position;
        
        p.transform.LookAt(p.transform.position + transform.forward);
    }
}
