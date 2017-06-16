using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{

    //--- Public ---//
    public AudioClip knockSound;
    public AudioClip openSound;

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

        sound.clip = knockSound;
        sound.Play();
        OnKnock.Invoke();
    }

    public void OpenDoor()
    {
        anime.SetBool("Open", open = true);
    }

    public void CloseDoor()
    {
        anime.SetBool("Open", open = false);
    }

    public void SwitchDoor()
    {
        if (open)
            CloseDoor();
        else if (!open)
            OpenDoor();
    }
}
