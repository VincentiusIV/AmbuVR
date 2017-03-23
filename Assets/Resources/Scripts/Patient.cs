using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour {


    // Serialize fields
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
    }
}
