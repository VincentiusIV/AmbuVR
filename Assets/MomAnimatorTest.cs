using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomAnimatorTest : MonoBehaviour {

    Animator anime;

    public AIEmotionalState emotionalState = AIEmotionalState.Normal;
    public float transitionLerp = .05f;

    bool isWalking;
    bool isTalking;
    float anger = 0;

	// Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAnimatorBools(true, false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAnimatorBools(false, true);
        }
            


        anger = Mathf.Lerp(anger,(int)emotionalState * 0.5f, transitionLerp);
        anime.SetFloat("Anger", anger);
	}

    private void SetAnimatorBools(bool _isWalking, bool _isTalking)
    {
        anime.SetBool("isWalking", isWalking = _isWalking);
        anime.SetBool("isTalking", isTalking = _isTalking);
    }
}
