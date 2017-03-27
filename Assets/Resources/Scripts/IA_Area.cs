using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum IA_Areas { Mouth, Burn}

public class IA_Area : MonoBehaviour {

    public IA_Areas thisArea;

    private Patient pt;

    public int burnDegree;

    private void Start()
    {
        pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pick Up"))
            pt.ApplyMed(other.GetComponent<ItemData>(), thisArea);
    }

    private void Update()
    {
        // 
        // scale object on patient based on tbsa
    }
}
