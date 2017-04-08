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

    //private TouchpadOptions to { get; set; }
    private TouchpadState state { get; set; }
    [SerializeField]private int amountOfOptions = 10;
    [SerializeField] private float uiRadius = 1;
    [SerializeField] private GameObject uiPrefab;
    private List<GameObject> panels;
    private TextMesh displayText;
    // Reference
    private DialogueController dc;
    private int currentSelection { get; set; }

    public void Start()
    {
        dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        displayText = transform.GetChild(0).GetComponent<TextMesh>();
        panels = new List<GameObject>();
        state = TouchpadState.DialogueSelect;
        DrawMenu(amountOfOptions);
    }

    private bool isDrawMenuActive = false;

    private void DrawMenu(int newAmount)
    {
        isDrawMenuActive = true;
        amountOfOptions = newAmount;

        for (int i = 0; i < amountOfOptions; i++)
        {
            float angle = i * Mathf.PI * 2 / amountOfOptions;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * uiRadius;
            if (i >= panels.Count)
            {
                GameObject obj = Instantiate(uiPrefab, transform.position,Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                panels.Add(obj);
                obj.transform.SetParent(transform);
            }
            else if(i == amountOfOptions - 1 && panels.Count != amountOfOptions)
            {
                for (int a = amountOfOptions; a < panels.Count; a++)
                {
                    Destroy(panels[a]);
                    panels.Remove(panels[a]);
                    if (currentSelection == a)
                        currentSelection = Mathf.Clamp(currentSelection - 1, 0, amountOfOptions);
                }
            }
            panels[i].transform.position = transform.position + newPos;
        }
        isDrawMenuActive = false;
    }

    public bool CanRedrawMenu { get { return !isDrawMenuActive; } }

    public void SetSelectedOption(bool increase)
    {
        if (!CanRedrawMenu)
            return;

        panels[currentSelection].GetComponent<Renderer>().material.color = Color.white;
        if(increase)
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
        panels[currentSelection].GetComponent<Renderer>().material.color = Color.black;
    }

    public void RotateWheelSelector(Vector2 touchpadCoord)
    {

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

    public void TouchpadPress()
    {
        Debug.Log("You pressed down on touchpad");
        switch (state)
        {
            case TouchpadState.Default:
                break;
            case TouchpadState.DialogueSelect:
                dc.Interact_Dialogue(currentSelection); break;
            case TouchpadState.Numpad:
                UpdateNumpad(currentSelection); break;
            default:
                break;
        }
        
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
    }

    private void SwitchToNumpad()
    {
        if (state == TouchpadState.Numpad)
            return;
        state = TouchpadState.Numpad;
        displayText.text = "0%";
        DrawMenu(10);
        for (int i = 0; i < 10; i++)
        {
            panels[i].transform.GetChild(0).GetComponent<TextMesh>().text = i.ToString();
        }
    }
    int numpadValue;

    private void UpdateNumpad(int change)
    {
        numpadValue += change;
        numpadValue = Mathf.Clamp(numpadValue, 0, 100);
        displayText.text = numpadValue + "%";
    }
}
