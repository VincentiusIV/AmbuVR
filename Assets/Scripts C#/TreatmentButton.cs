using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatmentButton : MonoBehaviour {

    public Patient pt;

    public bool evaluate;
    public bool reset;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit " + other.name);
        if(other.CompareTag("VR_Controller"))
        {
            if (evaluate)
            {
                pt.EvaluatePatient();
            }
        }
    }
}
 