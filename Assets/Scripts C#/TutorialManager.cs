using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;

    [Header("Stage Order")]
    public List<TutorialStageData> eventList;

    public bool moveToNext;

    AudioSource sound;

    private void Start()
    {
        if (instance == null)
            instance = this;

        sound = GetComponent<AudioSource>();
        sound.loop = false;
        sound.playOnAwake = false;
    }

    private IEnumerator Tutorial()
    {
        Debug.Log("Tutorial has started");

        for (int i = 0; i < eventList.Count; i++)
        {
            Debug.Log(eventList[i].text);
            if(eventList[i].clip != null)
            {
                sound.clip = eventList[i].clip;
                sound.Play();
                yield return new WaitUntil(() => sound.isPlaying == false);
            }
            eventList[i].tutEvent.state = EventState.CurrentObjective;
            Debug.Log("Current objective is "+ eventList[i].tutEvent.gameObject.name);
            yield return new WaitUntil(() => moveToNext);
            moveToNext = false;
        }
        
        
        // Pick up object
        
    }
}
[System.Serializable]
public class TutorialStageData
{
    public string text;
    public AudioClip clip;
    public GamePlayEvent tutEvent;
}

