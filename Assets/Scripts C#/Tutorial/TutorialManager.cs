using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;

    [Header("Intro")]
    public string introTxt;
    public AudioClip introClip;

    [Header("Stage Order")]
    public List<TutorialStageData> eventList;
    [HideInInspector]public bool moveToNext;

    [Header("Ref")]
    public Animator platform;
    public GameObject teleportingStage;

    //--- Private ---//
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
        Debug.Log(introTxt);
        if (introClip != null)
        {
            sound.clip = introClip;
            sound.Play();
            yield return new WaitForSeconds(introClip.length);
        }
            

        for (int i = 0; i < eventList.Count; i++)
        {
            Debug.Log(eventList[i].text);
            bool canTp = eventList[i].canPlayerTeleport;

            if (canTp)
            {
                AmbuVR.Player.instance.SetCanTeleport(canTp);
                platform.SetBool("Expand", canTp);
                yield return new WaitForSeconds(1f);
                teleportingStage.GetComponent<FadeObjects>().FadeIn();
            }
            

            if(eventList[i].clip != null)
            {
                sound.clip = eventList[i].clip;
                sound.Play();
            }

            if (eventList[i].tutEvent != null)
            {
                eventList[i].tutEvent.SetActive();
                Debug.Log("Current objective is " + eventList[i].tutEvent.gameObject.name);
                yield return new WaitUntil(() => moveToNext);
                Debug.Log(" Finished " + eventList[i].tutEvent.gameObject.name);
            }

            if(eventList[i].additionalWaitingTime > 0)
                yield return new WaitForSeconds(eventList[i].additionalWaitingTime);
            
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
    public float additionalWaitingTime;
    public bool canPlayerTeleport;
    public GamePlayEvent tutEvent;
}

