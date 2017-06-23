using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// UI controller for going back to menu, pausing game, settings
/// </summary>
public class UIController : MonoBehaviour
{
    //--- Public ---//
    public static UIController instance;

    public bool onAwakeTutorial = true;
    public bool onAwakeLevel = false;

    public List<AmbuVR.Button> buttons;

    [Header("Value Input")]
    public GameObject valueKnob;
    public Text valueLabel;
    public InteractableVR knob;

    public bool isVisible;
    public bool isIntegerBeingRequested;
    public bool teleportAnyway;

    [Header("Scene Options")]
    public string gSceneName = "GameFlowTesting";

    public string tSceneName = "Tutorial";

    //--- Private ---//
    public float inputValue;

    [Header("Dialogue")]
    public DialogueButton[] responseButtons;
    public Follow followScript;

    List<GameObject> visibleButtons = new List<GameObject>();

    private void Start()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        bool isVisibleAtStart = onAwakeTutorial && SceneManager.GetActiveScene().name == "Tutorial" || onAwakeLevel && SceneManager.GetActiveScene().name == "GameFlowTesting";
        ToggleUI(isVisibleAtStart);

        if (valueKnob != null)
        {
            valueKnob.SetActive(false);
        }
        else Debug.LogError("Assing the value knob to the UI controller!");

        for (int i = 0; i < responseButtons.Length; i++)
        {
            responseButtons[i].option = i;
        }
        ToggleResponseUI(false);
    }

    private void Update()
    {
        isVisible = visibleButtons.Count > 0;
    }

    public void ToggleUI(bool visible)
    {
        isVisible = visible;
        for (int i = 0; i < buttons.Count; i++)
        {
            ToggleManually(buttons[i].gameObject, visible);
        }
    }

    public void RequestIntegerFromPlayer(string text)
    {
        Debug.Log("Integer is requested from player");
        // Activate knob UI
        isIntegerBeingRequested = true;
        valueKnob.SetActive(isIntegerBeingRequested);
        valueLabel.text = text;
        isVisible = true;
    }

    public void ConfirmRequest()
    {
        isIntegerBeingRequested = false;
        inputValue = knob.value;
        valueKnob.SetActive(isIntegerBeingRequested);
        isVisible = false;
        GameFlowManager.instance.currentEvent.EventFinished();
    }

    public void UpdateResponses(Response[] responses, Transform npcToLookAt, bool followHmd = true)
    {
        for (int i = 0; i < responses.Length; i++)
        {
            responseButtons[i].textMesh.text = responses[i].ResponseText;
        }
        ToggleResponseUI(true, responses.Length);

       
        if(followHmd)
        {
            followScript.enabled = true;
        } 
        else if(!followHmd)
        {
            followScript.enabled = false;
            // Aim response ui to the NPC
            Vector3 direction = npcToLookAt.position - transform.parent.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.parent.rotation = rotation;

            // Position the ui at the player
            Vector3 hmd = AmbuVR.Player.instance.hmd.position;
            transform.parent.position = new Vector3(hmd.x, transform.parent.parent.position.y, hmd.z);
        }
        
    }

    public void ToggleResponseUI(bool state, int amount = 4)
    {
        for (int i = 0; i < amount; i++)
        {
            ToggleManually(responseButtons[i].gameObject, state);
            ToggleManually(responseButtons[i].transform.GetChild(0).gameObject, state);
        }
    }

    public void ToggleManually(GameObject button, bool state)
    {
        button.SetActive(state);
        
        if(state && !visibleButtons.Contains(button))
        {
            visibleButtons.Add(button);
        }
        else if(!state && visibleButtons.Contains(button))
        {
            visibleButtons.Remove(button);
        }
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene(gSceneName);
        SteamVR_LoadLevel.Begin(gSceneName);
        instance.ToggleUI(false);
    }

    public void PlayTutorial()
    {
        if (SceneManager.GetActiveScene().name == gSceneName)
            //SceneManager.LoadScene(tSceneName);
            SteamVR_LoadLevel.Begin(tSceneName);
        else TutorialManager.instance.StartTutorial();
    }
}
