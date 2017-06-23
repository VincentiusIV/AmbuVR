using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PatientAreaType { Mouth, Burn}

[System.Serializable]// likely be removed
public enum App_Status { UNFINISHED, FIN_INCORRECT, FIN_CORRECT }

[System.Serializable]
public struct AreaStatus
{
    public MedicalItem coolType;
    public bool isCooled;
    public bool isWrapped;
}

public class PatientArea : MonoBehaviour
{
    [Header("Mandatory References")]
    private Patient patient;                    // Reference to the patient
    private Renderer rend;                      // Reference to renderer on this gameobject

    public PatientAreaType areaType;            // Defines what this area is
    public App_Status status;                   // The status of the wound
    //public BurnDegree burnDegree;               // The degree of the burn

    public List<MedicalItem> placeOrder;        // The order in which med items were used on this area
    public List<MedicalItem> correctOrder;      // The correct order of med items

    public GameObject wrapVisual;

    [Header("Area status")]
    AreaStatus areaStatus;            // Status of this burn wound

    private void Start()
    {
        if(areaType == PatientAreaType.Burn)
        {
            areaStatus.isCooled = false;
            areaStatus.isWrapped = false;

            /*if (burnDegree == BurnDegree.Third) 
                areaStatus.coolType = MedicalItem.BurnS;
            else areaStatus.coolType = MedicalItem.Water;*/
        }

        status = App_Status.UNFINISHED;

        rend = GetComponent<Renderer>();
        patient = transform.GetComponentInParent<Patient>();

        placeOrder = new List<MedicalItem>();

        if(wrapVisual != null)
            wrapVisual.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Medical Item"))
        {
            ItemData med = other.GetComponent<ItemData>();
            ApplyMed(med.thisItem);
            other.GetComponent<InteractableVR>().DisconnectFromObject(Vector3.zero, Vector3.zero);
            other.transform.position = transform.position;
            other.transform.SetParent(transform);
            //Destroy(other.gameObject);
        }
    }

    public void ApplyMed(MedicalItem item)      // Applies a medical item to this area
    {
        if (status == App_Status.FIN_CORRECT)
        {
            Debug.Log("This wound is finished!");
            return;
        } 

        Debug.Log(string.Format("{0} {1} being applied with {2}", status, areaType, item));
        
        placeOrder.Add(item);

        if (item == MedicalItem.Water)
            rend.material.color = Color.blue;

        if (item == areaStatus.coolType)           
            areaStatus.isCooled = true;
        
        if (item == MedicalItem.PlasticWrap)
        {
            areaStatus.isWrapped = true;

            if (wrapVisual != null)
                wrapVisual.SetActive(true);
        }     

        if (placeOrder.Count == correctOrder.Count && CheckOrder())
        {
            status = App_Status.FIN_CORRECT;
            Debug.Log("You succesfully treated this burn!");
            patient.EvaluatePatient();
        }
        else if(placeOrder.Count == correctOrder.Count)
        {
            status = App_Status.FIN_INCORRECT;
            Debug.Log("You failed");
            patient.EvaluatePatient();
        }
    }

    public AreaStatus FinishStatus() // Returns the status of this area & changes color
    {
        if (CheckOrder())
            rend.material.color = Color.green;
        else rend.material.color = Color.yellow;

        return areaStatus;
    }

    public bool CheckOrder()
    {
        bool correct = false;

        if (placeOrder.Count != correctOrder.Count)
            for (int i = 0; i < correctOrder.Count - placeOrder.Count; i++)
                placeOrder.Add(MedicalItem.None);

        for (int i = 0; i < placeOrder.Count; i++)
        {
            if (placeOrder[i] == correctOrder[i])
            { correct = true; }                     // correct
            else { return correct = false; }        // false
        }
        return correct;
    }

    public void ResetPlaceOrder()
    {
        placeOrder = new List<MedicalItem>();
    }

    //////// DESKTOP TESTING /////////
    private void OnMouseOver()
    {
        if(Input.GetButton("Fire1"))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ApplyMed(MedicalItem.Water);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                ApplyMed(MedicalItem.BurnS);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                ApplyMed(MedicalItem.PM_Paracetamol);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                ApplyMed(MedicalItem.PM_Opiaten);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                ApplyMed(MedicalItem.PlasticWrap);
        }
        else if(Input.GetButtonDown("Fire2"))
        {
            placeOrder = new List<MedicalItem>();
        }
    }
}


