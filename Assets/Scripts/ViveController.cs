using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains functionality for the motion controllers
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveController : MonoBehaviour
{
    SteamVR_TrackedObject motionCon;
    SteamVR_Controller.Device device;

    // animator
    //private Animator anime;
    LineRenderer pointer;
    [SerializeField]Transform pointerOrigin;
    [SerializeField]float pointerLength;

    private void Awake()
    {
        motionCon = GetComponent<SteamVR_TrackedObject>();
        // anime = Getcom..
        pointer = GetComponent<LineRenderer>();
        pointer.enabled = false;
    }

    private void Update()
    {
        device = SteamVR_Controller.Input((int)motionCon.index);

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("touching the touchpad");
            DrawPointer();
        }
        if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("released touchpad");
            pointer.enabled = false;
        }

        if (device.GetTouch(SteamVR_Controller.ButtonMask.ApplicationMenu))
            Debug.Log("touching the application menu");
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
        }
        else
        {
            pointer.SetPosition(1, pointerOrigin.position + (pointerOrigin.forward * pointerLength));
        }
    }
}
