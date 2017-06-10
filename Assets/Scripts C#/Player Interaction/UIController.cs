using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI controller for going back to menu, pausing game, settings
/// </summary>
public class UIController : MonoBehaviour
{
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

    //--- Private ---//
    public float inputValue;

    private void Start()
    {
        if (instance == null)
            instance = this;

        bool isVisibleAtStart = onAwakeTutorial && SceneManager.GetActiveScene().name == "Tutorial" || onAwakeLevel && SceneManager.GetActiveScene().name == "GameFlowTesting";
        ToggleUI(isVisibleAtStart);

        if (valueKnob != null)
        {
            valueKnob.SetActive(false);
        }
        else Debug.LogError("Assing the value knob to the UI controller!");
    }

    public void ToggleUI(bool visible)
    {
        isVisible = visible;
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == null)
                buttons[i] = transform.GetChild(i).GetComponent<AmbuVR.Button>();
            buttons[i].gameObject.SetActive(visible);
        }
    }

    public void RequestIntegerFromPlayer(string text)
    {
        Debug.Log("Integer is requested from player");
        // Activate knob UI
        isIntegerBeingRequested = true;
        valueKnob.SetActive(isIntegerBeingRequested);
        valueLabel.text = text;
    }

    public void ConfirmRequest()
    {
        isIntegerBeingRequested = false;
        inputValue = knob.value;
        valueKnob.SetActive(isIntegerBeingRequested);
    }
}
