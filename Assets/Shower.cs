using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shower : MonoBehaviour {

    public ParticleSystem showerPS;
    public Transform showerPosition;
    public Transform resetPoint;

    float showerTimer = 0f;
    bool isPatientInShower;

    private void Start()
    {
        showerPS = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("AI"))
        {
            showerPS.Play();
            isPatientInShower = true;
            other.transform.position = showerPosition.position;
            //other.GetComponent<CapsuleCollider>().enabled = false;
            //other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<NavMeshAgent>().isStopped = true;
            NPCManager.instance.npcs[1].ChangeBehaviour(AIState.Idle, Vector3.zero, true);
        }
    }

    private void Update()
    {
        if(isPatientInShower)
        {
            showerTimer += Time.deltaTime;

            if(showerTimer > 10f)
            {
                Patient.instance.FinishCooling(MedicalItem.Water);
                Patient.instance.transform.position = resetPoint.position;
                NPCManager.instance.npcs[1].GetComponent<CapsuleCollider>().enabled = true;
                NPCManager.instance.npcs[1].GetComponent<NavMeshAgent>().enabled = true;
                this.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("AI"))
        {
            showerPS.Stop();
            isPatientInShower = false;
            Debug.Log("Patient stood under shower for: " + Mathf.RoundToInt(showerTimer) + " seconds");
            
        }
    }
}
