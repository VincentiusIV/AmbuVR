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

    private void Awake()
    {
        LEFT.BootSequence(this);
        RIGHT.BootSequence(this);
        //Debug.Log(string.Format("{0} is {1}, {2} is {3}", LEFT, LEFT.curManState, RIGHT, RIGHT.curManState));
    }
    /// <summary>
    /// Checks if given controller id can grab an object
    /// Can only grab if other controller is not holding the same object
    /// </summary>
    /// <param name="id">ID of the controller requesting permission</param>
    /// <returns></returns>
    public bool CanGrab(ControllerID id, GameObject objToGrab)
    {
        switch (id)
        {
            case ControllerID.LEFT:
                if (RIGHT.currentHeldObject == objToGrab && LEFT.curConState != ControllerState.Holding)
                {
                    if (RIGHT.currentHeldObject == objToGrab)
                    Debug.Log("Objects are equal");
                    return false;
                }
                else return true;
            case ControllerID.RIGHT:
                if (LEFT.currentHeldObject == objToGrab && RIGHT.curConState != ControllerState.Holding)
                {
                    if (LEFT.currentHeldObject == objToGrab)
                        Debug.Log("Objects are equal");
                    return false;
                }
                else return true;
            default:
                return true;
        }

    }

    public bool CanTouch(ControllerID id)
    {
        switch (id)
        {
            case ControllerID.LEFT:
                return RIGHT.isTouching ? false : true;
            case ControllerID.RIGHT:
                return LEFT.isTouching ? false : true;
            default:
                return true;
        }
    }
}
