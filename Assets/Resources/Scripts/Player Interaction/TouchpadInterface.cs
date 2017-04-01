using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TouchpadOptions { None = 0, Option1 = 1, Option2 = 2, Option3 = 3, Option4 = 4}

public class TouchpadInterface : MonoBehaviour {

    private GameObject[] panels;
    [SerializeField]private Color[] colors;

    public void Start()
    {
        panels = new GameObject[4];

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = transform.GetChild(i).gameObject;
            panels[i].GetComponent<Renderer>().material.color = colors[i];
        }
    }
    public int GetSelectedOption(Vector2 touchpadCoord)
    {
        // Add animation blend tree
        TouchpadOptions to = TouchpadOptions.None;

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


}
