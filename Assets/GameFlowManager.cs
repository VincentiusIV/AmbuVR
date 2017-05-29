using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{

    public static GameFlowManager instance;

    public List<GamePlayEvent> eventList = new List<GamePlayEvent>();

    public bool moveToNext = false;
    private bool isGameActive = false;

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
            Debug.Log("Current objective is: " + eventList[i].name);
            eventList[i].state = EventState.CurrentObjective;
            yield return new WaitUntil(() => moveToNext);
            moveToNext = false;
            Debug.Log(eventList[i].name + " was completed!");
            eventList[i].state = EventState.Finished;
        }

        Debug.Log("You finished the game! Congrats!");
        isGameActive = false;
    }
}
