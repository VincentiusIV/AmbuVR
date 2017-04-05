using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private Patient pt;
	// Use this for initialization
	void Start () {
        pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();
	}
	
	public void FinishGame()
    {
        pt.EvaluatePatient();
    }
}
