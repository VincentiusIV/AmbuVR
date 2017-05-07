using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameState
{
    public bool IsTBSAEstimated;
    public bool AreBurnsTreated;

    public int TBSAEstimation;
    public int ParentStressScore;
    public int BurnTreatmentScore;
}
public class GameController : MonoBehaviour
{
    private GameState gs;

    private Patient pt;
	// Use this for initialization
	void Start () {
        pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();
	}
	
	public void FinishGame()
    {
        pt.EvaluatePatient();
    }

    public void SendTBSAEstimation(int percentage)
    {
        gs.IsTBSAEstimated = true;
        gs.TBSAEstimation = percentage;

        
    }
}
