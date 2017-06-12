using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum EventState { Offline, CurrentObjective, Finished, Failed }

public class GamePlayEvent : MonoBehaviour {

    public EventState state;

    public bool gameFlow = true;
    public bool finishOnNear = false;
    public float distanceToAct = 2f;

    public UnityEvent OnSetActive;
    public UnityEvent OnEventFinished;

    //--- Private ---//
    Transform player;
    bool alreadyNear;

    private void Start()
    {
        player = AmbuVR.Player.instance.hmdPosition;
    }

    private void Update()
    {
        if(finishOnNear)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if(distance <= distanceToAct)
            {
                if(!alreadyNear)
                {
                    alreadyNear = true;
                    EventFinished();
                }
            }
            else
            {
                alreadyNear = false;
            }
        }
    }

    public void EnableEvent()
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
        Debug.Log(gameObject.name + " objective is finished!");
        if (gameFlow)
            GameFlowManager.instance.moveToNext = true;
        else TutorialManager.instance.moveToNext = true;

        OnEventFinished.Invoke();
    }

}
