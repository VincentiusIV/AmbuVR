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
}
