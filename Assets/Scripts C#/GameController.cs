using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PatientState ps;

    private Patient pt;

	void Start ()
    {
        pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();
	}
	
	public void FinishGame()
    {
        pt.EvaluatePatient();
    }

    public void SendTBSAEstimation(int percentage)
    {
        //ps.IsTBSAEstimated = true;
        //ps.TBSAEstimation = percentage;
    }
}
