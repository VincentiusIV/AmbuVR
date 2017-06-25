using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shower : MonoBehaviour {

    public ParticleSystem showerPS;
    public Transform showerPosition;
    public Transform resetPoint;

    public AudioSource showerOpen;

    public float minShowerTime = 10f;

    float showerTimer = 0f;
    bool isPatientInShower;
    bool didShower = false;

    private void Start()
    {
        showerPS = GetComponentInChildren<ParticleSystem>();
        showerOpen = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("AI") && didShower == false)
        {
            NPCManager.instance.npcs[1].transform.position = showerPosition.position;
            //other.GetComponent<CapsuleCollider>().enabled = false;
            //other.GetComponent<NavMeshAgent>().enabled = false;
            NPCManager.instance.npcs[1].StopMoving();
            NPCManager.instance.npcs[1].ChangeBehaviour(AIBehaviourState.Idle, Vector3.zero, true);
        }
    }

    public void UseShower()
    {
        showerPS.Play();
        showerOpen.Play();
        isPatientInShower = true;
    }

    public void CloseShower()
    {
        showerPS.Stop();
        showerOpen.Stop();
        isPatientInShower = false;
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
        if (showerTimer > minShowerTime && didShower == false)
        {
            didShower = true;
            isPatientInShower = false;
            Patient.instance.FinishCooling(MedicalItem.Water);
            NPCManager.instance.npcs[1].transform.position = resetPoint.position;
            NPCManager.instance.npcs[1].ChangeBehaviour(AIBehaviourState.Follow, Vector3.zero, true);
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
