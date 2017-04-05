using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TouchpadOptions { None = 0, Option1 = 1, Option2 = 2, Option3 = 3, Option4 = 4}

public class TouchpadInterface : MonoBehaviour {

    TouchpadOptions to { get; set; }

    private GameObject[] panels;
    [SerializeField]private Color[] colors;

    // Reference
    DialogueController dc;

    public void Start()
    {
        dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        to = TouchpadOptions.None;
        panels = new GameObject[4];

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = transform.GetChild(i).gameObject;
            panels[i].GetComponent<Renderer>().material.color = colors[i];
        }
    }

    public int SetSelectedOption(Vector2 touchpadCoord)
    {
        // Add animation blend tree
        panels[(int)to - 1].GetComponent<Renderer>().material.color = colors[(int)to - 1];

        if (touchpadCoord.x < 0 && touchpadCoord.y > 0)
            to = TouchpadOptions.Option1;
        else if (touchpadCoord.x > 0 && touchpadCoord.y > 0)
            to = TouchpadOptions.Option2;
        else if (touchpadCoord.x < 0 && touchpadCoord.y < 0)
            to = TouchpadOptions.Option3;
        else if (touchpadCoord.x > 0 && touchpadCoord.y < 0)
            to = TouchpadOptions.Option4;

        panels[(int)to - 1].GetComponent<Renderer>().material.color = Color.white;

        return (int)to;
    }

    public void TouchpadPress()
    {
        if(to != TouchpadOptions.None)
        {
            dc.PressSelectedOption(to);
            return;
        }
        // Play fade out animation or smth 
    }
    // TESTING TOOLS ///////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            dc.PressSelectedOption(TouchpadOptions.Option1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            dc.PressSelectedOption(TouchpadOptions.Option2);
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            dc.PressSelectedOption(TouchpadOptions.Option3);
        else if(Input.GetKeyDown(KeyCode.Alpha4))
            dc.PressSelectedOption(TouchpadOptions.Option4);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            dc.StartDialogue();
        }
    }
}
