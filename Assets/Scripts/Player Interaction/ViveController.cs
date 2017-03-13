using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains functionality for the motion controllers
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveController : MonoBehaviour
{
    // Private SteamVR fields
    SteamVR_TrackedObject motionCon;
    SteamVR_Controller.Device device;

    // Private fields
    LineRenderer pointer;
    UIController UI;

    // Private & Serialized fields
    [SerializeField]Transform pointerOrigin;
    [SerializeField]float pointerLength;

    private void Awake()
    {
        // SteamVR ref
        motionCon = GetComponent<SteamVR_TrackedObject>();
        // references
        pointer = GetComponent<LineRenderer>();
        pointer.enabled = false;

        UI = GameObject.FindWithTag("VariousController").GetComponent<UIController>();
        
    }

    private void Update()
    {
        device = SteamVR_Controller.Input((int)motionCon.index);

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) || UI.IsUIEnabled)
        {
            Debug.Log("touching the touchpad");
            DrawPointer();
        }
        if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("released touchpad");
            pointer.enabled = false;
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            Debug.Log("touching the application menu");
            // show/hide in game menu
            if (UI.ToggleUI())
            {
                // ui is now enabled
            }
            else pointer.enabled = false;
        }
            
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("VR_Controller"))
            return;
        else
        {
            if(device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                // pickup
                other.attachedRigidbody.isKinematic = true;
                //other.transform.position = transform.position;
                other.transform.SetParent(transform);
            }
            if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                // let go
                other.attachedRigidbody.isKinematic = false;
                other.transform.SetParent(null);
                ThrowVelocity(other.attachedRigidbody);
            }
        }
    }
    // Pointer
    private void DrawPointer()
    {
        pointer.enabled = true;

        RaycastHit hit;

        pointer.SetPosition(0, pointerOrigin.position);

        if (Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit, pointerLength))
        {
            pointer.SetPosition(1, hit.point);

            if(hit.collider.CompareTag("Button"))
            {
                hit.collider.GetComponent<ButtonScript>().Highlight();

                if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
                    hit.collider.GetComponent<ButtonScript>().Click();
            }
        }
        else
        {
            pointer.SetPosition(1, pointerOrigin.position + (pointerOrigin.forward * pointerLength));
        }
    }

    private void ThrowVelocity(Rigidbody rb)
    {
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
    }
}
