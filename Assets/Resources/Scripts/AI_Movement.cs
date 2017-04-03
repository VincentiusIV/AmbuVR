using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Movement : MonoBehaviour {

    // Serialized
    [SerializeField] private Transform[] spots;

    NavMeshAgent agent;

	void Start () {
        agent = GetComponent<NavMeshAgent>();

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

    void SaySomething()
    {

    }
}
