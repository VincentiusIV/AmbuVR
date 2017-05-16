using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[Serializable]
public enum ControllerID { LEFT = 0 , RIGHT = 1}
// Contains functionality for the motion controllers
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveController : MonoBehaviour, IManager
{
    // Public Getters
    public ManagerState curManState { get; private set; }
    public ControllerState curConState { get; private set; }
    public GameObject currentHeldObject { get; private set; }
    public bool isTouching { get; private set; }

    // Private SteamVR fields
    SteamVR_TrackedObject motionCon;
    SteamVR_Controller.Device device;

    // Private ref fields
    LineRenderer pointer;
    UIController UI;
    ControllerManager cm;
    TouchpadInterface ti;
    GameObject model;

    // Private & Serialized fields
    [SerializeField] ControllerID id;
    [SerializeField] Transform pointerOrigin;
    [SerializeField] float pointerLength;
    [SerializeField] Transform holdPosition;
    [SerializeField] float lerpIntensity;

    // Private 
    int oldLayer;
    Transform oldParent;

    public void BootSequence(ControllerManager _cm)
    {
        Debug.Log(string.Format("{0} {1} is booting up", GetType().Name, id));

        cm = _cm;   
        holdPosition = transform.Find("HoldPosition");
        // SteamVR ref
        motionCon = GetComponent<SteamVR_TrackedObject>();
        // references
        pointer = GetComponent<LineRenderer>();
        pointer.enabled = false;

        if(id == ControllerID.RIGHT)
            ti = transform.Find("HG_Interface").GetComponent<TouchpadInterface>();

        model = transform.Find("Model").gameObject;

        UI = GameObject.FindWithTag("VariousController").GetComponent<UIController>();

        isTouching = false;

        curManState = ManagerState.Completed;
        curConState = ControllerState.Aiming;
        Debug.Log(string.Format("{0} {1} status = {2}", GetType().Name, id, curManState));
    }

    private void Update()
    {
        if (curManState != ManagerState.Completed)
            return;
        
        device = SteamVR_Controller.Input((int)motionCon.index);

        if (id == ControllerID.RIGHT )
        {
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                ti.ToggleTI();
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                ti.SetSelectedOption(device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad));

                if (device.GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    ti.TouchpadPress();
                }
            }
            else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad) && curConState != ControllerState.Holding)
            {
                curConState = ControllerState.Aiming;
                ti.ToggleTI();
            }
        }

        if (curConState == ControllerState.Holding && currentHeldObject.name == "WaterBottle")
            currentHeldObject.GetComponent<WaterBottle>().SprinkleWater();


        AimChecking(device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) || UI.IsUIEnabled);

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            if (!UI.ToggleUI())
                pointer.enabled = false;
    }
    /// <summary>
    /// Checks what and how can be interacted based on the first 2 raycast hits & controller state 
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if(other != null && curConState != ControllerState.Holding)
            Grab_Check(other);
    }

    private void AimChecking(bool drawPointer)
    {
        pointer.enabled = true;

        if (drawPointer)
            pointer.SetPosition(0, pointerOrigin.position);
        else if (pointer.enabled)
            pointer.enabled = false;

        RaycastHit hit;

        if (Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit) || curConState == ControllerState.Holding)
        {
            if (drawPointer)
                pointer.SetPosition(1, hit.point);

            if (curConState == ControllerState.Holding)
            {
                Release_Check(hit);
                return;
            }
            for (int i = 0; i < (int)curConState; i++)
                switch (hit.collider.tag)
                {
                    case "Button":
                        UI_Check(hit); break;
                    /*case "Patient":
                        Paint_Check(hit); break;*/
                    case "Burn":
                    default:
                        return;
                }
        }
        else pointer.SetPosition(1, pointerOrigin.position + (pointerOrigin.forward * pointerLength));
    }
    /// <summary>
    /// Interaction with UI, checks when player clicks
    /// </summary>
    /// <param name="hit"></param>
    private void UI_Check(RaycastHit hit)
    {
        Debug.Log(string.Format("hit {0}, performing UI Check", hit.collider.tag));
        try
        {
            hit.collider.GetComponent<ButtonScript>().Highlight();
        }
        catch(NullReferenceException e)
        {
            Debug.Log("Hit collider is not UI or does not have a ButtonScript component; " + e.Message);
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            hit.collider.GetComponent<ButtonScript>().Click();
    }
    // Depracated way of teleporting, no longer needed
    /*private void TP_Check(RaycastHit hit)
    {
        Debug.Log(string.Format("hit {0}, performing TP Check", hit.collider.tag));
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            transform.parent.position = hit.point;
        }
            

        // TODO
        // upgrade functionality with fade to black animation,

    }*/
    /// <summary>
    /// Checks if player can grab something
    /// </summary>
    /// <param name="col"></param>
    private void Grab_Check(Collider col)
    {
        Debug.Log(string.Format("hit {0}, performing GRAB Check", col.tag));
        if (curConState == ControllerState.Holding)
            return;
        else if (cm.CanGrab(id, col.gameObject) && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            curConState = ControllerState.Holding;

            if(model.activeInHierarchy)
                model.SetActive(false);

            currentHeldObject = col.gameObject;
            currentHeldObject.transform.position = holdPosition.position;

            if (currentHeldObject.transform.parent != null)
                oldParent = currentHeldObject.transform.parent;

            currentHeldObject.transform.SetParent(holdPosition);

            if (currentHeldObject.layer != 2)
                oldLayer = currentHeldObject.layer;
            currentHeldObject.layer = 2;

            if (currentHeldObject.GetComponent<Rigidbody>() != null)
                currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            Debug.Log("Cannot grab the object the other controller is holding");
    }
    /// <summary>
    /// Check when the player releases a held object
    /// </summary>
    /// <param name="hit"></param>
    private void Release_Check(RaycastHit hit)
    {
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Releasing object: " + currentHeldObject.name);
            if (hit.collider != null && hit.collider.CompareTag("Interactable"))
            {
                currentHeldObject.transform.position = hit.point;
                currentHeldObject.transform.SetParent(hit.collider.transform);
            }
            //else if (hit.collider.CompareTag("Patient"))
            //    hit.collider.GetComponent<Patient>().AddObject(gameObject);
            else currentHeldObject.transform.SetParent(null);

            if (oldParent != null)
            {
                currentHeldObject.transform.SetParent(oldParent);
                oldParent = null;
            }

            currentHeldObject.layer = oldLayer;

            if (currentHeldObject.GetComponent<Rigidbody>() != null)
            {
                currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
                ThrowVelocity(currentHeldObject.GetComponent<Rigidbody>());
            }

            if (!model.activeInHierarchy)
                model.SetActive(true);

            currentHeldObject = null;
            curConState = ControllerState.Aiming;
        }
    }
    /// <summary>
    /// Checking if the player can paint burn wounds on patient
    /// </summary>
    /// <param name="hit"></param>
    private void Paint_Check(RaycastHit hit)
    {
        Debug.Log(string.Format("hit {0}, performing PAINT Check", hit.collider.tag));
        hit.transform.GetComponent<SkinTexture>().Highlight(hit.textureCoord);

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            hit.transform.GetComponent<SkinTexture>().SetPixels(hit.textureCoord, true, true, hit.point);
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
