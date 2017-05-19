using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class AI_Movement : MonoBehaviour
{
    // Public getters
    public int ID { get; private set; }
    public AIState state;

    public Transform[] patrolSpots;
    public float waitTimeAtSpot;

    // References
    NavMeshAgent agent;
    public DialogueController dc;
    AudioSource voice;
    Animator anime;

    bool dialogueEnabled;


    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        try
        {
            dialogueEnabled = true;
            dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        }
        catch(NullReferenceException)
        {
            dialogueEnabled = false;
            Debug.Log("No dialogue contorller available, dialogue is now disabled");
        }
        voice = GetComponent<AudioSource>();
        anime = GetComponent<Animator>();
        patrol = Patrol();

        if(patrolSpots.Length > 0)
            StartCoroutine(patrol);
    }

    private IEnumerator patrol;
    bool isPatrolRunning;
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
        throw new Exception(string.Format("NPC {0} is not on a navMesh", ID));
        isPatrolRunning = false;
    }

    public void PlayVoice(AudioClip newClip)
    {
        if (isPatrolRunning)
        {
            StopCoroutine(patrol);
            agent.isStopped = true;
            transform.LookAt(dc.hmd);
        }
            
        voice.clip = newClip;
        voice.Play();
    }



    public int stressLevel { get; private set; }

    public void UpdateStressLevel(int change)
    {
        stressLevel += change;
    }
}

public enum AIState
{
    Walk,
    Talk,
    Jump,
    Fall,

}
