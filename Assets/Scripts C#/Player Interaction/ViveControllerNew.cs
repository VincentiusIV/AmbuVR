﻿using System;
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
    bool canGrabAgain = true;

    private void Start()
    {
        motionCon = GetComponent<SteamVR_TrackedObject>();

        triggerCollider = GetComponent<SphereCollider>();
        triggerCollider.enabled = true;

        pointer = GetComponent<LineRenderer>();
        pointer.enabled = false;

        model = transform.Find("Model").gameObject;
    }

    private void Update()
    {
        device = SteamVR_Controller.Input((int)motionCon.index);

        if (isHolding)
        {
            if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                Transform origin = motionCon.origin ? motionCon.origin : motionCon.transform.parent;
                Vector3 velocity;
                Vector3 angularVelocity;

                if (origin != null)
                {
                    velocity = origin.TransformVector(device.velocity);
                    angularVelocity = origin.TransformVector(device.angularVelocity);
                }
                else
                {
                    velocity = device.velocity;
                    angularVelocity = device.angularVelocity;
                }

                currentHeldObject.DisconnectFromObject(velocity, angularVelocity);

                isHolding = false;
                triggerCollider.enabled = true;

                model.SetActive(true);
            }
        }

        if (canGrabAgain == false && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            canGrabAgain = true;

        if(device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad) && !isHolding)
        {
            pointer.enabled = !pointer.enabled;
        }

        if(pointer.enabled)
        {
            RaycastHit hit;
            pointer.SetPosition(0, pointerOrigin.position);

            if (Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit, 100, uiLayer))
            {
                pointer.SetPosition(1, hit.point);

                hit.collider.GetComponent<AmbuVR.Button>().OnPointerOver();

                if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
                    hit.collider.GetComponent<AmbuVR.Button>().UseButton();
            }
            else pointer.SetPosition(1, pointerOrigin.position + (pointerOrigin.forward * 100));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<InteractableVR>() && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && canGrabAgain)
        {
            if (other.GetComponent<InteractableVR>().isBeingHeld)
                return;

            other.GetComponent<InteractableVR>().ConnectToObject(holdPosition);

            currentHeldObject = other.GetComponent<InteractableVR>();
            triggerCollider.enabled = false;
            isHolding = true;

            model.SetActive(false);
            canGrabAgain = false;

            if (pointer.enabled)
                pointer.enabled = false;
        }
    }
}
