using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum IA_Areas { Mouth, Burn}

[System.Serializable]// likely be removed
public enum App_Status { UNFINISHED, FIN_INCORRECT, FIN_CORRECT}

[System.Serializable]
public struct BurnWoundStatus
{
    // Types
    public IA_Tags coolType;
    public IA_Tags medType;
    
    // State
    public bool isCooled;
    public bool didReceivePainMed;
    public bool isWrapped;
}

public class IA_Area : MonoBehaviour
{
    // ID of the burn wound
    public int id;

    public IA_Areas thisArea;
    public App_Status status;
    public BurnWoundStatus bws;
    // Tracks place order
    public List<IA_Tags> PlaceOrder { get; private set; }

    private Patient pt;
    private Renderer rend;

    private int burnDegree;

    private void Start()
    {
        pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();
        rend = GetComponent<Renderer>();
        status = App_Status.UNFINISHED;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pick Up"))
        {
            ApplyMed(other.GetComponent<ItemData>());
        }
    }

    private void ApplyMed(ItemData item)
    {
        Debug.Log(string.Format("{0} {1} being applied with {2}", status, thisArea, item.thisItem));
        // TODO
        // Add if status is not finished correctly
        if (status == App_Status.FIN_CORRECT)
            return;
        
        PlaceOrder.Add(item.thisItem);

        // Updates status of the wound
        if (bws.coolType == item.thisItem)
            bws.isCooled = true;
        else if (bws.medType == item.thisItem)
            bws.didReceivePainMed = true;
        else if (item.thisItem == IA_Tags.PlasticWrap)
            bws.isWrapped = true;
    }

    public void FinishStatus(bool fin)
    {
        if (fin)
            rend.material.color = Color.green;
        else rend.material.color = Color.yellow;
    }
}


