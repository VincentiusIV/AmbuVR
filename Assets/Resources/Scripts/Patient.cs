using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{    
    // Serialize fields
    [SerializeField] private List<IA_Tags> correctOrder = new List<IA_Tags>();
    private List<IA_Area> placeOrder = new List<IA_Area>();
    // only serialized for testing
    [SerializeField]private List<GameObject> snappedObjects = new List<GameObject>();

    // Reference fields
    private SettingsController sc;

    public List<IA_Tags> GetCorrectOrder{ get { return correctOrder; } }

    private void Start()
    {
        sc = GameObject.FindWithTag("VariousController").GetComponent<SettingsController>();
    }

    public void AddObject(GameObject objToAdd)
    {
        Debug.Log("adding object "+objToAdd.name+" to patient");
        // saves reference from added object to know which objects are active
        snappedObjects.Add(objToAdd);
        objToAdd.layer = 9;
        objToAdd.transform.SetParent(transform); 
    }

    public void UpdateBurnStatus(ItemData med, IA_Areas area)
    {
        
    }

    // Adding burn wounds
    [SerializeField]
    private GameObject burnWoundPrefab;

    private List<GameObject> burnWounds = new List<GameObject>();

    public void PlaceBurn(Vector3 pos)
    {
        // set burn degree
        GameObject newBurn = Instantiate(burnWoundPrefab, pos, Quaternion.identity) as GameObject;
        burnWounds.Add(newBurn);
        burnWounds[burnWounds.Count - 1].GetComponent<IA_Area>().id = burnWounds.Count;

        Debug.Log("Placed burn wound with id " + burnWounds[burnWounds.Count - 1].GetComponent<IA_Area>().id);
        // combine mesh?
    }

}
