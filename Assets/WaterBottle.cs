using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour {

    [SerializeField] private float tiltCap = 90.0f;

    private Transform tf;

    private void Start()
    {
        tf = GetComponent<Transform>();
    }
    private void Update()
    {
        if(IsBottleOverflowing())
        {
            Debug.Log("Bottle is overflowing");
        }
    }

    private bool IsBottleOverflowing()
    {
        Vector3 angle = tf.rotation.eulerAngles;
        Debug.Log(angle);
        if (angle.x > tiltCap || angle.z > tiltCap)
            return true;
        else return false;
    }
}
