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
    public AIBehaviourState behaviouralState;
    public AIMovementState movementState;
    public AIEmotionalState emotionalState;
    public float anger;
    public float transitionLerp = .05f;
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
    public float distanceToPlayer = 2f;

    [Header("Testing References")]
    public AIBehaviourState startState = AIBehaviourState.Idle;

    public TextMesh stateMesh;
    public bool raycastingEnabled = false;

    NavMeshAgent agent;
    AudioSource voice;

    int destPoint = 0;

    private Transform[] points;
    bool isWaitingForNext = false;
    IEnumerator waiting;

    Vector3 customPoint;

    //--- Bools ---//
    bool visibleToPlayer = false;

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
        if (behaviouralState == AIBehaviourState.Command && agent.isStopped != true)
            return;

        behaviouralState = newBehaviour;
        Debug.Log(string.Format("new behaviour of {0} is {1}", gameObject.name, behaviouralState.ToString()));
        if (isWaitingForNext)
            StopCoroutine(waiting);

        if (stateMesh != null)
            stateMesh.text = behaviouralState.ToString();

        switch (behaviouralState)
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
        if (points.Length == 0 || behaviouralState == AIBehaviourState.Idle)
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
        SetAnimator(AIMovementState.Walking);

        destPoint = (destPoint + 1) % points.Length;
    }

    IEnumerator WaitBeforeNextPoint()
    {
        isWaitingForNext = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        isWaitingForNext = false;
        GotoNextPoint();

    }

    private void Update()
    {
        Debug.DrawLine(transform.position, AmbuVR.Player.instance.hmd.position);
        reachedPlayer = Vector3.Distance(transform.position, AmbuVR.Player.instance.feet.position) < distanceToPlayer;

        switch (behaviouralState)
        {
            case AIBehaviourState.Idle:
                break;
            case AIBehaviourState.Follow:  
                if (reachedPlayer)
                    break;
                agent.SetDestination(AmbuVR.Player.instance.hmd.position);
                reachedPlayer = agent.remainingDistance < 2f && Vector3.Distance(transform.position, AmbuVR.Player.instance.feet.position) < distanceToPlayer;

                if (reachedPlayer)
                {
                    Debug.Log("Distance between AI and player: " + Vector3.Distance(transform.position, AmbuVR.Player.instance.hmd.position) + ", Path remaining: " + agent.remainingDistance);
                    SetAnimator(AIMovementState.Idle);
                    agent.isStopped = true;
                }
                break;
            case AIBehaviourState.Patrol:
                if (!agent.pathPending && agent.remainingDistance < 0.5f || agent.isStopped)
                    GotoNextPoint();
                break;
            case AIBehaviourState.Command:
                /*if (raycastingEnabled && Input.GetButtonDown("Fire1"))
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
                }*/
                break;
            default:
                break;
        }
        //--- For asking questions ---//
        if(GameFlowManager.instance.state != GameState.Dialogue && !DialogueController.instance.isActive && visibleToPlayer)
        {
            // See how close player is to NPC
            float distanceHMD = Vector3.Distance(transform.position, AmbuVR.Player.instance.feet.position);
            // When player is in a certain range -> enable ask question button
            if (distanceHMD < 1.5 && !questionButton.activeInHierarchy)
                UIController.instance.ToggleManually(questionButton, true);
            else if (distanceHMD > 1.5 && questionButton.activeInHierarchy)
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

        // Updates animator to idle when AI stopped talking
        if (!voice.isPlaying && movementState == AIMovementState.Talking)
            SetAnimator(AIMovementState.Idle);

        anger = Mathf.Lerp(anger, (int)emotionalState * 0.5f, transitionLerp);
        anime.SetFloat("Anger", anger);
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
    public void PlayVoice(AudioClip newClip, AIEmotionalState _emoState)
    {
        ChangeBehaviour(AIBehaviourState.Idle);
        SetAnimator(AIMovementState.Talking);
        emotionalState = _emoState;
        voice.clip = newClip;
        voice.Play();
    }

    public void StartQuestion()
    {
        UIController.instance.ToggleManually(questionButton, false);
        DialogueController.instance.StartCoroutine(DialogueController.instance.DialogueSession(questionDialogue));
    }

    private void SetAnimator(AIMovementState _movementState)
    {
        movementState = _movementState;

        switch (movementState)
        {
            case AIMovementState.Idle:
                SetAnimatorBools(false, false);
                break;
            case AIMovementState.Walking:
                SetAnimatorBools(true, false);
                break;
            case AIMovementState.Talking:
                SetAnimatorBools(false, true);
                break;
            default:
                break;
        }
    }

    private void SetAnimatorBools(bool _isWalking, bool _isTalking)
    {
        anime.SetBool("isWalking", _isWalking);
        anime.SetBool("isTalking", _isTalking);
    }
}
// Enum for behaviour
public enum AIBehaviourState
{
    Idle,
    Follow,
    Patrol,
    Command
}
// Enum for animations
public enum AIMovementState
{
    Idle = 0,
    Walking = 1,
    Talking = 2
}
// Enum for the intensity of some animations
public enum AIEmotionalState
{
    Normal = 0,
    Aggrevated = 1,
    Angry = 2,
    Crying = 3,
}