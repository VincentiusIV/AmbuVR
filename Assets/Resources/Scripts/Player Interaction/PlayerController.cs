using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public enum FPS_State { LOCKED, UNLOCKED }
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour {

    [SerializeField]private float speed = 10.0f;

    // Private & Serialized fields
    [SerializeField] Transform pointerOrigin;
    [SerializeField] float pointerLength;

    private FPS_State state;
    private CameraController cam;

    ControllerState curConState;
    GameObject currentHeldObject;
    Transform oldParent;
    public Transform holdPosition;
    int oldLayer;

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        state = FPS_State.UNLOCKED;

        cam = transform.GetChild(0).GetComponent<CameraController>();
        curConState = ControllerState.Aiming;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(state == FPS_State.UNLOCKED)
        {
            float vPos = Input.GetAxisRaw("Vertical") * speed;
            float hPos = Input.GetAxisRaw("Horizontal") * speed;

            Vector3 newPos = new Vector3(hPos, 0f, vPos) * Time.deltaTime;
            transform.Translate(newPos);

            cam.RotateCamera(state);
        }
        
        if (Input.GetButtonDown("Cancel"))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
        }

        if(Input.GetButtonDown("Submit"))
        {
            if (state == FPS_State.UNLOCKED)
                state = FPS_State.LOCKED;
            else state = FPS_State.UNLOCKED;

            Debug.Log(string.Format("Switching state to: {0}", state));
        }

        AimChecking();
	}
    

    private void AimChecking()
    {
        RaycastHit hit;

        if (Physics.Raycast(pointerOrigin.position, pointerOrigin.forward, out hit) || curConState == ControllerState.Holding)
        {
            if (curConState == ControllerState.Holding)
            {
                Release_Check(hit);
                return;
            }
            for (int i = 0; i < (int)curConState; i++)
                switch (hit.collider.tag)
                {
                    case "Pick Up":
                    case "VR_Controller":
                        Grab_Check(hit.collider); break;
                    case "Button":
                        UI_Check(hit); break;
                    case "TP_Spot":
                        TP_Check(hit); break;
                    case "Patient":
                        Paint_Check(hit); break;
                    case "Burn":
                    default:
                        return;
                }
        }
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
        catch (NullReferenceException e)
        {
            Debug.Log("Hit collider is not UI or does not have a ButtonScript component; " + e.Message);
        }

        if (Input.GetButtonDown("Fire1"))
            hit.collider.GetComponent<ButtonScript>().Click();
    }
    /// <summary>
    /// Checks if player can teleport, and teleports
    /// </summary>
    /// <param name="hit"></param>
    private void TP_Check(RaycastHit hit)
    {
        Debug.Log(string.Format("hit {0}, performing TP Check", hit.collider.tag));
        if (Input.GetButtonDown("Jump"))
        {
            transform.parent.position = hit.point;
        }


        // TODO
        // upgrade functionality with fade to black animation,

    }
    /// <summary>
    /// Checks if player can grab something
    /// </summary>
    /// <param name="col"></param>
    private void Grab_Check(Collider col)
    {
        Debug.Log(string.Format("hit {0}, performing GRAB Check", col.tag));
        if (curConState == ControllerState.Holding)
            return;
        else if (Input.GetButtonDown("Fire1"))
        {
            curConState = ControllerState.Holding;

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
        else if (Input.GetButtonDown("Fire1"))
            Debug.Log("Cannot grab the object the other controller is holding");
    }
    /// <summary>
    /// Check when the player releases a held object
    /// </summary>
    /// <param name="hit"></param>
    private void Release_Check(RaycastHit hit)
    {
        if (Input.GetButtonUp("Fire1"))
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
            }

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
        Transform obj;
        Debug.Log(string.Format("hit {0}, performing PAINT Check", hit.collider.tag));
        /* if (hit.transform.parent != null)
             obj = GetHighestParent(hit.transform);
         else obj = hit.transform;*/
        SkinTexture hitST = hit.transform.GetComponent<SkinTexture>();
        if (hitST == null)
            return;

        hit.transform.GetComponent<SkinTexture>().Highlight(hit.textureCoord);

        if (Input.GetButton("Fire1"))
            hit.transform.GetComponent<SkinTexture>().SetPixels(hit.textureCoord, true, true, hit.point);
    }

    Transform GetHighestParent(Transform child)
    {
        Transform parent = child;
        while (parent.parent != null)
        {
            if (parent.parent != null)
                parent = parent.parent;
            else break;
        }
        return parent;
    }
}
