using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItem : MonoBehaviour {

    public Vector3 spawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawner"))
            transform.position = spawnPosition;
    }

}
