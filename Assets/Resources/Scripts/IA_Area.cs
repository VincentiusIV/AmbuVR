using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum IA_Areas { Mouth, Burn}

[System.Serializable]// likely be removed
public enum App_Status { UNFINISHED, FIN_INCORRECT, FIN_CORRECT }

[System.Serializable]
public struct BurnWoundStatus
{
    // Types
    public IA_Tags coolType;
    
    // State
    public bool isCooled;
    public bool isWrapped;
}

public class IA_Area : MonoBehaviour
{
    // ID of the burn wound
    public Patient pt;

    public int id;
    public IA_Areas thisArea;
    public App_Status status;
    public BurnWoundStatus bws;
    // Tracks place order
    public List<IA_Tags> PlaceOrder { get; private set; }
    
    private Renderer rend;

    public int burnDegree;

    private void Start()
    {
        PlaceOrder = new List<IA_Tags>();
        rend = GetComponent<Renderer>();
        status = App_Status.UNFINISHED;
        pt = transform.GetComponentInParent<Patient>();

        if (burnDegree > 2)
            bws.coolType = IA_Tags.BurnS;
        else bws.coolType = IA_Tags.Water;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pick Up"))
        {
            ItemData activeItem = other.GetComponent<ItemData>();
            if(activeItem.correctArea == IA_Areas.Burn && thisArea == IA_Areas.Burn)
                ApplyMed(activeItem.thisItem);
            else if(activeItem.correctArea == IA_Areas.Mouth && thisArea == IA_Areas.Mouth)
            {
                if (activeItem.thisItem == pt.correctPainMed)
                    pt.didReceiveCorrectPainMed = true;
            }

        }
    }
    
    public void ApplyMed(IA_Tags item)
    {
        Debug.Log(string.Format("{0} {1} being applied with {2}", status, thisArea, item));
        // TODO
        // Add if status is not finished correctly
        if (status == App_Status.FIN_CORRECT)
            return;
        if (item == IA_Tags.Water)
            rend.material.color = Color.blue;
        PlaceOrder.Add(item);

        // Updates status of the wound
        if (bws.coolType == item)
            bws.isCooled = true;
        else if (item == IA_Tags.PlasticWrap)
            bws.isWrapped = true;
    }

    public BurnWoundStatus FinishStatus(bool fin)
    {
        if (fin)
            rend.material.color = Color.green;
        else rend.material.color = Color.yellow;

        return bws;
    }

    public void ResetPlaceOrder()
    {
        PlaceOrder = new List<IA_Tags>();
    }
}


