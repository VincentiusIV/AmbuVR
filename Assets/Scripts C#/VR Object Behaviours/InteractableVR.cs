using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    public RotateAxis rotateAround;
    public float activationThreshold = 0f;
    public int valueRange = 100;

    [Header("Special onGripDown")]
    public ParticleSystem psToEmit;
    //public Animator anime;
    //public string animKeyword;

    [Header("Events")]
    public UnityEvent OnGrab;
    public UnityEvent OnRelease;
    public UnityEvent OnSpecial;

    [Header("References")]
    public GlowObjectCmd outline;
    public bool isBeingHeld;
    public Text valueOutput;
    //--- Private ---//
    Rigidbody rb;

    private Vector3 newRotationEuler;
    private Vector3 oldRotationEuler;
    private Transform handRotation;

    public float value;

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

            float xRotation = transform.eulerAngles.x;
            float yRotation = transform.eulerAngles.y;
            float zRotation = transform.eulerAngles.z;

            // Clamp new rotation between min and max rotation
            switch (rotateAround)
            {
                case RotateAxis.x:
                    xRotation = CustomMathf.ClampAngle(newRotation.z, minRotation.x, maxRotation.x);
                    break;
                case RotateAxis.y:
                    yRotation = CustomMathf.ClampAngle(newRotation.z, minRotation.y, maxRotation.y);
                    break;
                case RotateAxis.z:
                    zRotation = CustomMathf.ClampAngle(newRotation.z, minRotation.z, maxRotation.z);
                    break;
                default:
                    break;
            }

            newRotation = new Vector3(Mathf.RoundToInt(xRotation), Mathf.RoundToInt(yRotation), Mathf.RoundToInt(zRotation));

            switch (rotateAround)
            {
                case RotateAxis.x:
                    value = Mathf.RoundToInt(newRotation.x / (maxRotation.x / valueRange));
                    break;
                case RotateAxis.y:
                    value = Mathf.RoundToInt(newRotation.y / (maxRotation.y / valueRange));
                    break;
                case RotateAxis.z:
                    value = Mathf.RoundToInt(newRotation.z / (maxRotation.z / valueRange));
                    break;
                default:
                    break;
            }

            if (valueOutput != null)
                valueOutput.text = Mathf.RoundToInt(value).ToString();

            if(value > activationThreshold)
            {
                PerformSpecial();
            }

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

        OnGrabVirtual();
        OnGrab.Invoke();
    }

    public void DisconnectFromObject(Vector3 _velo, Vector3 _anguVelo)
    {
        isBeingHeld = false;

        if (pickUp)
            ReleaseObject(_velo, _anguVelo);
        else if (rotate)
            StopRotating();

        OnReleaseVirtual();
        OnRelease.Invoke();
    }

    public void PerformSpecial()
    {
        if (psToEmit != null)
        {
            if (psToEmit.isPlaying)
                psToEmit.Stop();
            else psToEmit.Play();
        }

        OnSpecial.Invoke();
        /*
        if(anime != null)
        {
            anime.SetBool(animKeyword, animState = !animState);
        }*/
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

        if (psToEmit != null && psToEmit.isPlaying)
            psToEmit.Stop();
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

    public virtual void OnGrabVirtual()
    {

    }

    public virtual void OnReleaseVirtual()
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
[System.Serializable]
public enum RotateAxis
{
    x, y, z
}


