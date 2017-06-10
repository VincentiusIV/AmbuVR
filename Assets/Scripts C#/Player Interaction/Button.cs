using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace AmbuVR
{
    [RequireComponent(typeof(AudioSource))]
    public class Button : MonoBehaviour
    {
        //--- Public ---//
        [Header("References")]
        public cakeslice.Outline outline;
        public Text textMesh;
        public AudioClip selectSound;
        public AudioClip clickSound;

        [Header("Events")]
        public UnityEvent OnPointerOver;
        public UnityEvent OnUseButton;

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

            OnPointerOver.AddListener(PointerOver);
            OnUseButton.AddListener(UseButton);
        }

        public virtual void UseButton()
        {
            Debug.Log("You pressed button " + gameObject.name);

            if(clickSound != null)
            {
                sound.Stop();
                sound.clip = clickSound;
                sound.Play();     
            }
        }

        private void OnMouseOver()
        {
            Debug.Log("Over: " + gameObject.name);
            PointerOver();

            if (Input.GetButtonDown("Fire1"))
                UseButton();
        }

        public void PointerOver()
        {
            if(selected == false)
            {
                selected = true;
                sound.clip = selectSound;
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

        public void PointerExit()
        {
            if(outline != null)
                outline.enabled = false;
            selected = false;
        }


        IEnumerator SwitchOff()
        {
            isSwitchOffActive = true;
            yield return new WaitForSeconds(.1f);
            PointerExit();
            isSwitchOffActive = false;
        }
    }
}


