using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    [Range(0, 3)]public int option = 0;
    public TextMesh textMesh;

    private void Start()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMesh>();
    }

    private void OnMouseDown()
    {
        DialogueController.instance.PressSelectedOption(option);
        ResponseUI.instance.ToggleVisible(false);
    }
}
