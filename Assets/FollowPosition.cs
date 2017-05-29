using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPosition : MonoBehaviour {

    [Header("Data")]
    public Transform parent;
    public Vector3 offset = new Vector3(-1, 0f, 2.5f);

    public NavMeshAgent agent;

    private void Start()
    {
        agent.SetDestination(transform.position);
    }
    private void Update()
    {
        Vector3 newPosition = new Vector3(parent.position.x + offset.x, 0f, parent.position.z + offset.z);
        transform.position = newPosition;

        if (agent.isStopped == false)
            agent.SetDestination(transform.position);

        if (agent.remainingDistance < .5f)
            agent.isStopped = true;
    }
}
