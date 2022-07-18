using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private Transform[] waypointTargets;
    [SerializeField]
    private State state;
    [SerializeField]
    int index;
    [SerializeField]
    float delay;

    // animation IDs
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;

    //sounds
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    public enum State
    {
        PATROL,
        ALERT,
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AssignAnimationIDs();

        switch (state)
        {
            default:

            case State.PATROL:
                patrolState();

                break;

            case State.ALERT:


                break;
        }
    }

    private void patrolState()
    {
        //find the waypoint and go to it
        Vector3 target;

        index = index % waypointTargets.Length;
        target = waypointTargets[index].position;

        agent.SetDestination(target);

        // Check if we've reached the destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    _animator.SetFloat(_animIDSpeed, 0f);
                    _animator.SetFloat(_animIDMotionSpeed, 1f);

                    delay += Time.deltaTime;
                    if (delay > 1.5f)
                    {
                        //iterate waypoints
                        index++;
                        delay = 0f;
                    }
                }
            }
        }
        else
        {
            _animator.SetFloat(_animIDSpeed, agent.speed);
            _animator.SetFloat(_animIDMotionSpeed, 1f);
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            }
        }
    }
}
