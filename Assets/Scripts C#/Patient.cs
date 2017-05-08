using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PatientState
{
    public int totalAmountOfBurns;      // the total amount of burns
    public int amountOfBurnsTreated;    // amount of burns that were treated correctly
    public bool receivedCorrectPainMed; // did this patient receive the correct pain medication?
}

public class Patient : MonoBehaviour
{
    public PatientState patientState;                               // The state of this patient
    private List<MedicalItem> correctOrder = new List<MedicalItem>();// The correct order in which objects should be placed on this patient

    public GameObject burnWoundPrefab;                              // Prefab of a burn wound      
    public List<PatientArea> burnWounds = new List<PatientArea>();  // List of burn wounds attached to this patient
    public PatientArea mouth;                                       // the mouth object of this patient

    public ConfigureResultsToDisplay crd;                           // Reference to config to display manager

    public int tbsa;

    private void Start()
    {
        patientState.totalAmountOfBurns = burnWounds.Count;

        PatientSettings patientSettings = GameObject.FindWithTag("VariousController").GetComponent<PatientSettings>();

        
    }

    public void EvaluatePatient()       //Evaluates the patient, checking the current status
    {
        Debug.Log("Evaluating patient...");
        List<AreaStatus> bwsList = new List<AreaStatus>();

        if (burnWounds.Count == 0)
            Debug.LogWarning("There are no burn wounds to evaluate, make sure you have referenced manually placed wounds!");

        foreach (PatientArea burn in burnWounds)
            bwsList.Add(burn.FinishStatus());

        if(crd != null)
            crd.ConfigureResultDisplay(bwsList, patientState.receivedCorrectPainMed, true);
    }

    public void ResetPatient()
    {
        foreach (PatientArea burn in burnWounds)
        {
            burn.ResetPlaceOrder();
        }
    }   // Resets the patient

    public void PlaceBurn(Vector3 pos)  // Used to place burn wounds depending on where the player is pointing the controller
    {
        if (burnWounds.Count > 50)
            return;

        GameObject newBurn = Instantiate(burnWoundPrefab, pos, Quaternion.identity) as GameObject;
        burnWounds.Add(newBurn.GetComponent<PatientArea>());
    }
}
