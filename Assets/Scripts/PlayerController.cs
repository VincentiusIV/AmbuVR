using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]private float speed = 10.0f;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        
	}
	
	// Update is called once per frame
	void Update () {

        float vPos = Input.GetAxisRaw("Vertical") * speed;
        float hPos = Input.GetAxisRaw("Horizontal") * speed;

        Vector3 newPos = new Vector3(hPos, 0f, vPos) * Time.deltaTime;
        transform.Translate(newPos);
        
        if (Input.GetButtonDown("Cancel"))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
        }

            
	}
}
