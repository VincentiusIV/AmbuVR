﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IVRTutorial : InteractableVR
{
    public bool wasGrabbedOnce;
    public bool wasReleasedOnce;

    GamePlayEvent gameEvent;

    private void Start()
    {
        gameEvent = GetComponent<GamePlayEvent>();
    }

    public override void OnGrabVirtual()
    {
        base.OnGrabVirtual();
        if (gameEvent.state != EventState.CurrentObjective)
            return;

        if (wasGrabbedOnce)
            return;
        else wasGrabbedOnce = true;

        gameEvent.EventFinished();
    }

    public override void OnReleaseVirtual()
    {
        base.OnReleaseVirtual();

        if (gameEvent.state != EventState.CurrentObjective)
            return;

        if (wasReleasedOnce)
            return;
        else wasReleasedOnce = true;

        gameEvent.EventFinished();
    }

    private void OnMouseDown()
    {
        if (gameEvent.state != EventState.CurrentObjective)
            return;

        if (wasGrabbedOnce)
            return;
        else wasGrabbedOnce = true;

        gameEvent.EventFinished();
    }

    private void OnMouseUp()
    {
        if (gameEvent.state != EventState.CurrentObjective)
            return;

        if (wasReleasedOnce)
            return;
        else wasReleasedOnce = true;

        gameEvent.EventFinished();
    }

}
