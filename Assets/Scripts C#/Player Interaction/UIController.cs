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


}
