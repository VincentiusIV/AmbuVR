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

    private void Start()
    {
        if (instance == null)
            instance = this;

        bool isVisible = onAwakeTutorial && SceneManager.GetActiveScene().name == "" || onAwakeLevel && SceneManager.GetActiveScene().name == "";
    }

    public void ToggleUI(bool visible)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(visible);
        }
    }
}
