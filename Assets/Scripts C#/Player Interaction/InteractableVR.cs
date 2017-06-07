using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[RequireComponent(typeof(Rigidbody))]
public class InteractableVR : MonoBehaviour
{
    //--- Public ---//
    [Header("Picking Up")]
    public bool pickUp = true;

    [Header("Rotating")]
    public bool rotate = false;
    public float minRotation;
    public float maxRotation;
    
    [Header("References")]
    public Outline outline;
    public bool isBeingHeld;

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

    public void ConnectToObject(Transform holdPosition)
    {
        if (pickUp)
            HoldObject(holdPosition);
        else if (rotate)
            RotateObject();
    }

    public void DisconnectFromObject(Vector3 _velo, Vector3 _anguVelo)
    {
        if (pickUp)
            ReleaseObject(_velo, _anguVelo);
        else if (rotate)
            StopRotating();
    }

    private void HoldObject(Transform holdPosition)
    {
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        transform.SetParent(holdPosition);
        isBeingHeld = true;

        OnGrab();
    }
	
    private void ReleaseObject(Vector3 _velocity, Vector3 _angularVelocity)
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.velocity = _velocity;
        rb.angularVelocity = _velocity;

        isBeingHeld = false;

        OnRelease();
    }

    private void RotateObject()
    {

    }

    private void StopRotating()
    {

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
