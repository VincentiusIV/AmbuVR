using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TouchpadState { Default = 0, DialogueSelect = 8, Numpad = 4, Teleporting = 12}

[System.Serializable]
public enum TIButtonMask { Option1 = 0, Option2 = 1, Option3 = 2, Option4 = 3 }

[System.Serializable]
public enum TIButtonFunction {  Say = 0, TBSA = 1, Teleport = 2, Placeholder = 3,
                                Plus = 4, Minus = 5, Enter = 6, Back = 7,
                                Hi = 8, Bye = 9, Greetings = 10, Hello = 11,
                             }

public class TouchpadInterface : MonoBehaviour {

    public bool isActive { get; private set; }
    private TouchpadState state { get; set; }
    private TIButtonMask currentSelection { get; set; }
    // Serialized
    [SerializeField] private int amountOfOptions = 4;
    //[SerializeField] private float uiRadius = 1;
    [SerializeField] private GameObject uiPrefab;

    public Color defaultColor;
    public Color selectedColor;
    // Reference
    public GameObject mainPanel;
    public List<GameObject> panels;
    private TextMesh displayText;
    public DialogueController dc;
    //private MeshRenderer mr;
    //private GameController gc;

    public TBSA_Controller tbsa;

    public void Awake()
    {
        //obj = transform.GetChild(0).gameObject;
        //dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        displayText = transform.GetChild(0).GetComponent<TextMesh>();
        //mr = GetComponent<MeshRenderer>();
        //gc = GameObject.FindWithTag("VariousController").GetComponent<GameController>();
        currentSelection = TIButtonMask.Option1;
        //selectionIndex = 1;
        ConfigureMenu(TouchpadState.Default);

        foreach (GameObject item in panels)
        {
            item.GetComponent<Renderer>().material.color = defaultColor;
        }
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

        if (newState != TouchpadState.DialogueSelect )
        {
            int start = (int)newState;

            for (int i = start; i < start + 4; i++)
            {
                panels[i - start].transform.GetChild(0).GetComponent<TextMesh>().text = ((TIButtonFunction)i).ToString();
            }
        }
    }

    public void SetSelectedOption(Vector2 touchpadCoord )
    {
        panels[(int)currentSelection].GetComponent<Renderer>().material.color = defaultColor;
        if (touchpadCoord.x < 0 && touchpadCoord.y > 0)
            currentSelection = TIButtonMask.Option1;
        else if (touchpadCoord.x > 0 && touchpadCoord.y > 0)
            currentSelection = TIButtonMask.Option2;
        else if (touchpadCoord.x < 0 && touchpadCoord.y < 0)
            currentSelection = TIButtonMask.Option3;
        else if (touchpadCoord.x > 0 && touchpadCoord.y < 0)
            currentSelection = TIButtonMask.Option4;
        panels[(int)currentSelection].GetComponent<Renderer>().material.color = selectedColor;
    }

    // Handles touchpad press
    public void TouchpadPress()
    {
        Debug.Log(string.Format("{0} curselect & selectIndex: {1}", (int)currentSelection, selectionIndex));
        switch (state)
        {
            case TouchpadState.Default:
                DefaultPress(); break;
            case TouchpadState.DialogueSelect:
                if ((int)currentSelection + 1 <= selectionIndex)
                    dc.Interact_Dialogue((int)currentSelection); break;
            case TouchpadState.Numpad:
                NumpadPress(); break;
            case TouchpadState.Teleporting:
                break;
            default:
                break;
        }
    }

    private void DefaultPress()
    {
        switch (currentSelection)
        {
            case TIButtonMask.Option1:
                ConfigureMenu(TouchpadState.DialogueSelect); break;
            case TIButtonMask.Option2:
                ConfigureMenu(TouchpadState.Numpad); break;
            case TIButtonMask.Option3:
                break;
            case TIButtonMask.Option4:
                ConfigureMenu(TouchpadState.Default); break;
            default:
                break;
        }
    }

    private void NumpadPress()
    {
        TIButtonFunction keyNumber = (TIButtonFunction)((int)currentSelection + (int)state);

        switch (keyNumber)
        {
            case TIButtonFunction.Plus:
                UpdateNumpad(1); break;
            case TIButtonFunction.Minus:
                UpdateNumpad(-1); break;
            case TIButtonFunction.Enter:
                tbsa.FinishAttempt(); break;
            case TIButtonFunction.Back:
                ConfigureMenu(TouchpadState.Default); break;
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

        tbsa.UpdateInputField(change);
    }

    private int selectionIndex;
    public void UpdateText(Response[] responses)
    {
        selectionIndex = responses.Length;
        for (int i = 0; i < amountOfOptions; i++)
        {
            string newText;
            if (i < responses.Length)
                newText = responses[i].ResponseText;
            else newText = "";

            panels[i].transform.GetChild(0).GetComponent<TextMesh>().text = newText;
        }
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentSelection = TIButtonMask.Option1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentSelection = TIButtonMask.Option2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentSelection = TIButtonMask.Option3;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            currentSelection = TIButtonMask.Option4;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
            TouchpadPress();
    }*/

    // Depracated way of drawing the menu
    // Places options in a circle, can add nearly unlimited amount of buttons to the circle
    // Actually worked but unnecessary in the end
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
