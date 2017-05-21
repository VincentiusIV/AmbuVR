using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class AI_Movement : MonoBehaviour
{
    [Header("State")]
    public int id;
    public int stressLevel;
    public AIState state;

    [Header("Patrolling")]
    public Transform[] patrolSpots;
    public float waitTimeAtSpot;

    [Header("Command Behaviour")]
    public cakeslice.Outline outline;

    NavMeshAgent agent;
    //public DialogueController dc;
    AudioSource voice;
    Animator anime;

    IEnumerator patrol;
    bool isPatrolRunning;
    bool isSelectedToMove = false;
    bool selected = false;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        //dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        voice = GetComponent<AudioSource>();
        anime = GetComponent<Animator>();
        patrol = Patrol();

        if(patrolSpots.Length > 0)
            StartCoroutine(patrol);

        outline.enabled = false;
    }
    private void Update()
    {
        if(isSelectedToMove)
        {
            if(Input.GetButtonDown("Fire1") && selected)
            {
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    agent.SetDestination(hit.point);
                    isSelectedToMove = false;
                    selected = false;
                }
            }
            else if (Input.GetButtonUp("Fire1") && selected != true)
                selected = true;
        }
    }
    private IEnumerator Patrol()
    {
        isPatrolRunning = true;
        while(agent.isOnNavMesh)
        {
            for (int i = 0; i < patrolSpots.Length; i++)
            {
                agent.SetDestination(patrolSpots[i].position);
                yield return new WaitUntil(() => transform.position == agent.destination);
                yield return new WaitForSeconds(waitTimeAtSpot);
                if (i == patrolSpots.Length - 1)
                    i = 0;
            }
        }
        else throw new Exception(string.Format("NPC {0} is not on a navMesh", id));
        isPatrolRunning = false;
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    private void OnMouseDown()
    { 
        isSelectedToMove = true;
    }

    public void PlayVoice(AudioClip newClip)
    {
        if (isPatrolRunning)
        {
            StopCoroutine(patrol);
            agent.isStopped = true;
            //transform.LookAt(dc.hmd);
        }
            
        voice.clip = newClip;
        voice.Play();
    }

    public void UpdateStressLevel(int change)
    {
        stressLevel += change;

        // Set new state of the patient
    }
}

public enum AIState
{
    Walk,
    Talk,
    Jump,
    Fall,

}
