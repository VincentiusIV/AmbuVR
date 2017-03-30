using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ControllerID { LEFT, RIGHT}
// Contains functionality for the motion controllers

// TODO
// Refactor code to:
// - Move functionality to different script, have only input being handled in this one
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveController : MonoBehaviour, IManager
{
    public ManagerState curManState { get; private set; }
    public ControllerState curConState { get; private set; }

    // Private SteamVR fields
    SteamVR_TrackedObject motionCon;
    SteamVR_Controller.Device device;

    // Private ref fields
    LineRenderer pointer;
    UIController UI;
    ControllerManager cm;

    // Private & Serialized fields
    [SerializeField] ControllerID id;
    [SerializeField] Transform pointerOrigin;
    [SerializeField] float pointerLength;
    [SerializeField] Transform holdPosition;
    [SerializeField] float lerpIntensity;

    // Private 
    int oldLayer;

    public GameObject currentHeldObject { get; private set; }
    // other controller
    bool drawPointer;

    public void BootSequence(ControllerManager _cm)
    {
        Debug.Log(string.Format("{0} {1} is booting up", GetType().Name, id));

        cm = _cm;
        holdPosition = transform.FindChild("HoldPosition");
        // SteamVR ref
        motionCon = GetComponent<SteamVR_TrackedObject>();
        // references
        pointer = GetComponent<LineRenderer>();
        pointer.enabled = drawPointer = false;

        UI = GameObject.FindWithTag("VariousController").GetComponent<UIController>();
        curManState = ManagerState.Completed;
        Debug.Log(string.Format("{0} {1} status = {2}", GetType().Name, id, curManState));
    }

    private void Update()
    {
        // TODO:
        // Touchpad swipes, more UI control
        device = SteamVR_Controller.Input((int)motionCon.index);
        AimChecking();

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) || UI.IsUIEnabled)
        {
            drawPointer = true;
        }
        else if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            drawPointer = false;
        }
        else if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            if (!UI.ToggleUI())
                pointer.enabled = false;
        }

        
    }
    // Pointer
    private void AimChecking()
    {
        pointer.enabled = true;

        if (drawPointer)
            pointer.SetPosition(0, pointerOrigin.position);
        else if (pointer.enabled)
            pointer.enabled = false;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(pointerOrigin.position, pointerOrigin.forward, pointerLength);

        if (hits.Length > 0)
        {
            if (drawPointer)
                pointer.SetPosition(1, hits[0].point);

            for (int i = 0; i < (int)curConState; i++)
            {
                switch (hits[(i].collider.tag)
                {
                    case "Pick Up":
                        Grab_Check(hits[i]); break;
                    case "Button":
                        UI_Check(hits[i]); break;
                    case "TP_Spot":
                        TP_Check(hits[i]); break;
                    case "Patient":
                        Paint_Check(hits[i]); break;
                    default:
                        Release_Check(hits[i]); break;
                }
            }
        }
        else
        {
            pointer.SetPosition(1, pointerOrigin.position + (pointerOrigin.forward * pointerLength));
        }
    }
    /// <summary>
    /// Interaction with UI, checks when player clicks
    /// </summary>
    /// <param name="hit"></param>
    private void UI_Check(RaycastHit hit)
    {
        hit.collider.GetComponent<ButtonScript>().Highlight();

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            hit.collider.GetComponent<ButtonScript>().Click();
    }
    /// <summary>
    /// Checks if player can teleport, and teleports
    /// </summary>
    /// <param name="hit"></param>
    private void TP_Check(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == 10 && device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
            transform.parent.position = hit.point;

        // TODO
        // upgrade functionality with fade to black animation, 

    }
    /// <summary>
    /// Checks if player can grab something
    /// </summary>
    /// <param name="hit"></param>
    private void Grab_Check(RaycastHit hit)
    {
        if (curConState != ControllerState.Holding && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            curConState = ControllerState.Holding;

            currentHeldObject = hit.collider.gameObject;
            currentHeldObject.transform.SetParent(holdPosition);

            if (currentHeldObject.layer != 2)
                oldLayer = currentHeldObject.layer;
            currentHeldObject.layer = 2;

            if (currentHeldObject.GetComponent<Rigidbody>() != null)
                currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    /// <summary>
    /// Check when the player releases a held object
    /// </summary>
    /// <param name="hit"></param>
    private void Release_Check(RaycastHit hit)
    {
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && curConState == ControllerState.Holding)
        {
            curConState = ControllerState.Aiming;

            if (hit.collider.CompareTag("Interactable"))
            {
                // prob rewrite this into a interactable
                currentHeldObject.transform.position = hit.point;
                currentHeldObject.transform.SetParent(hit.collider.transform);
            }
            else if (hit.collider.CompareTag("Patient"))
                hit.collider.GetComponent<Patient>().AddObject(gameObject);

            if (currentHeldObject.transform.parent != hit.collider.transform)
                currentHeldObject.transform.SetParent(null);

            currentHeldObject.transform.SetParent(null);

            currentHeldObject.layer = oldLayer;

            if (currentHeldObject.GetComponent<Rigidbody>() != null)
            {
                currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
                ThrowVelocity(currentHeldObject.GetComponent<Rigidbody>());
            }
        }
    }
    /// <summary>
    /// Checking if the player can paint burn wounds on patient
    /// </summary>
    /// <param name="hit"></param>
    private void Paint_Check(RaycastHit hit)
    {
        hit.transform.GetComponent<SkinTexture>().Highlight(hit.textureCoord);

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            hit.transform.GetComponent<SkinTexture>().SetPixels(hit.textureCoord, true, hit.point);
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
        currentHeldObject = null;
    }
}
