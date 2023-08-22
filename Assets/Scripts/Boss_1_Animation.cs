using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_1_Animation : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Attack,
    }

    private EnemyState currentState;
    public Transform pointA, pointB;  // Patrol points
    private float patrolSpeed = 3.0f;
    public float maxSightDistance = 5.0f;
    private float fieldOfViewAngle = 90.0f;
    public float attackDistance = 2.0f;
    public float patrolThreshold = 1f;
    private GameObject hitbox;

    private bool isDead = false;  // To track if the enemy is already dead

    private Transform target;
    private Transform currentTarget;
    private NavMeshAgent navAgent;
    private Animator animator;  // Reference to the Animator component

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();  // Get the Animator component
        navAgent.speed = patrolSpeed;  // Set initial speed to patrol speed
        currentTarget = pointB;
        currentState = EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            float distanceToPlayer = Vector3.Distance(target.position, transform.position);
            float distanceToA = Vector3.Distance(target.position, pointA.position);
            float distanceToB = Vector3.Distance(target.position, pointB.position);

            // Check if player is out of range of both points A and B
            if (distanceToA > maxSightDistance && distanceToB > maxSightDistance)
            {
                currentState = EnemyState.Patrol;
            }

            switch (currentState)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }
        }
    }
    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = target.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);
        return angle < fieldOfViewAngle * 0.5f;
    }
    void Patrol()
    {
        navAgent.speed = patrolSpeed;  // Set speed to patrol speed
        navAgent.isStopped = false;

        // Make the enemy face the current target point before moving towards it
        transform.LookAt(currentTarget);

        navAgent.SetDestination(currentTarget.position);
        animator.Play("Boss_1_walk");

        if (Vector3.Distance(transform.position, pointA.position) < patrolThreshold && currentTarget == pointA)
        {
            currentTarget = pointB;
        }
        else if (Vector3.Distance(transform.position, pointB.position) < patrolThreshold && currentTarget == pointB)
        {
            currentTarget = pointA;
        }
    }
    void Attack()
    {
        Debug.Log("IsAtacking");
        navAgent.isStopped = true;
        //hitbox.SetActive(true);

        animator.Play("Boss_1_attack");  // Play the Attack animation
    }
}
