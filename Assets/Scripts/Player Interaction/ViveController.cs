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

    // Private ref fields
    LineRenderer pointer;
    UIController UI;

    // Private & Serialized fields
    [SerializeField] Transform pointerOrigin;
    [SerializeField] float pointerLength;
    [SerializeField] Transform holdPosition;
    [SerializeField] float lerpIntensity;

    // Private 
    int oldLayer;
    RaycastHit hit;
    [SerializeField]GameObject currentHeldObject;

    bool drawPointer;

    private void Awake()
    {
        // SteamVR ref
        motionCon = GetComponent<SteamVR_TrackedObject>();
        // references
        pointer = GetComponent<LineRenderer>();
        pointer.enabled = drawPointer =  false;

        UI = GameObject.FindWithTag("VariousController").GetComponent<UIController>();
        
    }

    private void Update()
    {
        device = SteamVR_Controller.Input((int)motionCon.index);
        DrawPointer();
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) || UI.IsUIEnabled)
        {
            Debug.Log("touching the touchpad");
            drawPointer = true;
        }
        if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("released touchpad");
            drawPointer = false;
        }
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            Debug.Log("touching the application menu");
            // show/hide in game menu
            if (!UI.ToggleUI())
                pointer.enabled = false;
        }

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger) && currentHeldObject == null)
        {
            // when aiming at an object it is highlighted
            // when trigger is pressed it hovers in front of the hand (portal gun)
            if(hit.collider.CompareTag("Pick Up") || hit.collider.CompareTag("Interactable"))
            {
                currentHeldObject = hit.collider.gameObject;
                currentHeldObject.transform.SetParent(holdPosition);

                if(currentHeldObject.layer != 2)
                    oldLayer = currentHeldObject.layer;
                currentHeldObject.layer = 2;

                if (currentHeldObject.GetComponent<Rigidbody>() != null)
                    currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && currentHeldObject != null)
        {
            if (currentHeldObject != null)
            {
                RaycastHit hit2;

                if (Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit2, 8))
                {
                    Debug.Log("Hitting " + hit.collider.gameObject.name + " while holding");

                    if (hit.collider.CompareTag("Interactable"))
                    {
                        currentHeldObject.transform.position = hit.point;
                        currentHeldObject.transform.SetParent(hit.collider.transform);
                    }
                }
                else
                    currentHeldObject.transform.SetParent(null);

                currentHeldObject.layer = oldLayer;

                Debug.Log(currentHeldObject.layer + " , " + oldLayer);

                if (currentHeldObject.GetComponent<Rigidbody>() != null)
                {
                    currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
                    ThrowVelocity(currentHeldObject.GetComponent<Rigidbody>());
                }
            }
        }

        //TODO
        // If you are holding something, send out a second raycast that ignores the held object

        
    }

    // Pointer

    private void DrawPointer()
    {
        pointer.enabled = true;

        if (drawPointer)
            pointer.SetPosition(0, pointerOrigin.position);
        else if (pointer.enabled)
            pointer.enabled = false;

        if (Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit, pointerLength))
        {
            if(drawPointer)
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
        currentHeldObject = null;
    }
}
