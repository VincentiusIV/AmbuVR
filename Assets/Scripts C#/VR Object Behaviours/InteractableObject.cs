using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InteractableObject : MonoBehaviour {

    [Header("Mandatory References")]
    public Animator objectToAnimate;
    public Rigidbody rigidBody;
    public ParticleSystem psToActivate;

    private bool isObjectActive = false;

    private void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VR_Controller") || other.CompareTag("Pick Up"))
        {
            Debug.Log("Activating object: " + gameObject.name);
            if(objectToAnimate == null)
            {
                throw new Exception("There is no animator connected to: "+ gameObject.name);
            }
            
            // Activate / Decativate
            isObjectActive = !isObjectActive;

            if (rigidBody != null)
                rigidBody.isKinematic = isObjectActive;
            if (psToActivate != null)
            {
                if (isObjectActive)
                    psToActivate.Play();
                else psToActivate.Stop();
            }

            objectToAnimate.SetBool("Open", isObjectActive);
        }
    }
}
