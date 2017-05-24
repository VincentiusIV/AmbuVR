using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EventState { Offline, CurrentObjective, Finished, Failed }

public class GamePlayEvent : MonoBehaviour {

    public EventState state;
    
    private void Start()
    {
        state = EventState.Offline;
    }

    public void EventFinished()
    {
        if (state != EventState.CurrentObjective)
            return;

        GameFlowManager.instance.moveToNext = true;
    }
}
