using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum EventState { Offline, CurrentObjective, Finished, Failed }

public class GamePlayEvent : MonoBehaviour {

    public EventState state;

    public bool gameFlow = true;
    public bool finishOnTrigger = false;

    public UnityEvent OnSetActive;
    public UnityEvent OnEventFinished;

    private void Awake()
    {
        state = EventState.Offline;
        OnSetActive.AddListener(SetActive);
        OnEventFinished.AddListener(EventFinished);
    }

    public void SetActive()
    {
        Debug.Log(gameObject.name + " is the current event");
        state = EventState.CurrentObjective;
        OnSetActive.Invoke();
    }

    public void EventFinished()
    {
        if (state != EventState.CurrentObjective)
            return;

        state = EventState.Finished;

        if (gameFlow)
            GameFlowManager.instance.moveToNext = true;
        else TutorialManager.instance.moveToNext = true;

        OnEventFinished.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if(state == EventState.CurrentObjective && finishOnTrigger && other.CompareTag("Player"))
        {
            EventFinished();
        }
    }
}
