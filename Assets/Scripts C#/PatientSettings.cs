using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreatmentSteps { Cooling, PainMedication, Wrapping }

public class PatientSettings : MonoBehaviour {

    [Header("TBSA")]
    [Range(0, 100)] public int minBurnCenter;   // Min TBSA when a child should be send to a burn center
    [Range(0, 100)] public int marginOfError;   // Margin of error when estimating the TBSA
    public int maxAttempts;                     // Max amount of attempts for the player to estimate TBSA

    [Header("Cooling")]
    [Range(0, 100)] public int minTbsaWater = 0;         // Defines min TBSA for use of water
    [Range(0, 100)] public int minTbsaShield = 10;      // Defines min TBSA for use of burn shield

    [Header("Pain Medication")]
    [Range(0, 100)] public int minTbsaParaceta;       // Min TBSA required for use of paracetamol
    [Range(0, 100)] public int minTbsaOpiaten;        // Min TBSA required for use of Opiaten (needle) 

    //[Header("Plastic Wrap")]
    //public BurnDegree minDegree; // The min burn degree required when a plastic wrap should be used

    [Header("Total Application")]
    public List<TreatmentSteps> treatmentOrder;

    public MedicalItem CoolingToUse(int tbsa)
    {
        MedicalItem cooling;
        if (tbsa > minTbsaWater && tbsa < minTbsaShield)
            cooling = MedicalItem.Water;
        else cooling = MedicalItem.BurnS;
        return cooling;
    }

    public MedicalItem PainMedicationToUse(int tbsa)
    {
        MedicalItem painMedication;
        if (tbsa > minTbsaParaceta && tbsa < minTbsaOpiaten)
            painMedication = MedicalItem.PM_Paracetamol;
        else painMedication = MedicalItem.PM_Opiaten;
        return painMedication;
    }

    /*public bool ShouldUsePlasticWrap(BurnDegree degree)
    {
        int burnDegree = (int)degree;
        int minDegree = (int)this.minDegree;

        if (burnDegree > minDegree)
            return true;
        else return false;
    }*/
}
