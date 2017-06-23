using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour {

    public VideoPlayer player;

    private void Start()
    {
        player.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided tv");
        if(other.GetComponent<TVClip>())
        {
            Debug.Log("activating tv");
            player.gameObject.SetActive(true);
            // get clip
            player.clip = other.GetComponent<TVClip>().clip;
            // play clip
            player.Play();

            if(!isTurningOff)
                StartCoroutine(TurnOff());
        }
    }

    bool isTurningOff;
    IEnumerator TurnOff()
    {
        isTurningOff = true;
        yield return new WaitUntil(() => player.isPlaying == false);
        player.gameObject.SetActive(false);
        isTurningOff = false;
    }
    
}
