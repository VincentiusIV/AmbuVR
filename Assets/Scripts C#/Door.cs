using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{

    //--- Public ---//
    public AudioClip[] knockSounds;
    public AudioClip openSound;
    public AudioClip closeSound;

    public Animator anime;

    public UnityEvent OnKnock;

    //--- Private ---//
    AudioSource sound;

    bool open;

    private void Start()
    {
        sound = GetComponent<AudioSource>();

        if (anime == null)
            anime = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            OpenDoor();
        if (Input.GetKeyDown(KeyCode.P))
            CloseDoor();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("AI"))
        {
            OpenDoor();
        }
    }

    public void Knock()
    {
        // Play sound
        Debug.Log("You knocked on the door");

        PlayClip(knockSounds[Random.Range(0, knockSounds.Length -1)]);
        OnKnock.Invoke();
    }

    public void OpenDoor()
    {
        anime.SetBool("Open", open = true);
        PlayClip(openSound);
    }

    public void CloseDoor()
    {
        anime.SetBool("Open", open = false);
        PlayClip(closeSound);
    }

    void PlayClip(AudioClip clip)
    {
        sound.clip = clip;
        sound.Play();
    }

    public void SwitchDoor()
    {
        if (open)
            CloseDoor();
        else if (!open)
            OpenDoor();
    }
}
