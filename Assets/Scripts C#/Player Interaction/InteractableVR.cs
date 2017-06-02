using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableVR : MonoBehaviour
{
    //--- Public ---//
    public bool isBeingHeld;

    //--- Private ---//

    Transform hand;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isBeingHeld)
        {
            transform.position = hand.position;
        }
    }

    public void HoldObject(Transform _hand)
    {
        hand = _hand;

        rb.isKinematic = true;

        OnGrab();
    }
	
    public void ReleaseObject(SteamVR_TrackedObject motionCon, SteamVR_Controller.Device device)
    {
        rb.isKinematic = false;

        Transform origin = motionCon.origin ? motionCon.origin : motionCon.transform.parent;
        if (origin != null)
        {
            rb.velocity = origin.TransformVector(device.velocity);
            rb.angularVelocity = origin.TransformVector(device.angularVelocity);
        }
        else
        {
            rb.velocity = device.velocity;
            rb.angularVelocity = device.angularVelocity;
        }

        OnRelease();
    }

    public void OnGrab()
    {

    }

    public void OnRelease()
    {

    }
}
