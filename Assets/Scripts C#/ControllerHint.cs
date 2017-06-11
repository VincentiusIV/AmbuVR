using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum ControlPart { Model, Trigger, Grip, Touchpad, ApplicationButton, SystemButton }

[System.Serializable]
public class HintData
{
    public ControlPart part;
    public string hintText;
    public GameObject gameObject;
}

public class ControllerHint : MonoBehaviour
{
    public static ControllerHint instance;

    public GameObject hintWindow;
    public Text hintText;

    public bool showHintsOnLeft;

    [Header("Left Controller")]
    public List<HintData> leftHintList = new List<HintData>();

    [Header("Right Controller")]
    public List<HintData> rightHintList = new List<HintData>();

    //--- Private ---//
    GameObject hintObject;
    LineRenderer line;
    bool isHintActive;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (leftHintList.Count != rightHintList.Count)
            Debug.LogError("You did not configure controller hint correctly! Uneven count!");

        line = GetComponent<LineRenderer>();
        line.enabled = false;

        hintWindow.SetActive(false);
        turnOff = TurnOff();
    }

    private void Update()
    {
        if(isHintActive)
        {
            line.SetPosition(0, hintWindow.transform.position);
            line.SetPosition(1, hintObject.transform.position);
        }

        if (Input.GetKeyDown(KeyCode.M))
            ShowHint(ControlPart.Model);
    }

    public void ShowHint(ControlPart part)
    {
        List<HintData> partList = new List<HintData>();

        if (showHintsOnLeft)
            partList = leftHintList;
        else partList = rightHintList;

        for (int i = 0; i < partList.Count; i++)
        {
            if (partList[i].part == part)
            {
                hintObject = partList[i].gameObject;
                hintText.text = partList[i].hintText;
                break;
            }
            return;
        }

        if (isHintActive)
            StopCoroutine(turnOff);

        turnOff = TurnOff();
        StartCoroutine(turnOff);
    }

    IEnumerator turnOff;

    IEnumerator TurnOff()
    {
        isHintActive = true;
        line.enabled = true;
        hintWindow.SetActive(true);
        yield return new WaitForSeconds(4f);
        hintWindow.SetActive(false);
        line.enabled = false;
        isHintActive = false;
    }
}
