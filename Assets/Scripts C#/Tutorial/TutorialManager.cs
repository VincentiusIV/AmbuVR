using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;

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

    }

    public void StartTutorial()
    {
        StartCoroutine(Tutorial());
    }

    private IEnumerator Tutorial()
    {
        Debug.Log("Tutorial has started");
        UIController.instance.ToggleUI(false);

        for (int i = 0; i < eventList.Count; i++)
        {
            Debug.Log(eventList[i].text);
            bool canTp = eventList[i].canPlayerTeleport;

            if (canTp)
            {
                AmbuVR.Player.instance.SetCanTeleport(canTp);   
                yield return new WaitForSeconds(1f);
                teleportingStage.GetComponent<FadeObjects>().FadeIn();
            }
            platform.SetBool("Expand", canTp);

            if (eventList[i].clip != null)
            {
                sound.clip = eventList[i].clip;
                sound.Play();
                yield return new WaitUntil(() => sound.isPlaying == false);
            }

            ControllerHint.instance.ShowHint(eventList[i].hintPart);

            if (eventList[i].tutEvent != null)
            {
                eventList[i].tutEvent.EnableEvent();
                Debug.Log("Current objective is " + eventList[i].tutEvent.gameObject.name);
                yield return new WaitUntil(() => moveToNext);
                Debug.Log(" Finished " + eventList[i].tutEvent.gameObject.name);
            }

            if(eventList[i].additionalWaitingTime > 0)
                yield return new WaitForSeconds(eventList[i].additionalWaitingTime);
            
            moveToNext = false;
        }

        GameObject.FindWithTag("Player").transform.position = Vector3.zero;
        platform.SetBool("Expand", false);
        AmbuVR.Player.instance.SetCanTeleport(false);
        teleportingStage.GetComponent<FadeObjects>().FadeOut();
        Debug.Log("Tutorial Finished");
        // Pick up object
        UIController.instance.ToggleUI(true);
    }
}
[System.Serializable]
public class TutorialStageData
{
    public string text;
    public AudioClip clip;
    public ControlPart hintPart;
    public float additionalWaitingTime;
    public bool canPlayerTeleport;
    public GamePlayEvent tutEvent;
}

