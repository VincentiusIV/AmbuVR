using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public Transform objToFollow;
    public float lerpIntensity = 0.1f;

    private void Update()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, objToFollow.position, lerpIntensity);
        transform.position = newPosition;
        transform.rotation = objToFollow.rotation;
    }
}
