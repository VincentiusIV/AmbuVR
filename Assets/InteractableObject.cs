using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    [Header("Mandatory References")]
    public Animator objectToAnimate;
    public Rigidbody rigidBody;

    private bool isObjectActive = false;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VR_Controller") || other.CompareTag("Pick Up"))
        {
            if(objectToAnimate == null)
            {
                throw new Exception("There is no animator connected to: "+ gameObject.name);
            }
            
            // Activate / Decativate
            isObjectActive = !isObjectActive;

            if (rigidBody != null)
                rigidBody.isKinematic = isObjectActive;

            objectToAnimate.SetBool("Open", isObjectActive);
        }
    }
}
