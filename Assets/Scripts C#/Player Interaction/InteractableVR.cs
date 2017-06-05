using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Rigidbody))]
public class InteractableVR : MonoBehaviour
{
    //--- Public ---//
    public bool isBeingHeld;

    //--- Private ---//
    Rigidbody rb;
    Outline outline;

    bool isSwitchOffActive;
    IEnumerator switchOff;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        outline = transform.FindChild("Model").GetComponent<Outline>();
    }
    private void Update()
    {

    }

    public void HoldObject(Transform holdPosition)
    {
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;
        transform.SetParent(holdPosition);
        rb.isKinematic = true;

        OnGrab();
    }
	
    public void ReleaseObject(SteamVR_TrackedObject motionCon, SteamVR_Controller.Device device)
    {
        transform.SetParent(null);
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

    public virtual void OnGrab()
    {

    }
    public virtual void OnRelease()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("NearPlayerTrigger"))
        {
            outline.enabled = true;

            if (isSwitchOffActive)
                StopCoroutine(switchOff);

            switchOff = SwitchOff();
            StartCoroutine(switchOff);
        }
    }
    IEnumerator SwitchOff()
    {
        isSwitchOffActive = true;
        yield return new WaitForSeconds(.1f);
        outline.enabled = isSwitchOffActive = false;
    }
}
