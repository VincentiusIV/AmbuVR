using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TouchpadState { Default, DialogueSelect, Numpad }
/* TODO
 * - Add touchpad interface states
 * - Be able to interact with environment (dialogue, tbsa, 
 * 
 * */
public class TouchpadInterface : MonoBehaviour {

    public bool isActive { get; private set; }
    private TouchpadState state { get; set; }
    private int currentSelection { get; set; }
    // Serialized
    [SerializeField] private int amountOfOptions = 10;
    [SerializeField] private float uiRadius = 1;
    [SerializeField] private GameObject uiPrefab;
    // Reference
    public GameObject obj;
    private List<GameObject> panels;
    private TextMesh displayText;
    public DialogueController dc;
    private MeshRenderer mr;

    public void Awake()
    {
        //obj = transform.GetChild(0).gameObject;
        //dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        displayText = transform.GetChild(0).GetChild(0).GetComponent<TextMesh>();
        mr = GetComponent<MeshRenderer>();
        panels = new List<GameObject>();

        ConfigureMenu(TouchpadState.Default);
    }

    public void ToggleTI()
    {
        if (obj.activeInHierarchy)
            return;
        obj.SetActive(isActive = !isActive);
    }

    public void ConfigureMenu(TouchpadState newState, int amountOfDia = 0)
    {
        state = newState;
        switch (newState)
        {
            case TouchpadState.Default:
                SwitchToDefault(); break;
            case TouchpadState.DialogueSelect:
                DrawMenu(amountOfDia); break;
            case TouchpadState.Numpad:
                SwitchToNumpad(); break;
            default:
                break;
        }
    }

    private void SwitchToDefault()
    {
        Debug.Log("Switching to default");
        displayText.text = "Menu";
        DrawMenu(2);
        panels[0].transform.GetChild(0).GetComponent<TextMesh>().text = "Say";
        panels[1].transform.GetChild(0).GetComponent<TextMesh>().text = "TBSA";
    }

    private void SwitchToNumpad()
    {
        Debug.Log("Switching to numpad");
        displayText.text = "0%";
        DrawMenu(10);
        for (int i = 0; i < 10; i++)
        {
            panels[i].transform.GetChild(0).GetComponent<TextMesh>().text = i.ToString();
        }
    }

    private bool isDrawMenuActive = false;

    private void DrawMenu(int newAmount)
    {
        isDrawMenuActive = true;
        amountOfOptions = newAmount;

        for (int i = 0; i < panels.Count; i++)
            Destroy(panels[i]);

        panels = new List<GameObject>();

        for (int i = 0; i < amountOfOptions; i++)
        {
            float angle = i * Mathf.PI * 2 / amountOfOptions;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * uiRadius;
            if (i >= panels.Count)
            {
                GameObject _obj = Instantiate(uiPrefab, obj.transform.position,Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                panels.Add(_obj);
                _obj.transform.SetParent(obj.transform);
            }
            panels[i].transform.position = obj.transform.position + newPos;
        }
        isDrawMenuActive = false;
        SetSelectedOption(false, true);
    }

    public bool CanRedrawMenu { get { return !isDrawMenuActive; } }

    // Sets the selected option based on input
    public void SetSelectedOption(bool increase = false, bool reset = false)
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
    }
    // Converts touchpadCoord into selected option
    public void RotateWheelSelector(Vector2 touchpadCoord)
    {
        // Rotation
        float angle = Mathf.Atan2(touchpadCoord.x, touchpadCoord.y);
        float degrees = (180 / Mathf.PI) * angle;

        int selection = Mathf.Clamp((int)((degrees + 180f) / (360 / amountOfOptions)), 0, amountOfOptions + 1);

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Debug.Log(selection);
    }
    /*
    private int Rotation(Vector2 touchpadCoord)
    {
        // Rotation
        float angle = Mathf.Atan2(touchpadCoord.x, touchpadCoord.y);
        float degrees = (180 / Mathf.PI) * angle;

        int selection = Mathf.Clamp((int)((degrees + 180f) / (360 / amountOfOptions)), 0, amountOfOptions + 1);
        
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        Debug.Log(selection);
        //transform.localRotation = Quaternion.Euler(transform.localRotation.x, degrees, transform.localRotation.z);
        return selection;
    }*/

    // Handles touchpad press
    public void TouchpadPress()
    {
        Debug.Log("You pressed down on touchpad");
        switch (state)
        {
            case TouchpadState.Default:
                DefaultPress(); break;
            case TouchpadState.DialogueSelect:
                dc.Interact_Dialogue(currentSelection); break;
            case TouchpadState.Numpad:
                UpdateNumpad(currentSelection); break;
            default:
                break;
        }
        
    }

    private void DefaultPress()
    {
        if(currentSelection == 0)
        {
            dc.Interact_Dialogue(currentSelection);
        }
        else if(currentSelection == 1)
        {
            ConfigureMenu(TouchpadState.Numpad);
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
        Debug.Log("Updating text...");
        DrawMenu(responses.Length);
        for (int i = 0; i < responses.Length; i++)
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
}
