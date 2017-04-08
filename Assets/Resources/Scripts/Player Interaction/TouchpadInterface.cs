using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TouchpadOptions { Option0 = 0, Option1 = 1, Option2 = 2, Option3 = 3, Option4 = 4, Option5 = 5, Option6 = 6, Option7 = 7, Option8 = 8, Option9 = 9}

[System.Serializable]
public enum TouchpadState { Default, DialogueSelect, Numpad }
/* TODO
 * - Add touchpad interface states
 * - Be able to interact with environment (dialogue, tbsa, 
 * 
 * */
public class TouchpadInterface : MonoBehaviour {

    private TouchpadOptions to { get; set; }
    private TouchpadState state { get; set; }
    [SerializeField]private int amountOfOptions = 10;

    private GameObject[] panels;
    private Color[] colors;
    [SerializeField] private TextMesh[] texts;
    // Reference
    private DialogueController dc;

    public void Start()
    {
        dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        to = TouchpadOptions.Option0;
        panels = new GameObject[amountOfOptions];
        colors = new Color[amountOfOptions];
        Debug.Log(360 / amountOfOptions);

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(Random.Range(0f, 255f), Random.Range(0f, 255f), Random.Range(0f, 255f));
        }
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = transform.GetChild(i).gameObject;
            panels[i].GetComponent<Renderer>().material.color = colors[i];
        }
    }

    private void Update()
    {
        // Testing Input //
        if (Input.GetKeyDown(KeyCode.Alpha1))
            dc.PressSelectedOption(TouchpadOptions.Option1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            dc.PressSelectedOption(TouchpadOptions.Option2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            dc.PressSelectedOption(TouchpadOptions.Option3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            dc.PressSelectedOption(TouchpadOptions.Option4);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            dc.Interact_Dialogue();
        }
    }

    public int SetSelectedOption(Vector2 touchpadCoord)
    {
        
        // Add animation blend tree
        panels[(int)to].GetComponent<Renderer>().material.color = colors[(int)to];

        to = (TouchpadOptions)Rotation(touchpadCoord);

        panels[(int)to].GetComponent<Renderer>().material.color = Color.white;
        Debug.Log(to);
        return (int)to;
    }

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
    }


    public void TouchpadPress()
    {
        Debug.Log("You pressed down on touchpad");

        dc.PressSelectedOption(to);
        // Play fade out animation or smth 
    }

    public void UpdateText(Response[] responses)
    {
        for (int i = 0; i < responses.Length; i++)
        {
            texts[i].text = responses[i].ResponseText;
            amountOfOptions = responses.Length;
        }
    }

}
