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

    public GameObject currentHeldObject;
    public bool isHolding;

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
        DrawPointer();

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
    private void DrawPointer()
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
            string firstTag = hits[0].collider.tag;

            switch (firstTag)
            {
                case "Pick Up":
                    curConState = ControllerState.AimAtObject; break;
                case "Button":
                    curConState = ControllerState.AimAtUI; break;
                case "TP_Spot":
                    curConState = ControllerState.AimAtTPSpot; break;
                default:
                    break;
            }
        }
        else // When aiming at nothing; early exit;
        {
            curConState = ControllerState.AimAtNothing;
            pointer.SetPosition(1, pointerOrigin.position + (pointerOrigin.forward * pointerLength));
            return;
        }

        if (drawPointer)
            pointer.SetPosition(1, hits[0].point);

        foreach (RaycastHit hit in hits)
        {
            switch (curConState)
            {
                case ControllerState.AimAtUI:
                    UI_Check(hit); break;
                case ControllerState.AimAtTPSpot:
                    TP_Check(hit); break;
                case ControllerState.AimAtObject:
                    break;
                case ControllerState.Holding:
                    break;
                default:
                    break;
            }


            if (hit.collider.CompareTag("Burn"))
                return;

            else if (hit.collider.CompareTag("Patient"))
            {
                hit.transform.GetComponent<SkinTexture>().Highlight(hit.textureCoord);

                if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                    hit.transform.GetComponent<SkinTexture>().SetPixels(hit.textureCoord, true, hit.point);
            }
            else if (hit.collider.CompareTag("Pick Up") && !isHolding && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                isHolding = true;

                currentHeldObject = hit.collider.gameObject;
                currentHeldObject.transform.SetParent(holdPosition);

                if (currentHeldObject.layer != 2)
                    oldLayer = currentHeldObject.layer;
                currentHeldObject.layer = 2;

                if (currentHeldObject.GetComponent<Rigidbody>() != null)
                    currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && isHolding)
            {
                isHolding = false;
                RaycastHit hit2;

                if (Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit2, 8))
                {
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
                }
                else
                    currentHeldObject.transform.SetParent(null);

                currentHeldObject.layer = oldLayer;

                if (currentHeldObject.GetComponent<Rigidbody>() != null)
                {
                    currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
                    ThrowVelocity(currentHeldObject.GetComponent<Rigidbody>());
                }
            }
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
