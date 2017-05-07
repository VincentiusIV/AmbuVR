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

    // Serialized
    [SerializeField] private Transform[] patrolSpots;
    public float waitTimeAtSpot;

    NavMeshAgent agent;
    DialogueController dc;
    AudioSource voice;

    public AIState state;
    Animator anime;

	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
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
        if(agent.isOnNavMesh)
        {
            for (int i = 0; i < patrolSpots.Length; i++)
            {
                agent.SetDestination(patrolSpots[i].position);
                yield return new WaitUntil(() => transform.position == agent.destination);
                yield return new WaitForSeconds(waitTimeAtSpot);
            }
            StartCoroutine(patrol);
        }
        else throw new Exception(string.Format("NPC {0} is not on a navMesh", ID));
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
    Talk
}
