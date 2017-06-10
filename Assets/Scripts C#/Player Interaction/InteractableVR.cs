using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using AmbuVR;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class InteractableVR : MonoBehaviour
{
    //--- Public ---//
    [Header("Picking Up")]
    public bool pickUp = true;

    [Header("Rotating")]
    public bool rotate = false;
    public Vector3 minRotation;
    public Vector3 maxRotation;

    [Header("References")]
    public cakeslice.Outline outline;
    public bool isBeingHeld;
    public Text valueOutput;
    //--- Private ---//
    Rigidbody rb;

    private Vector3 newRotationEuler;
    private Vector3 oldRotationEuler;
    private Transform handRotation;

    public Vector3 rotationValues;

    bool isSwitchOffActive;
    IEnumerator switchOff;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if(outline != null)
            outline.enabled = false;

        minRotation += transform.rotation.eulerAngles;
        maxRotation += transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if(isBeingHeld && rotate)
        {
            newRotationEuler = handRotation.rotation.eulerAngles;
            Vector3 rotationDiff = oldRotationEuler - newRotationEuler;
            Vector3 newRotation = transform.rotation.eulerAngles - rotationDiff;

            // Clamp new rotation between min and max rotation
            float xRotation = CustomMathf.ClampAngle(newRotation.x, minRotation.x, maxRotation.x);
            float yRotation = CustomMathf.ClampAngle(newRotation.y, minRotation.y, maxRotation.y);
            float zRotation = CustomMathf.ClampAngle(newRotation.z, minRotation.z, maxRotation.z);

            newRotation = new Vector3(Mathf.RoundToInt(xRotation), Mathf.RoundToInt(yRotation), Mathf.RoundToInt(zRotation));
            rotationValues = newRotation - minRotation;
            valueOutput.text = Mathf.RoundToInt(rotationValues.z).ToString();

            transform.rotation = Quaternion.Euler(newRotation);
            oldRotationEuler = newRotationEuler;
        }
    }

    public void ConnectToObject(Transform holdPosition)
    {
        isBeingHeld = true;

        if (pickUp)
            HoldObject(holdPosition);
        else if (rotate)
            RotateObject(holdPosition);

        OnGrab();
    }

    public void DisconnectFromObject(Vector3 _velo, Vector3 _anguVelo)
    {
        isBeingHeld = false;

        if (pickUp)
            ReleaseObject(_velo, _anguVelo);
        else if (rotate)
            StopRotating();

        OnRelease();
    }

    private void HoldObject(Transform holdPosition)
    {
        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        transform.SetParent(holdPosition);

    }
	
    private void ReleaseObject(Vector3 _velocity, Vector3 _angularVelocity)
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.velocity = _velocity;
        rb.angularVelocity = _velocity;

    }


    private void RotateObject(Transform holdPosition)
    {
        handRotation = holdPosition;
        Debug.Log("Rotating object " + gameObject.name);
        newRotationEuler = holdPosition.rotation.eulerAngles;
        oldRotationEuler = holdPosition.rotation.eulerAngles;
    }

    private void StopRotating()
    {
        newRotationEuler = Vector3.zero;
    }

    public virtual void OnGrab()
    {

    }

    public virtual void OnRelease()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("NearPlayerTrigger") && outline != null)
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
