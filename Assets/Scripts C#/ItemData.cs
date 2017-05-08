using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MedicalItem { None, BurnS, Water, PM_Paracetamol, PM_Opiaten, PlasticWrap}

public class ItemData : MonoBehaviour
{
    [SerializeField]public MedicalItem thisItem;
    [SerializeField] public PatientAreaType correctArea;
}


