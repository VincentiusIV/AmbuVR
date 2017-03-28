using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerID { LEFT, RIGHT}
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
    [SerializeField] ViveController otherController;
    bool drawPointer;

    private void Awake()
    {
        holdPosition = transform.FindChild("HoldPosition");
        // SteamVR ref
        motionCon = GetComponent<SteamVR_TrackedObject>();
        // references
        pointer = GetComponent<LineRenderer>();
        pointer.enabled = drawPointer =  false;

        UI = GameObject.FindWithTag("VariousController").GetComponent<UIController>();

        // get reference to other controller
        if (id == ControllerID.LEFT)
            otherController = transform.parent.FindChild(ControllerID.RIGHT.ToString()).GetComponent<ViveController>();
        else otherController = transform.parent.FindChild(ControllerID.LEFT.ToString()).GetComponent<ViveController>();
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
        if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            drawPointer = false;
        }
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
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
            if (drawPointer)
                pointer.SetPosition(1, hits[0].point);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.layer == 10 && device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
                    transform.parent.position = hit.point;

                else if (hit.collider.CompareTag("Button"))
                {
                    hit.collider.GetComponent<ButtonScript>().Highlight();

                    if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
                        hit.collider.GetComponent<ButtonScript>().Click();
                }
                else if (hit.collider.CompareTag("Burn"))
                    return;

                else if (hit.collider.CompareTag("Patient"))
                {
                    hit.transform.GetComponent<SkinTexture>().Highlight(hit.textureCoord);

                    if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                        hit.transform.GetComponent<SkinTexture>().SetPixels(hit.textureCoord, true, hit.point);
                }
                else if (hit.collider.CompareTag("Pick Up") && !isHolding && !otherController.isHolding && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
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
