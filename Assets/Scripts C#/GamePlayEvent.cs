using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EventState { Offline, CurrentObjective, Finished, Failed }

public class GamePlayEvent : MonoBehaviour {

    public EventState state;

    public bool gameFlow = true;
    public bool finishOnTrigger = false;

    private void Awake()
    {
        state = EventState.Offline;
    }

    public void SetActive()
    {
        state = EventState.CurrentObjective;
    }

    public void EventFinished()
    {
        if (state != EventState.CurrentObjective)
            return;

        state = EventState.Finished;

        if (gameFlow)
            GameFlowManager.instance.moveToNext = true;
        else TutorialManager.instance.moveToNext = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(state == EventState.CurrentObjective && finishOnTrigger && other.CompareTag("Player"))
        {
            EventFinished();
        }
    }
}
