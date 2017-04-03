using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{    
    // Serialize fields
    [SerializeField] private List<IA_Tags> correctOrder = new List<IA_Tags>();
    // only serialized for testing
    [SerializeField]private List<GameObject> snappedObjects = new List<GameObject>(); // unsure if necessary

    // Public Get
    public List<IA_Tags> GetCorrectOrder { get { return correctOrder; } }

    public void AddObject(GameObject objToAdd)// Unsure if necessary
    {
        Debug.Log("adding object "+objToAdd.name+" to patient");
        // saves reference from added object to know which objects are active
        snappedObjects.Add(objToAdd);
        objToAdd.layer = 9;
        objToAdd.transform.SetParent(transform); 
    }

    [SerializeField]private GameObject burnWoundPrefab;
    // List of all burns
    private List<IA_Area> burnWounds = new List<IA_Area>();

    public void PlaceBurn(Vector3 pos)
    {
        // set burn degree
        if (burnWounds.Count > 50)
            return;

        GameObject newBurn = Instantiate(burnWoundPrefab, pos, Quaternion.identity) as GameObject;
        burnWounds.Add(newBurn.GetComponent<IA_Area>());
        burnWounds[burnWounds.Count - 1].id = burnWounds.Count - 1;

        //Debug.Log("Placed burn wound with id " + burnWounds[burnWounds.Count - 1].GetComponent<IA_Area>().id);
        // combine mesh?
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

    public void EvaluatePatient()
    {
        foreach (IA_Area burn in burnWounds)
        {
            // Update color of the burn area
            burn.FinishStatus(CheckOrder(burn.PlaceOrder));
            // Write to JSON report on the specific burn?

        }
    }
}
