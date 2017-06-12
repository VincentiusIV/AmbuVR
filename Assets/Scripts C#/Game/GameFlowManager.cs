using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;

    public List<GameStage> eventList = new List<GameStage>();

    public bool moveToNext = false;
    public bool isGameActive = false;

    public Text objectiveTxt;

    private void Start()
    {
        if (instance == null)
            instance = this;

        StartCoroutine(GameFlow());
    }

    IEnumerator GameFlow()
    {
        isGameActive = true;

        for (int i = 0; i < eventList.Count; i++)
        {
            /// START DIALOGUE
            if (eventList[i].dialogueEventID != -1)
            {
                // Before anything begins, the ai that is supposed to speak has to walk to the player
                NPCManager.instance.npcs[DialogueController.instance.talkingNPCID].ChangeBehaviour(AIState.Follow);
                // Wait untill npc has reached player
                yield return new WaitUntil(() => NPCManager.instance.npcs[DialogueController.instance.talkingNPCID].reachedPlayer);           
                // Do dialogue and wait for it to finish
                yield return DialogueController.instance.StartCoroutine(DialogueController.instance.DialogueSession(eventList[i].dialogueEventID));
                AmbuVR.Player.instance.SetCanTeleport(true);
            }
            Debug.Log("Current objective is: " + eventList[i].gameEvent.name);
            objectiveTxt.text = eventList[i].gameEvent.name;
            eventList[i].gameEvent.SetActive();
            yield return new WaitUntil(() => moveToNext);
            moveToNext = false;
            Debug.Log(eventList[i].gameEvent.name + " was completed!");
            eventList[i].gameEvent.state = EventState.Finished;
        }

        Debug.Log("You finished the game! Congrats!");
        isGameActive = false;
    }
}

[System.Serializable]
public class GameStage
{
    public int dialogueEventID;
    public GamePlayEvent gameEvent;
}

