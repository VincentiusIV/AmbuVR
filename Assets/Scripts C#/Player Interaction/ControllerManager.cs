using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[Serializable]
public enum ControllerState
{
    Aiming = 1, Holding = 2
}

public class ControllerManager : MonoBehaviour
{
    [SerializeField] ViveController LEFT;
    [SerializeField] ViveController RIGHT;

    private ViveController[] controllers;
    //private DialogueController dc;

    private void Awake()
    {
        controllers = new ViveController[2];
        controllers[0] = LEFT;
        controllers[1] = RIGHT;

        for (int i = 0; i < 2; i++)
        {
            controllers[i].BootSequence(this);
        }

        //dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        //Debug.Log(string.Format("{0} is {1}, {2} is {3}", LEFT, LEFT.curManState, RIGHT, RIGHT.curManState));
    }
    /// <summary>
    /// Checks if given controller id can grab an object
    /// Can only grab if other controller is not holding the same object
    /// </summary>
    /// <param name="_id">ID of the controller requesting permission</param>
    /// <returns></returns>
    public bool CanGrab(ControllerID _id, GameObject objToGrab)
    {
        int otherID;
        if (_id == ControllerID.LEFT)
            otherID = (int)ControllerID.RIGHT;
        else otherID = (int)ControllerID.LEFT;

        GameObject otherHeldObj = controllers[otherID].currentHeldObject;
        if (otherHeldObj != objToGrab)
        {
            if (otherHeldObj != null && otherHeldObj.tag == "Patient" && objToGrab.tag == "Patient")
                return false;
            return true;
        }
        else return false;
    }

    // returns the gameobjects that are currently being held by the player
    public bool CanUseObject(GameObject objToCheck)
    {
        GameObject[] heldObjs = new GameObject[2];

        for (int i = 0; i < 2; i++)
            if (controllers[i].currentHeldObject == objToCheck)
                return false;

        return true;
    }
}
