using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shower : MonoBehaviour {

    public ParticleSystem showerPS;
    public Transform showerPosition;
    public Transform resetPoint;

    public AudioSource showerOpen;


    float showerTimer = 0f;
    bool isPatientInShower;
    bool didShower = false;

    private void Start()
    {
        showerPS = GetComponentInChildren<ParticleSystem>();
        showerOpen = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("AI") && didShower == false)
        {
            showerPS.Play();
            isPatientInShower = true;
            NPCManager.instance.npcs[1].transform.position = showerPosition.position;
            //other.GetComponent<CapsuleCollider>().enabled = false;
            //other.GetComponent<NavMeshAgent>().enabled = false;
            NPCManager.instance.npcs[1].StopMoving();
            NPCManager.instance.npcs[1].ChangeBehaviour(AIState.Idle, Vector3.zero, true);
            
            showerOpen.Play();
        }
    }

    private void Update()
    {
        if(isPatientInShower)
        {
            showerTimer += Time.deltaTime;

            
        }
    }

    private void FixedUpdate()
    {
        if (showerTimer > 10f && didShower == false)
        {
            didShower = true;
            isPatientInShower = false;
            Patient.instance.FinishCooling(MedicalItem.Water);
            NPCManager.instance.npcs[1].transform.position = resetPoint.position;
            NPCManager.instance.npcs[1].ChangeBehaviour(AIState.Follow, Vector3.zero, true);
            showerOpen.Stop();
            GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("AI") && didShower == true)
        {
            
            Debug.Log("Patient stood under shower for: " + Mathf.RoundToInt(showerTimer) + " seconds");
            
        }
    }
}
