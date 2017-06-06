using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Rigidbody))]
public class InteractableVR : MonoBehaviour
{
    //--- Public ---//
    public bool isBeingHeld;
    public Outline outline;

    //--- Private ---//
    Rigidbody rb;
    

    bool isSwitchOffActive;
    IEnumerator switchOff;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        outline.enabled = false;
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
        isBeingHeld = true;
        OnGrab();
    }
	
    public void ReleaseObject(Vector3 _velocity, Vector3 _angularVelocity)
    {
        transform.SetParent(null);
        isBeingHeld = false;

        rb.isKinematic = false;
        rb.velocity = _velocity;
        rb.angularVelocity = _velocity;

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
