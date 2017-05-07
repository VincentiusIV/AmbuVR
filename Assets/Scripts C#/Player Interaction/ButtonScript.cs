using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    [SerializeField] GameObject highlight;
    [SerializeField] GameObject picture;

    IEnumerator turnOff;
    bool isTurnOffActive;

    [SerializeField] EnviroID environment;
    [SerializeField] Difficulty difficulty;
    [SerializeField] bool finishScene;

    SceneCreator sc;
    GameController gc;

    private void Start()
    {
        sc = GameObject.FindWithTag("VariousController").GetComponent<SceneCreator>();
        gc = GameObject.FindWithTag("VariousController").GetComponent<GameController>();
        highlight.SetActive(false);

        if(environment != EnviroID.None)
        {
            gameObject.name = environment.ToString();

            picture.GetComponent<Renderer>().material.SetTexture(Shader.PropertyToID("_MainTex"), Resources.Load(environment.ToString()) as Texture);
        }
        if(difficulty != Difficulty.None)
        {
            gameObject.name = difficulty.ToString();
        }
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
        // play sound & show smth visual that this option is selected

        // perform action

        if (environment != EnviroID.None)
            sc.SetEnvironment = (int)environment;
        if (difficulty != Difficulty.None)
            sc.SetDifficulty = (int)difficulty;

        if (finishScene)
            gc.FinishGame();
    }
}
