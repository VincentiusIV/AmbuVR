using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour {

    [SerializeField]private float speed = 10.0f;

    LineRenderer pointer;

    // Private & Serialized fields
    [SerializeField] Transform pointerOrigin;
    [SerializeField] float pointerLength;

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        pointer = GetComponent<LineRenderer>();
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

        DrawPointer();
	}

    // Pointer
    private void DrawPointer()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(pointerOrigin.position, pointerOrigin.forward, pointerLength);
        //pointer.SetPosition(0, pointerOrigin.position);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.layer == 10 && Input.GetButtonDown("Fire2"))
                        transform.position = hit.point;

                else if (hit.collider.CompareTag("Button"))
                {
                    hit.collider.GetComponent<ButtonScript>().Highlight();

                    if (Input.GetButtonDown("Fire1"))
                        hit.collider.GetComponent<ButtonScript>().Click();

                    return;
                }

                else if (hit.collider.CompareTag("Burn"))
                    return;

                else if (hit.collider.CompareTag("Patient"))
                {
                    hit.transform.GetComponent<SkinTexture>().Highlight(hit.textureCoord);

                    if (Input.GetButtonDown("Fire1"))
                        hit.transform.GetComponent<SkinTexture>().SetPixels(hit.textureCoord, true, hit.point);
                }
            }
        }
        else
        {
            //pointer.SetPosition(1, pointerOrigin.position + (pointerOrigin.forward * pointerLength));
        }
    }
}
