using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{    
    // Serialize fields
    [SerializeField] private List<IA_Tags> correctOrder = new List<IA_Tags>();
    private List<IA_Tags> placeOrder = new List<IA_Tags>();
    // only serialized for testing
    [SerializeField]public List<GameObject> snappedObjects = new List<GameObject>();

    // Reference fields
    private SettingsController sc;

    private void Start()
    {
        sc = GameObject.FindWithTag("VariousController").GetComponent<SettingsController>();
    }

    private void Update()
    {

    }

    public void AddObject(GameObject objToAdd)
    {
        Debug.Log("adding object "+objToAdd.name+" to patient");
        // saves reference from added object to know which objects are active
        snappedObjects.Add(objToAdd);
        objToAdd.layer = 9;
        objToAdd.transform.SetParent(transform); 

        
        
    }

    public void ApplyMed(ItemData med, IA_Areas area)
    {
        Debug.Log("Applying med: "+med.thisItem.ToString());

        if (area == med.correctArea)
            placeOrder.Add(med.thisItem);
        else placeOrder.Add(IA_Tags.None);

        if (correctOrder[placeOrder.Count] != med.thisItem)
            Debug.Log("Wrong order, should have placed " + correctOrder[placeOrder.Count - 1].ToString());
    }

    void CheckOrder()
    {
        // 
        if (placeOrder.Count != correctOrder.Count)
            for (int i = 0; i < correctOrder.Count - placeOrder.Count; i++)
                placeOrder.Add(IA_Tags.None);

        for (int i = 0; i < placeOrder.Count; i++)
        {
            if (placeOrder[i] == correctOrder[i])
            { }// correct
            else { }// false
        }

        // sc.something
    }
}
