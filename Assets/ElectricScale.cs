using System.Collections;
using System.Collections.Generic;
using CHG.Lab;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricScale : MonoBehaviour
{
    public int currentWeight;
    public TextMeshPro scaleText;
    public List<GameObject> cubes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + "Triggered");
        Metal metal = other.GetComponent<Metal>();
        
        if(metal != null)
        {
            int metalWeight = metal.GetWeight();
            currentWeight += metalWeight;

            if(scaleText != null)
            {
                scaleText.text = currentWeight.ToString() + "g";
            }
        }
        cubes.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        Metal metal = other.GetComponent<Metal>();
        
        if(metal != null)
        {
            int metalWeight = metal.GetWeight();
            currentWeight -= metalWeight;

            if(scaleText != null)
            {
                scaleText.text = currentWeight.ToString() + "g";
            }
        }
        cubes.Remove(cubes.Find(x => x.GetInstanceID() == other.GetInstanceID()));
    }

    public void ResetCubes() 
    {
        foreach(GameObject cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();
        currentWeight = 0;
        scaleText.text = "0g";
    }
}
