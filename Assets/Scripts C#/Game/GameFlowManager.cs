using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;

    public List<GameStage> eventList = new List<GameStage>();

    public GameState state;

    public bool moveToNext = false;
    public bool isGameActive = false;

    public Text objectiveTxt;

    private void Start()
    {
        if (instance == null)
            instance = this;

        StartCoroutine(GameFlow());
    }

    public GamePlayEvent currentEvent;

    IEnumerator GameFlow()
    {
        isGameActive = true;

        for (int i = 0; i < eventList.Count; i++)
        {
            /// START DIALOGUE
            if (eventList[i].dialogueEventID != -1)
            {
                // Do dialogue and wait for it to finish
                state = GameState.Dialogue;

                if(DialogueController.instance.isActive)
                    DialogueController.instance.ForceQuitDialogue();

                yield return DialogueController.instance.StartCoroutine(DialogueController.instance.DialogueSession(eventList[i].dialogueEventID));
                NPCManager.instance.npcs[DialogueController.instance.talkingNPCID].ChangeBehaviour(AIState.Idle);
            }
            else Debug.Log(i + " Skipping dialogue cause event id is -1");

            if(eventList[i].gameEvent != null)
            {
                state = GameState.Event;
                Debug.Log("Current objective is: " + eventList[i].gameEvent.name);
                objectiveTxt.text = eventList[i].gameEvent.name;
                // Turn the new event on
                currentEvent = eventList[i].gameEvent;
                eventList[i].gameEvent.EnableEvent();
                eventList[i].gameEvent.state = EventState.CurrentObjective;
                yield return new WaitUntil(() => moveToNext);
                moveToNext = false;
                
            }  
        }
        state = GameState.Finished;
        Debug.Log("You finished the game! Congrats!");
        objectiveTxt.text = "Thanks for playing!";
        isGameActive = false;
    }
}

[System.Serializable]
public class GameStage
{
    public int dialogueEventID;
    public GamePlayEvent gameEvent;
}

public enum GameState
{
    Dialogue, Event, Finished
}


