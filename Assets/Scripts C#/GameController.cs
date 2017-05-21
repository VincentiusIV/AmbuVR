using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameController : MonoBehaviour
{
    private PatientState ps;

    private Patient pt;

    private bool gameActive;

	void Start ()
    {
        try
        {
            pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();
            gameActive = true;
        }
        catch(NullReferenceException)
        {
            Debug.Log("There is no patient in scene, game will be set inactive");
            gameActive = false;
        }
        
	}
	
	public void FinishGame()
    {
        if (gameActive)
            pt.EvaluatePatient();
        else Debug.Log("Game is not active");
    }

    public void SendTBSAEstimation(int percentage)
    {
        //ps.IsTBSAEstimated = true;
        //ps.TBSAEstimation = percentage;
    }
}
