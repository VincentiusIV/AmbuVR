using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{    
    // Serialize fields
    [SerializeField] private List<IA_Tags> correctOrder = new List<IA_Tags>();
    [SerializeField] private GameObject burnWoundPrefab;

    [HideInInspector] public IA_Tags correctPainMed;
    [HideInInspector] public bool didReceiveCorrectPainMed;
    // Public Get
    public List<IA_Tags> GetCorrectOrder { get { return correctOrder; } }

    // References
    public List<IA_Area> burnWounds = new List<IA_Area>();
    public ConfigureResultsToDisplay crd;

    public void PlaceBurn(Vector3 pos)
    {
        // set burn degree
        if (burnWounds.Count > 50)
            return;

        GameObject newBurn = Instantiate(burnWoundPrefab, pos, Quaternion.identity) as GameObject;
        burnWounds.Add(newBurn.GetComponent<IA_Area>());
        burnWounds[burnWounds.Count - 1].id = burnWounds.Count - 1;
    }

    public bool CheckOrder(List<IA_Tags> placeOrder)
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
    List<BurnWoundStatus> bwsList;
    

    public void EvaluatePatient()
    {
        Debug.Log("Evaluating patient...");
        bwsList = new List<BurnWoundStatus>();
        if (burnWounds.Count == 0)
            Debug.LogWarning("There are no burn wounds to evaluate, make sure you have referenced manually placed wounds!");
        foreach (IA_Area burn in burnWounds)
        {
            // Update color of the burn area
            bool newStatus = CheckOrder(burn.PlaceOrder);

            bwsList.Add(burn.FinishStatus(newStatus));
            // Write to JSON report on the specific burn?
        }
        if(crd != null)
            crd.ConfigureResultDisplay(bwsList, didReceiveCorrectPainMed, true);
    }

    public void ResetPatient()
    {
        foreach (IA_Area burn in burnWounds)
        {
            burn.ResetPlaceOrder();
        }
    }
}
