using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    [SerializeField]
    GameObject highlight;

    IEnumerator turnOff;
    bool isTurnOffActive;
    float timer;

    private void Start()
    {
        highlight.SetActive(false);
    }

    // Called when pointer hits the collider of this button
    public void Highlight()
    {
        if(!highlight.activeInHierarchy)
            highlight.SetActive(true);

        if(isTurnOffActive)
            StopCoroutine(turnOff);

        turnOff = TurnOffHighlight();
        StartCoroutine(turnOff);
    }

    IEnumerator TurnOffHighlight()
    {
        isTurnOffActive = true;
        yield return new WaitForSeconds(.1f);
        highlight.SetActive(false);
        isTurnOffActive = false;
    }

    // Called when a button is highlighted and trigger is pressed
    public void Click()
    {
        Debug.Log("you clicked");
        // play sound
        // perform action
    }
}
