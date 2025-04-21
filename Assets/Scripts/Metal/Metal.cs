using System.Collections;
using System.Collections.Generic;
using CHG.Lab;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Metal : MonoBehaviour
{
    public MetalData metalData;
    private XRGrabInteractable _interactable;
    public XRGrabInteractable Interactable
    {
        get => _interactable;
        set => _interactable = value;
    }
    void Awake()
    {
        Interactable = GetComponent<XRGrabInteractable>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetWeight()
    {
        return metalData != null ? metalData.weight : 0;
    }
}
