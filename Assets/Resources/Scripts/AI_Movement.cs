using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Movement : MonoBehaviour
{
    // Public getters
    [SerializeField]public int ID { get; private set; }

    // Serialized
    [SerializeField] private Transform[] spots;

    NavMeshAgent agent;
    DialogueController dc;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        dc = GameObject.FindWithTag("DialogueController").GetComponent<DialogueController>();
        
        if(spots.Length > 0)
            StartCoroutine(Patrol());
    }
	
	IEnumerator Patrol()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            agent.SetDestination(spots[i].position);
            yield return new WaitUntil(() => transform.position == agent.destination);
        }
        StartCoroutine(Patrol());
    }

    public void DialogueRequest()
    {
        dc.StartDialogue();
    }
}
