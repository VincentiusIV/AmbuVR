using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBSA_Controller : MonoBehaviour {

    public GameObject currentPatient;
    public Transform spawnPos;

    public TextMesh attemptDisplay;
    public TextMesh correctDisplay;

    public int exampleTBSA;

    //private SkinTexture skinTexture;

    private int inputField;

    private void Start()
    {
        //skinTexture = currentPatient.GetComponent<SkinTexture>();
    }

    public void UpdateInputField(int change)
    {
        inputField += change;
        inputField = Mathf.Clamp(inputField, 0, 100);
        attemptDisplay.text = "Attempt: " + inputField+"%";

    }

    public void FinishAttempt()
    {
        int correctTBSA = exampleTBSA;//(int)skinTexture.GetTBSA();
        correctDisplay.text = "TBSA = " + correctTBSA;

        string resultText;
        if (inputField == correctTBSA)
            resultText = "You are correct";
        else resultText = "You fail";
        attemptDisplay.text = resultText;
    }
    /* Keyboard testing
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            UpdateInputField(1);
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            UpdateInputField(-1);
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
            FinishAttempt();
    }*/
}

