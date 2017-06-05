using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmbuVR
{
    [RequireComponent(typeof(cakeslice.Outline))]
    public class Button : MonoBehaviour
    {
        public cakeslice.Outline outline;
        public TextMesh textMesh;

        public bool selected;
        bool isSwitchOffActive;
        IEnumerator switchOff;

        private void Start()
        {
            textMesh = transform.GetChild(0).GetComponent<TextMesh>();
            outline = GetComponent<cakeslice.Outline>();
            outline.enabled = false;
            switchOff = SwitchOff();
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
            outline.enabled = true;

            if (isSwitchOffActive)
                StopCoroutine(switchOff);

            switchOff = SwitchOff();
            StartCoroutine(switchOff);
        }

        public void OnPointerExit()
        {
            outline.enabled = false;
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


