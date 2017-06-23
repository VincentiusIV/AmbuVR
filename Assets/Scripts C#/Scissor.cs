using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissor : InteractableVR {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Clothes"))
        {
            //other.transform.SetParent(null);
            //other.GetComponent<Rigidbody>().isKinematic = false;
            //other.GetComponent<Rigidbody>().useGravity = true;

            //other.GetComponent<Collider>().isTrigger = false;
            other.transform.parent.gameObject.SetActive(false);
        }
    }
}
