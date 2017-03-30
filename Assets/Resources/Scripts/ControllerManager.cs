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
    /// <param name="id"></param>
    /// <returns></returns>
    public bool CanGrab(ControllerID id)
    {
        switch (id)
        {
            case ControllerID.LEFT:
                if (RIGHT.curConState == ControllerState.Holding)
                    return false;
                else return true;
            case ControllerID.RIGHT:
                if (LEFT.curConState == ControllerState.Holding)
                    return false;
                else return true;
            default:
                return true;
        }

    }
}
