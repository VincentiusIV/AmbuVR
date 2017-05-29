using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class MovingObject : MonoBehaviour
{
    [Header("State")]
    public int id;
    public AIState state;
    public Animator anime;

    [Header("Patrolling")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 2f;

    [Header("Command Behaviour")]
    public cakeslice.Outline outline;
    public Transform player;

    NavMeshAgent agent;   
    AudioSource voice;

    int destPoint = 0;

    private Transform[] points;
    bool isWaitingForNext = false;
    IEnumerator waiting;

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        voice = GetComponent<AudioSource>();

        player = Camera.main.transform;//GameObject.FindWithTag("Player").transform;
        waiting = WaitBeforeNextPoint();

        points = new Transform[0];
        state = AIState.Idle;

        if(outline != null)
            outline.enabled = false;

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }

    void ChangeBehaviour(AIState newBehaviour)
    {
        state = newBehaviour;
        Debug.Log(string.Format("At {0} new behaviour of {1} is {2}", Time.time, gameObject.name, state.ToString()));
        if (isWaitingForNext)
            StopCoroutine(waiting);

        UpdateAnimator(false);

        switch (state)
        {
            case AIState.Idle:
                break;
            case AIState.Follow:
                points = new Transform[1];
                points[0] = player;
                break;
            case AIState.Patrol:
                points = patrolPoints;
                break;
            case AIState.Command:
                points = new Transform[0];
                break;
            default:
                break;
        }
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0 || state == AIState.Idle)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
        // Set animation
        UpdateAnimator(true);

        destPoint = (destPoint + 1) % points.Length;
    }

    IEnumerator WaitBeforeNextPoint()
    {
        isWaitingForNext = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        isWaitingForNext = false;
        GotoNextPoint();
        
    }

    void UpdateAnimator(bool isMoving)
    {
        anime.SetBool("isWalking", isMoving);
    }

    private void Update()
    {
        switch (state)
        {
            case AIState.Idle:
                break;
            case AIState.Follow:
            case AIState.Patrol:
                // Choose the next destination point when the agent gets
                // close to the current one.
                if (!agent.pathPending && agent.remainingDistance < 0.5f || agent.isStopped)
                    GotoNextPoint();
                break;
            case AIState.Command:
                if (Input.GetButtonDown("Fire1"))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        points = new Transform[1];
                        Transform testCommand = GameObject.Find("TestCommandTransform").transform;
                        testCommand.position = hit.point;
                        points[0] = testCommand;
                        GotoNextPoint();
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }
    private void OnMouseExit()
    {
        outline.enabled = false;
    }
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            ChangeBehaviour(AIState.Idle);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            ChangeBehaviour(AIState.Follow);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            ChangeBehaviour(AIState.Patrol);
        if(Input.GetKeyDown(KeyCode.Keypad4))
            ChangeBehaviour(AIState.Command);
    }

    // Plays a voice from this AI, 
    public void PlayVoice(AudioClip newClip)
    {
        ChangeBehaviour(AIState.Idle);
        voice.clip = newClip;
        voice.Play();
    }
}
// Enum for animations
public enum AIState
{
    Idle,
    Follow,
    Patrol,
    Command
}
