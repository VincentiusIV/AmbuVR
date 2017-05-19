using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PatientState
{
    [Header("Burn Data")]
    [Range(0,100)]public int tbsa;      // the tbsa of the patient
    public MedicalItem coolingToUse;    // cooling to use
    public MedicalItem painMedToUse;    // pain med to use

    [Header("Statistics")]
    public int totalAmountOfBurns;      // the total amount of burns
    public int amountOfBurnsTreated;    // amount of burns that were treated correctly
    public bool receivedPainMed;
    public bool receivedCorrectPainMed; // did this patient receive the correct pain medication?
}

public class Patient : MonoBehaviour
{
    public PatientState ps;                 // The state of this patient
    private List<MedicalItem> correctOrder; // The correct order in which objects should be placed on this patient
    public List<PatientArea> burnWounds;    // List of burn wounds attached to this patient

    public PatientArea mouth;               // the mouth object of this patient
    public ConfigureResultsToDisplay crd;   // Reference to config to display manager

    private void Start()
    {
        correctOrder = new List<MedicalItem>();
        burnWounds = new List<PatientArea>();

        // Configuring patient depending on settings
        ps.totalAmountOfBurns = burnWounds.Count;
        PatientSettings patientSettings = GameObject.FindWithTag("VariousController").GetComponent<PatientSettings>();
        ps.coolingToUse = patientSettings.CoolingToUse(ps.tbsa);
        ps.painMedToUse = patientSettings.PainMedicationToUse(ps.tbsa);
    }

    public void ReceivePainMed(MedicalItem med)
    {
        if (!ps.receivedPainMed)
        {
            ps.receivedPainMed = true;
            ps.receivedCorrectPainMed = ps.painMedToUse == med;
        }
        else Debug.Log("This patient already received the " + ps.receivedCorrectPainMed + " pain med");
    }

    public void EvaluatePatient()       //Evaluates the patient, checking the current status
    {
        Debug.Log("Evaluating patient...");
        List<AreaStatus> areaStatusList = new List<AreaStatus>();

        if (burnWounds.Count == 0)
            Debug.LogWarning("There are no burn wounds to evaluate, make sure you have referenced manually placed wounds!");

        foreach (PatientArea burn in burnWounds)
            areaStatusList.Add(burn.FinishStatus());

        if (crd != null)
            crd.ConfigureResultDisplay(areaStatusList, ps.receivedCorrectPainMed, true);
        else Debug.LogError("Patient has no reference to result display");
    }

    public void LockPatientRagdoll(bool lockState)
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody item in rigidBodies)
        {
            item.isKinematic = lockState;
        }
    }
}
