using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    [Header("Mandatory References")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform objectSpawnPos;

    public bool triggerEnabled;

    private void OnTriggerEnter(Collider other)
    {
        if(triggerEnabled && other.CompareTag("VR_Controller"))
        {
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        Instantiate(objectToSpawn, objectSpawnPos.position, Quaternion.identity);
    }
}
