using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct PatientState
{
    [Header("Burn Data")]
    [Range(0,100)]public int tbsa;      // the tbsa of the patient
    public MedicalItem coolingToUse;    // cooling to use
    public MedicalItem painMedToUse;    // pain med to use

    [Header("Statistics")]
    public bool estimated;              // was the patient estimated?
    public bool estimatedCorrectly;     // did the player estimate correctly?
    public int totalAmountOfBurns;      // the total amount of burns
    public int amountOfBurnsTreated;    // amount of burns that were treated correctly
    public bool receivedPainMed;
    public bool receivedCorrectPainMed; // did this patient receive the correct pain medication?
}

public class Patient : MonoBehaviour
{
    public static Patient instance;

    public PatientState patientState;       // The state of this patient
    private List<MedicalItem> correctOrder; // The correct order in which objects should be placed on this patient
    public List<PatientArea> burnWounds;    // List of burn wounds attached to this patient

    public PatientArea mouth;               // the mouth object of this patient

    public UnityEvent onFinishTBSA;
    public UnityEvent onFinishCooling;
    public UnityEvent onFinishPainMed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        // Configuring patient depending on settings
        patientState.totalAmountOfBurns = burnWounds.Count;
        PatientSettings patientSettings = GameObject.FindWithTag("VariousController").GetComponent<PatientSettings>();
        patientState.coolingToUse = patientSettings.CoolingToUse(patientState.tbsa);
        patientState.painMedToUse = patientSettings.PainMedicationToUse(patientState.tbsa);

        correctOrder = new List<MedicalItem>();
        // Create correct treatment order
        for (int i = 0; i < patientSettings.treatmentOrder.Count; i++)
        {
            if (patientSettings.treatmentOrder[i] == TreatmentSteps.Cooling)
                correctOrder.Add(patientState.coolingToUse);
            else if (patientSettings.treatmentOrder[i] == TreatmentSteps.Wrapping)
                correctOrder.Add(MedicalItem.PlasticWrap);
        }
        // Set correct orders for each burn wound
        foreach (PatientArea item in burnWounds)
        {
            item.correctOrder = correctOrder;
        }
    }

    public void ReceivePainMed(MedicalItem med)
    {
        if (!patientState.receivedPainMed)
        {
            patientState.receivedPainMed = true;
            patientState.receivedCorrectPainMed = patientState.painMedToUse == med;
            onFinishPainMed.Invoke();
        }
        else Debug.Log("This patient already received the " + patientState.receivedCorrectPainMed + " pain med");
    }

    public void FinishCooling(MedicalItem cooling)
    {
        foreach (PatientArea item in burnWounds)
        {
            item.ApplyMed(cooling);
            patientState.amountOfBurnsTreated++;
        }

        onFinishCooling.Invoke();
    }

    //Evaluates the patient, checking the current status
    public void EvaluatePatient()       
    {
        GetComponent<GamePlayEvent>().EventFinished();
    }

    internal void AddBurn(Vector3 worldPoint)
    {
        throw new NotImplementedException();
    }

    // TO-DO:
    // Method to lock/unlock the patient ragdoll
    public void LockPatientRagdoll(bool lockState)
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody item in rigidBodies)
        {
            item.isKinematic = lockState;
        }
    }



    public void ConfirmTBSAEstimation(int estimation)
    {
        patientState.estimated = true;
        patientState.estimatedCorrectly = patientState.tbsa == estimation;
        onFinishTBSA.Invoke();
    }
}
