using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI controller for going back to menu, pausing game, settings
/// </summary>
public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Start()
    {
        if (instance == null)
            instance = this;

    }

    public void ToggleUI()
    {

    }
}
