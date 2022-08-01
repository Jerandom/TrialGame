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

    [SerializeField]
    float walkSpeed = 2f;
    [SerializeField]
    float runSpeed = 3.5f;
    [SerializeField]
    float viewRadius = 6f;
    [SerializeField]
    float viewAngle = 60f;

    [SerializeField]
    LayerMask PlayerMask;
    [SerializeField]
    LayerMask ObstacleMask;

    Vector3 PlayerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;

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

    private void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_PlayerInRange = false;

        agent.isStopped = false;
        agent.speed = walkSpeed;

    }

    private void Update()
    {
        AssignAnimationIDs();

        switch (state)
        {
            default:

            case State.PATROL:
                patrolState();
                SearchPlayer();

                break;

            case State.ALERT:
                ChasePlayer(m_PlayerPosition);
                SearchPlayer();

                break;
        }
    }

    private void patrolState()
    {
        //find the waypoint and go to it
        Vector3 target;
        agent.speed = walkSpeed;

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
                        //iterate waypoints with delay
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

    private void ChasePlayer(Vector3 player)
    {
        agent.SetDestination(player);

        if (Vector3.Distance(transform.position, player) <= 3f)
        {
            m_PlayerNear = false;
            agent.isStopped = false;
            agent.speed = runSpeed;
        }
        
        if (!m_PlayerInRange)
        {
            // Check if we've reached the destination
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        agent.speed = 0;
                        _animator.SetFloat(_animIDSpeed, 0f);
                        _animator.SetFloat(_animIDMotionSpeed, 1f);

                        delay += Time.deltaTime;
                        if (delay > 5f)
                        {
                            //switch state with a delay
                            state = State.PATROL;
                            agent.speed = walkSpeed;
                            delay = 0f;
                        }
                    }
                }
            }
        }

        _animator.SetFloat(_animIDSpeed, agent.speed);
        _animator.SetFloat(_animIDMotionSpeed, 1f);
    }

    private void SearchPlayer()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, PlayerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            //check if player is within the sphere
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                //inside view angle
                float distToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, ObstacleMask))
                {
                    m_PlayerInRange = true;
                    state = State.ALERT;
                }
                //outside view angle
                else
                {
                    m_PlayerInRange = false;
                }
            }
            //outside view distance
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }

            //get player position
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, viewRadius);
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