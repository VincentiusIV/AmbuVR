using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum ControlPart { None = -1, Model =0, Trigger = 1, Grip = 2, Touchpad = 3, ApplicationButton = 4, SystemButton = 5}

[System.Serializable]
public struct HintData
{
    public  ControlPart part;
    public  string hintText;
    public  GameObject gameObject;

    public HintData(ControlPart part, string hintText, GameObject gameObject)
    {
        this.part = part;
        this.hintText = hintText;
        this.gameObject = gameObject;
    }
}

public class ControllerHint : MonoBehaviour
{
    public static ControllerHint instance;

    public GameObject hintWindow;
    public Text hintText;

    public Material selectMaterial;
    public bool showHintsOnLeft;

    public List<string> modelNames = new List<string>();

    [Header("Left Controller")]
    public GameObject leftController;
    public List<HintData> leftHintList = new List<HintData>();

    [Header("Right Controller")]
    public GameObject rightController;
    public List<HintData> rightHintList = new List<HintData>();

    //--- Private ---//
    GameObject hintObject;
    LineRenderer line;
    bool isHintActive;

    Material defaultMaterial;

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

        for (int i = 0; i < 6; i++)
        {
            leftHintList.Add(new HintData((ControlPart)i, ((ControlPart)i).ToString(), leftController.transform.Find(modelNames[i]).GetChild(0).gameObject));
            rightHintList.Add(new HintData((ControlPart)i, ((ControlPart)i).ToString(), rightController.transform.Find(modelNames[i]).GetChild(0).gameObject));
        }
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
        if (Input.GetKeyDown(KeyCode.N))
            ShowHint(ControlPart.Trigger);
        if (Input.GetKeyDown(KeyCode.B))
            ShowHint(ControlPart.Grip);
    }

    public void ShowHint(ControlPart part)
    {
        if (isHintActive || part == ControlPart.None)
            return;

        List<HintData> partList = new List<HintData>();

        Debug.Log(string.Format("right {0}. left {1}", rightHintList.Count, leftHintList.Count));

        if (showHintsOnLeft)
            partList = leftHintList;
        else partList = rightHintList;

        Debug.Log(partList.Count);

        if (partList.Count == 0)
            return;

        for (int i = 0; i < partList.Count; i++)
        {
            if (partList[i].part == part)
            {
                hintObject = partList[i].gameObject;
                hintText.text = partList[i].hintText;
                Debug.Log("Hinting to " + hintObject.name);
                break;
            }
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
