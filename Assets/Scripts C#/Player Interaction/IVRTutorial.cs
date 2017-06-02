using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IVRTutorial : InteractableVR
{
    bool wasGrabbedOnce;

    public override void OnGrab()
    {
        base.OnGrab();

        if (wasGrabbedOnce)
            return;
        else wasGrabbedOnce = true;

        GetComponent<GamePlayEvent>().EventFinished();
    }

    public override void OnRelease()
    {
        base.OnRelease();

        GetComponent<GamePlayEvent>().EventFinished();
    }

    private void OnMouseDown()
    {
        if (wasGrabbedOnce)
            return;
        else wasGrabbedOnce = true;

        GetComponent<GamePlayEvent>().EventFinished();
    }

    private void OnMouseUp()
    {
        GetComponent<GamePlayEvent>().EventFinished();
    }
}
