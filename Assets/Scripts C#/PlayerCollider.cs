using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour {

    public Transform hmdPosition;

    private void Update()
    {
        if (hmdPosition != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(hmdPosition.position, Vector3.down, out hit))
            {
                transform.position = hit.point;
            }
            else transform.position = new Vector3(hmdPosition.position.x, hmdPosition.position.y - 1, hmdPosition.position.z);
        }
        else Debug.Log("Please setup hmd position for player collider");
    }
}
