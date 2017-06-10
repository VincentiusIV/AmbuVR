﻿using System.Collections;
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
        public GlowObjectCmd outline;
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
            if (outline != null)
                outline.Hide();

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

            if(clickSound != null)
            {
                sound.Stop();
                sound.clip = clickSound;
                sound.Play();     
            }

            OnUseButton.Invoke();
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
                outline.Show();
            }

            OnPointerOver.Invoke();
        }

        public void PointerExit()
        {
            if (outline != null)
            {
                outline.Hide();
            }
            selected = false;
        }


        private void OnMouseOver()
        {
            Debug.Log("Over: " + gameObject.name);
            PointerOver();

            if (Input.GetButtonDown("Fire1"))
                UseButton();
        }

        private void OnMouseExit()
        {
            PointerExit();
        }
    }
}


