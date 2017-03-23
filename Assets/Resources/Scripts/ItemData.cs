using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum IA_Tags { None, Patient, BurnS, Water, PainMed, PWrap }

public class ItemData : MonoBehaviour
{
    [SerializeField]public IA_Tags thisItem;
    [SerializeField] public IA_Areas correctArea;
}


