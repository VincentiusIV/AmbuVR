using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class SnapPosition : MonoBehaviour {

    private ControllerManager cm;
    private MeshRenderer mr;

    private GameObject currentSnappedObj;
    private bool isFull = false;

    private void Start()
    {
        try
        {
            cm = GameObject.Find("[CameraRig]").GetComponent<ControllerManager>();
        }
        catch(NullReferenceException)
        {
            throw new Exception("There is no CameraRig in the scene");
        }
        
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
    }

	private void OnTriggerStay(Collider other)
    {
        if (isFull)
            return;

        mr.enabled = true;

        if(other.CompareTag("Pick Up"))
        {
            if(cm.CanUseObject(other.gameObject))
            {
                isFull = true;
                currentSnappedObj = other.gameObject;
                other.transform.position = transform.position;
                other.transform.SetParent(transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        mr.enabled = false;

        if (other.gameObject == currentSnappedObj)
        {
            currentSnappedObj = new GameObject();
            isFull = false;
        }    
    }
}
*/