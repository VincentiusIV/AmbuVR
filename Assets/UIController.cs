using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]Transform mainCamera;
    [SerializeField]GameObject UI;

    private bool enableState = false;

    private void Start()
    {
        UI.SetActive(false);
    }

    public bool ToggleUI()
    {
        // switch on
        if(!enableState)
        {
            enableState = true;
            UI.SetActive(true);
            // set rotation for ui to face camera
            return enableState;
        }
        // switch off
        else
        {
            enableState = false;
            UI.SetActive(false);
            return enableState;
        }
    }

    public bool IsUIEnabled
    {
        get { return enableState; }
    }
}
