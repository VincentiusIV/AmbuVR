using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum IA_Tags { None, BurnS, Water, PM_Paracetamol, PM_Opiaten, PlasticWrap}

public class ItemData : MonoBehaviour
{
    [SerializeField]public IA_Tags thisItem;
    [SerializeField] public IA_Areas correctArea;
}


