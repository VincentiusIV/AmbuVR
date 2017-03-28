using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum IA_Areas { Mouth, Burn}

[System.Serializable]
public enum App_Status { UNFINISHED, FIN_INCORRECT, FIN_CORRECT}

public class IA_Area : MonoBehaviour
{
    public int id;

    public IA_Areas thisArea;
    public App_Status status;
    private List<IA_Tags> placeOrder = new List<IA_Tags>();
    private List<IA_Tags> correctOrder = new List<IA_Tags>();

    private Patient pt;

    private int burnDegree;

    private void Start()
    {
        pt = GameObject.FindWithTag("Patient").GetComponent<Patient>();
        correctOrder = pt.GetCorrectOrder;

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
        // TODO
        // Add if status is not finished correctly
        if (status == App_Status.FIN_CORRECT)
            return;

        placeOrder.Add(item.thisItem);

        // check correct order from patient
        // update burn status to patient
        pt.UpdateBurnStatus(item, thisArea);
    }

    public bool CheckOrder()
    {
        bool correct = false;

        if (placeOrder.Count != correctOrder.Count)
            for (int i = 0; i < correctOrder.Count - placeOrder.Count; i++)
                placeOrder.Add(IA_Tags.None);

        for (int i = 0; i < placeOrder.Count; i++)
        {
            if (placeOrder[i] == correctOrder[i])
            { correct = true; }// correct
            else { return correct = false; }// false
        }
        return correct;
        // sc.something
    }
}


