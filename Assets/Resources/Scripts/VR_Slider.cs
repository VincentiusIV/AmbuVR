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
        if(hitPosition.x < rightBounds.position.x && hitPosition.x > leftBounds.position.x)
        {
            transform.position = new Vector3(hitPosition.x, transform.position.y, transform.position.z);
            // calc value
        }
        
    }
}
