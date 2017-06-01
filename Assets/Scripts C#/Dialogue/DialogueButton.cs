using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    [Range(0, 3)]public int option = 0;
    public TextMesh textMesh;
    public cakeslice.Outline outline;

    public bool selected;
    bool isSwitchOffActive;
    IEnumerator switchOff;

    private void Start()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMesh>();
        outline = GetComponent<cakeslice.Outline>();
        outline.enabled = false;
        switchOff = SwitchOff();
    }

    public void UseButton()
    {
        DialogueController.instance.PressSelectedOption(option);
        ResponseUI.instance.ToggleVisible(false);
    }

    private void OnMouseDown()
    {
        UseButton();
    }

    public void OnPointerOver()
    {
        outline.enabled = true;

        if (isSwitchOffActive)
            StopCoroutine(switchOff);

        switchOff = SwitchOff();
        StartCoroutine(switchOff);
    }

    public void OnPointerExit()
    {
        outline.enabled = false;
    }

    
    IEnumerator SwitchOff()
    {
        isSwitchOffActive = true;
        yield return new WaitForSeconds(.1f);
        OnPointerExit();
        isSwitchOffActive = false;
    }
}
