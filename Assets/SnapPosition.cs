using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPosition : MonoBehaviour {

    private ControllerManager cm;
    private MeshRenderer mr;

    private GameObject currentSnappedObj;

    private void Start()
    {
        cm = GameObject.Find("[CameraRig]").GetComponent<ControllerManager>();
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
    }

	private void OnTriggerStay(Collider other)
    {
        mr.enabled = true;

        if(other.CompareTag("Pick Up"))
        {
            if(cm.CanUseObject(other.gameObject))
            {
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
            currentSnappedObj = new GameObject();
    }
}
