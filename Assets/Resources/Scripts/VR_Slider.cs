using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Slider : MonoBehaviour
{
    [SerializeField] Transform leftBounds;
    [SerializeField] Transform rightBounds;

    public int GetValue { get; private set; }

    public void SetPosition(Vector3 hitPosition)
    {
        if(hitPosition.z < rightBounds.position.z && hitPosition.z > leftBounds.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, hitPosition.z);
            // calc value
        }
        
    }
}
