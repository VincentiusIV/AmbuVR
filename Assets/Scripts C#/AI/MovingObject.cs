﻿using System;
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
    public AIBehaviourState state;
    public Animator anime;
    public int questionDialogue;
    public GameObject questionButton;

    [Header("Patrolling")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 2f;

    [Header("Command Behaviour")]
    public GlowObjectCmd outline;
    public Transform player;
    public bool reachedPlayer;

    [Header("Testing References")]
    public AIBehaviourState startState = AIBehaviourState.Idle;
    public TextMesh stateMesh;
    public bool raycastingEnabled = false;

    NavMeshAgent agent;
    AudioSource voice;

    int destPoint = 0;

    private Transform[] points;
    bool isWaitingForNext = false;
    bool visibleToPlayer = false;
    IEnumerator waiting;

    Vector3 customPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        voice = GetComponent<AudioSource>();

        player = AmbuVR.Player.instance.hmd;

        waiting = WaitBeforeNextPoint();

        points = new Transform[0];

        if (outline != null)
            outline.Hide();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        ChangeBehaviour(startState);
    }

    public void ChangeBehaviour(AIBehaviourState newBehaviour, Vector3 custom = new Vector3(), bool forceChange = false)
    {
        if (state == AIBehaviourState.Command && agent.isStopped != true)
            return;

        state = newBehaviour;
        Debug.Log(string.Format("new behaviour of {0} is {1}", gameObject.name, state.ToString()));
        if (isWaitingForNext)
            StopCoroutine(waiting);

        if (stateMesh != null)
            stateMesh.text = state.ToString();

        UpdateAnimator(false);

        switch (state)
        {
            case AIBehaviourState.Idle:
                break;
            case AIBehaviourState.Follow:
                points = new Transform[1];
                points[0] = player;
                break;
            case AIBehaviourState.Patrol:
                points = patrolPoints;
                break;
            case AIBehaviourState.Command:
                points = new Transform[1];
                Transform testCommand = GameObject.Find("TestCommandTransform").transform;
                testCommand.position = custom;
                points[0] = testCommand;
                break;
            default:
                break;
        }
        GotoNextPoint();
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    void GotoNextPoint()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        // Returns if no points have been set up
        if (points.Length == 0 || state == AIBehaviourState.Idle)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;

        if (destPoint > points.Length - 1)
            destPoint = 0;
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
        // add more animations
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, AmbuVR.Player.instance.hmd.position);

        switch (state)
        {
            case AIBehaviourState.Idle:
                break;
            case AIBehaviourState.Follow:
                if (reachedPlayer)
                    break;
                agent.SetDestination(AmbuVR.Player.instance.hmd.position);
                reachedPlayer = agent.remainingDistance < 2f && Vector3.Distance(transform.position, AmbuVR.Player.instance.hmd.position) < 3f;

                if (reachedPlayer)
                {
                    Debug.Log("Distance between AI and player: " + Vector3.Distance(transform.position, AmbuVR.Player.instance.hmd.position) + ", Path remaining: " + agent.remainingDistance);
                    UpdateAnimator(false);
                    agent.isStopped = true;
                    //AmbuVR.Player.instance.SetCanTeleport(false);
                }
                break;
            case AIBehaviourState.Patrol:
                if (!agent.pathPending && agent.remainingDistance < 0.5f || agent.isStopped)
                    GotoNextPoint();
                break;
            case AIBehaviourState.Command:
                if (raycastingEnabled && Input.GetButtonDown("Fire1"))
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

        if(GameFlowManager.instance.state != GameState.Dialogue && !DialogueController.instance.isActive && visibleToPlayer)
        {
            // See how close player is to NPC
            float distanceHMD = Vector3.Distance(transform.position, AmbuVR.Player.instance.hmd.position);
            // When player is in a certain range -> enable ask question button
            if (distanceHMD < 3 && !questionButton.activeInHierarchy)
                UIController.instance.ToggleManually(questionButton, true);
            else if (distanceHMD > 3 && questionButton.activeInHierarchy)
                UIController.instance.ToggleManually(questionButton, false);

            if (visibleToPlayer)
            {
                // Rotate towards player
                Vector3 direction = transform.position - AmbuVR.Player.instance.hmd.position;
                direction = new Vector3(direction.x, 0f, direction.z);
                Quaternion rotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, .02f);
            }

        }
        else if(questionButton.activeInHierarchy)
        {
            UIController.instance.ToggleManually(questionButton, false);
        }
    }

    public void BecameVisible()
    {
        visibleToPlayer = true;
    }

    public void BecameInvisible()
    {
        visibleToPlayer = false;
    }

    /*private void OnMouseEnter()
    {
        outline.Show();
    }
    private void OnMouseExit()
    {
        outline.Hide();
    }
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            ChangeBehaviour(AIState.Idle);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            ChangeBehaviour(AIState.Follow);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            ChangeBehaviour(AIState.Patrol);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            ChangeBehaviour(AIState.Command);
    }*/

    // Plays a voice from this AI, 
    public void PlayVoice(AudioClip newClip)
    {
        ChangeBehaviour(AIBehaviourState.Idle);
        voice.clip = newClip;
        voice.Play();
    }

    public void StartQuestion()
    {
        UIController.instance.ToggleManually(questionButton, false);
        DialogueController.instance.StartCoroutine(DialogueController.instance.DialogueSession(questionDialogue));
    }
}
// Enum for animations
public enum AIBehaviourState
{
    Idle,
    Follow,
    Patrol,
    Command
}


public enum AIEmotionalState
{
    Normal,
    Crying,
    Sad,
}