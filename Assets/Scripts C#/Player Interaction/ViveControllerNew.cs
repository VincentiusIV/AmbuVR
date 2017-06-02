using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Contains functionality for the motion controllers
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveControllerNew : MonoBehaviour
{
    //--- Public ---//
    public Transform holdPosition;
    public Transform pointerOrigin;
    public LayerMask uiLayer;

    //--- Private ---//
    SteamVR_TrackedObject motionCon;
    SteamVR_Controller.Device device;
    SphereCollider triggerCollider;
    InteractableVR currentHeldObject;
    LineRenderer pointer;
    GameObject model;

    //--- Booleans ---//
    bool isHolding = false;

    private void Start()
    {
        triggerCollider = GetComponent<SphereCollider>();
        triggerCollider.enabled = true;

        pointer = GetComponent<LineRenderer>();
        pointer.enabled = false;

        model = transform.FindChild("Model").gameObject;
    }

    private void Update()
    {
        device = SteamVR_Controller.Input((int)motionCon.index);

        if (isHolding)
        {
            if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                currentHeldObject.ReleaseObject(motionCon, device);
                isHolding = false;
                triggerCollider.enabled = true;

                model.SetActive(true);
            }
        }

        if(device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad) && !isHolding)
        {
            pointer.enabled = !pointer.enabled;
        }

        if(pointer.enabled)
        {
            RaycastHit hit;

            if(Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit, 100, uiLayer))
            {
                if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
                    hit.collider.GetComponent<AmbuVR.Button>().UseButton();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<InteractableVR>() && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (other.GetComponent<InteractableVR>().isBeingHeld)
                return;

            other.GetComponent<InteractableVR>().HoldObject(holdPosition);

            currentHeldObject = other.GetComponent<InteractableVR>();
            triggerCollider.enabled = false;
            isHolding = true;

            model.SetActive(false);

            if (pointer.enabled)
                pointer.enabled = false;
        }
    }
}
