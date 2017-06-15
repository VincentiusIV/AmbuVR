using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class CustomEventTrigger : MonoBehaviour {

    public UnityEvent OnCollisionEnterEvent;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("VR_Controller"))
            return;
        Debug.Log(string.Format("You hit {0} with your controller", gameObject.name));
        OnCollisionEnterEvent.Invoke();
    }
}
