using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;

    [Header("Stage Order")]
    public List<TutorialStageData> eventList;
    public bool moveToNext;

    public string introTxt;
    public AudioClip introClip;

    AudioSource sound;

    private void Start()
    {
        if (instance == null)
            instance = this;

        sound = GetComponent<AudioSource>();
        sound.loop = false;
        sound.playOnAwake = false;

        StartCoroutine(Tutorial());
    }

    private IEnumerator Tutorial()
    {
        Debug.Log("Tutorial has started");

        // Introduction


        for (int i = 0; i < eventList.Count; i++)
        {
            Debug.Log(eventList[i].text);

            if(eventList[i].clip != null)
            {
                sound.clip = eventList[i].clip;
                sound.Play();
            }

            eventList[i].tutEvent.state = EventState.CurrentObjective;
            Debug.Log("Current objective is "+ eventList[i].tutEvent.gameObject.name);
            yield return new WaitUntil(() => moveToNext);
            moveToNext = false;
        }

        Debug.Log("Tutorial Finished");
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

