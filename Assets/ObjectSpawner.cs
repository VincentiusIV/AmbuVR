using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    [Header("Mandatory References")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform objectSpawnPos;
    [SerializeField] private TextMesh text;

    private void Start()
    {
        text.text = objectToSpawn.name;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VR_Controller"))
        {
            Instantiate(objectToSpawn, objectSpawnPos.position, Quaternion.identity);
        }
    }
}
