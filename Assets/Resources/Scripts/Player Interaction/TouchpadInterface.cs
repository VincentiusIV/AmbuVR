﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TouchpadState { Default = 0, DialogueSelect = 8, Numpad = 4}

[System.Serializable]
public enum TIButtonMask { Option1 = 0, Option2 = 1, Option3 = 2, Option4 = 3 }

[System.Serializable]
public enum TIButtonFunction {  Say = 0, TBSA = 1, PlaceHolder = 2, Placeholder = 3,
                                Plus = 4, Minus = 5, Enter = 6, Back = 7,
                             }

public class TouchpadInterface : MonoBehaviour {

    public bool isActive { get; private set; }
    private TouchpadState state { get; set; }
    private TIButtonMask currentSelection { get; set; }
    // Serialized
    [SerializeField] private int amountOfOptions = 4;
    [SerializeField] private float uiRadius = 1;
    [SerializeField] private GameObject uiPrefab;
    // Reference
    public GameObject mainPanel;
    public List<GameObject> panels;
    private TextMesh displayText;
    public DialogueController dc;
    private MeshRenderer mr;

    public void Awake()
    {
        //obj = transform.GetChild(0).gameObject;
        //dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        displayText = transform.GetChild(0).GetComponent<TextMesh>();
        mr = GetComponent<MeshRenderer>();

        currentSelection = TIButtonMask.Option1;
    }

    public void ToggleTI()
    {
        if (mainPanel.activeInHierarchy)
            return;
        mainPanel.SetActive(isActive = !isActive);
    }

    public void ConfigureMenu(TouchpadState newState)
    {
        state = newState;
        displayText.text = newState.ToString();

        int start = (int)newState;
        
        for (int i = start; i < start + 4; i++)
        {
            panels[i].transform.GetChild(0).GetComponent<TextMesh>().text = ((TIButtonFunction)i).ToString();
        }
    }

    public void SetSelectedOption(Vector2 touchpadCoord )
    {
        panels[(int)currentSelection].GetComponent<Renderer>().material.color = Color.white;
        if (touchpadCoord.x < 0 && touchpadCoord.y > 0)
            currentSelection = TIButtonMask.Option1;
        else if (touchpadCoord.x > 0 && touchpadCoord.y > 0)
            currentSelection = TIButtonMask.Option2;
        else if (touchpadCoord.x < 0 && touchpadCoord.y < 0)
            currentSelection = TIButtonMask.Option3;
        else if (touchpadCoord.x > 0 && touchpadCoord.y < 0)
            currentSelection = TIButtonMask.Option4;
        panels[(int)currentSelection].GetComponent<Renderer>().material.color = Color.black;
    }

    // Handles touchpad press
    public void TouchpadPress()
    {
        switch (state)
        {
            case TouchpadState.Default:
                DefaultPress(); break;
            case TouchpadState.DialogueSelect:
                dc.Interact_Dialogue((int)currentSelection); break;
            case TouchpadState.Numpad:
                UpdateNumpad((int)currentSelection); break;
            default:
                break;
        }
    }

    private void DefaultPress()
    {
        switch (currentSelection)
        {
            case TIButtonMask.Option1:
                ConfigureMenu(TouchpadState.Numpad); break;
            case TIButtonMask.Option2:
                break;
            case TIButtonMask.Option3:
                break;
            case TIButtonMask.Option4:
                break;
            default:
                break;
        }
    }

    public int numpadValue { get; private set; }
    private void UpdateNumpad(int change, bool reset = false)
    {
        numpadValue += change;
        if (reset)
            numpadValue = change;
        numpadValue = Mathf.Clamp(numpadValue, 0, 100);
        displayText.text = numpadValue + "%";
    }

    public void UpdateText(Response[] responses)
    {
        for (int i = 0; i < amountOfOptions; i++)
        {
            panels[i].transform.GetChild(0).GetComponent<TextMesh>().text = responses[i].ResponseText;
        }
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SetSelectedOption(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SetSelectedOption(true);

        if (Input.GetKeyDown(KeyCode.F2))
            DrawMenu(amountOfOptions++);
        else if (Input.GetKeyDown(KeyCode.F3))
            DrawMenu(amountOfOptions--);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            TouchpadPress();
        else if (Input.GetKeyDown(KeyCode.LeftAlt))
            SwitchToNumpad();
        else if (Input.GetKeyDown(KeyCode.LeftShift))
            ToggleTI();
        else if (Input.GetKeyDown(KeyCode.Escape))
            ConfigureMenu(TouchpadState.Default);
    }*/


    /*private void DrawMenu()
    {
        newAmount = Mathf.Clamp(newAmount, 0, 4);
        isDrawMenuActive = true;
        amountOfOptions = newAmount;

        Vector3 position = obj.transform.position;
        obj.transform.SetParent(null);
        obj.transform.position = Vector3.zero;
        for (int i = 0; i < panels.Count; i++)
            Destroy(panels[i]);

        panels = new List<GameObject>();

        for (int i = 0; i < amountOfOptions; i++)
        {
            float angle = i * Mathf.PI * 2 / amountOfOptions;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * uiRadius;
            if (i >= panels.Count)
            {
                GameObject _obj = Instantiate(uiPrefab, obj.transform.position,obj.transform.rotation);
                panels.Add(_obj);
                _obj.transform.SetParent(obj.transform);
            }
            panels[i].transform.position = obj.transform.position + newPos;
        }
        obj.transform.SetParent(transform);
        obj.transform.position = position;
        isDrawMenuActive = false;
    }*/

    // Sets the selected option based on input
    /*public void SetSelectedOption(bool increase = false, bool reset = false)
    {
        if (!CanRedrawMenu)
            return;
        if (reset == false)
        {
            panels[currentSelection].GetComponent<Renderer>().material.color = Color.white;
            if (increase)
            {
                if (currentSelection == amountOfOptions - 1)
                    currentSelection = 0;
                else currentSelection++;
            }
            else
            {
                if (currentSelection == 0)
                    currentSelection = amountOfOptions - 1;
                else currentSelection--;
            }
        }
        else currentSelection = 0;
        panels[currentSelection].GetComponent<Renderer>().material.color = Color.black;
    }*/
}
