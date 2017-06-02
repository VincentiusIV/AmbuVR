using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueButton : AmbuVR.Button
{
    [Range(0, 3)]public int option = 0;

    public override void UseButton()
    {
        base.UseButton();

        DialogueController.instance.PressSelectedOption(option);
        ResponseUI.instance.ToggleVisible(false);
    }
}
