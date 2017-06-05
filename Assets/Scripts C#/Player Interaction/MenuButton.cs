using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : AmbuVR.Button
{
    [Header("Options")]
    public bool playGame;
    public string gSceneName = "GameFlowTesting";

    public bool playTutorial;
    public string tSceneName = "Tutorial";

    public override void UseButton()
    {
        base.UseButton();

        if (playGame)
            SceneManager.LoadScene(gSceneName);

        if (playTutorial)
            TutorialManager.instance.StartTutorial();

        UIController.instance.ToggleUI(false);
    }
}
