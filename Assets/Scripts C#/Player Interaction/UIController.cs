using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public bool isVisible;
    public bool isIntegerBeingRequested;

    private void Start()
    {
        if (instance == null)
            instance = this;

        bool isVisibleAtStart = onAwakeTutorial && SceneManager.GetActiveScene().name == "Tutorial" || onAwakeLevel && SceneManager.GetActiveScene().name == "GameFlowTesting";
        ToggleUI(isVisibleAtStart);
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
        // Activate knob UI
    }

    public void ConfirmRequest()
    {
        isIntegerBeingRequested = false;
    }

    public int CollectIntegerFromPlayer()
    {
        //Get value from knob
        // Dactivate 
        return 0;
    }

}
