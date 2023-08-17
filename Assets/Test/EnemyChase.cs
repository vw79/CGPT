using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Import the NavMesh namespace

public class EnhancedEnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol
    }

    public EnemyState currentState;
    public Transform pointA, pointB;  // Patrol points
    public float maxSightDistance = 5.0f;
    public float fieldOfViewAngle = 90.0f;
    public float maxViewDistance = 10.0f;
    float patrolThreshold = 0.5f;  // Increase the distance tolerance

    private Transform target;
    private Transform currentTarget;
    private NavMeshAgent navAgent;  // Use NavMeshAgent instead

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();  // Get the NavMeshAgent component
        currentTarget = pointB;
        currentState = EnemyState.Patrol;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
        }
    }

    void Idle()
    {
        navAgent.isStopped = true;
    }

    void Patrol()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(currentTarget.position);

        if (Vector2.Distance(transform.position, pointA.position) < patrolThreshold)
        {
            currentTarget = pointB;
        }
        else if (Vector2.Distance(transform.position, pointB.position) < patrolThreshold)
        {
            currentTarget = pointA;
        }
    }
}
