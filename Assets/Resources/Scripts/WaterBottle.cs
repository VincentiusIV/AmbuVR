using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour {

    [SerializeField] private float tiltCap = 90.0f;

    private Transform tf;
    private ParticleLauncher pl;
    private ViveController controller;

    private void Start()
    {
        tf = GetComponent<Transform>();
        pl = transform.GetChild(0).GetChild(0).GetComponent<ParticleLauncher>();
    }

    private void Update()
    {
        if(controller != null && controller.IsGripPressed(gameObject))
        {
            pl.LaunchParticle();
        }
    }

    public void SetController(ViveController _controller)
    {
        controller = _controller;
    }

    /*private bool IsBottleOverflowing()
    {
        Vector3 angle = tf.rotation.eulerAngles;
        Debug.Log(angle);
        if (angle.x > tiltCap || angle.z > tiltCap)
            return true;
        else return false;
    }*/
}
