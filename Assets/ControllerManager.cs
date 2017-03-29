using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ControllerState
{
    AimAtNothing, AimAtUI, AimAtTPSpot, AimAtObject, Holding, 
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
}
