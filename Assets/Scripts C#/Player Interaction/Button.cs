using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmbuVR
{
    [RequireComponent(typeof(cakeslice.Outline))]
    [RequireComponent(typeof(AudioSource))]
    public class Button : MonoBehaviour
    {
        //--- Public ---//
        public cakeslice.Outline outline;
        public TextMesh textMesh;

        //--- Private ---//
        IEnumerator switchOff;
        AudioSource sound;

        //--- Booleans ---//
        bool selected;
        bool isSwitchOffActive;
        

        private void Start()
        {
            textMesh = transform.GetChild(0).GetComponent<TextMesh>();
            outline = GetComponent<cakeslice.Outline>();
            outline.enabled = false;
            switchOff = SwitchOff();

            sound = GetComponent<AudioSource>();
        }

        public virtual void UseButton()
        {
            Debug.Log("You pressed button " + gameObject.name);
        }

        private void OnMouseDown()
        {
            UseButton();
        }

        public void OnPointerOver()
        {
            if(selected == false)
            {
                selected = true;
                sound.Play();
            }

            if (isSwitchOffActive)
                StopCoroutine(switchOff);
            else
                outline.enabled = true;

            switchOff = SwitchOff();
            StartCoroutine(switchOff);
        }

        public void OnPointerExit()
        {
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


