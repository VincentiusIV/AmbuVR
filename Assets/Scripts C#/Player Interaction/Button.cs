using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AmbuVR
{
    [RequireComponent(typeof(AudioSource))]
    public class Button : MonoBehaviour
    {
        //--- Public ---//
        public cakeslice.Outline outline;
        public Text textMesh;

        //--- Private ---//
        IEnumerator switchOff;
        AudioSource sound;

        //--- Booleans ---//
        bool selected;
        bool isSwitchOffActive;
        

        private void Awake()
        {
            if(outline != null)
                outline.enabled = false;
            switchOff = SwitchOff();

            sound = GetComponent<AudioSource>();

            if(GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;
            }
        }

        public virtual void UseButton()
        {
            Debug.Log("You pressed button " + gameObject.name);
        }

        private void OnMouseOver()
        {
            Debug.Log("Over: " + gameObject.name);
            OnPointerOver();

            if (Input.GetButtonDown("Fire1"))
                UseButton();
        }
        public void OnPointerOver()
        {
            if(selected == false)
            {
                selected = true;
                sound.Play();
            }

            if(outline != null)
            {
                if (isSwitchOffActive)
                    StopCoroutine(switchOff);
                else
                    outline.enabled = true;

                switchOff = SwitchOff();
                StartCoroutine(switchOff);
            }
        }

        public void OnPointerExit()
        {
            if(outline != null)
                outline.enabled = false;
            selected = false;
        }


        IEnumerator SwitchOff()
        {
            isSwitchOffActive = true;
            yield return new WaitForSeconds(.1f);
            OnPointerExit();
            isSwitchOffActive = false;
        }
    }
}


