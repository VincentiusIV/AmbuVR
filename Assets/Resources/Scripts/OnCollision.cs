 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Play sound on collision
[RequireComponent(typeof(AudioSource))]
public class OnCollision : MonoBehaviour
{
    // Reference
    private AudioSource asFx;

    // Serialize fields
    [SerializeField] private AudioClip FallOnFloorSound;

    private void Start()
    {
        asFx = GetComponent<AudioSource>();

        FallOnFloorSound = Resources.Load("Sounds/FallOnFloor") as AudioClip;
        // get audio clips from resources
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(ShouldPlayClip(FallOnFloorSound))
        {
            if (asFx.isPlaying)
                asFx.Stop();
                
            asFx.clip = FallOnFloorSound;
            asFx.loop = false;
            asFx.Play();
        }
           
    }

    private bool ShouldPlayClip(AudioClip clip)
    {
        if (clip != null)
            return true;
        else return false;
    }

}
