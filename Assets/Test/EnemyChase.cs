using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Hurt,
    }

    private EnemyState currentState;
    public Transform pointA, pointB;  // Patrol points
    private float patrolSpeed = 3.0f;
    public float maxSightDistance = 5.0f;
    private float fieldOfViewAngle = 90.0f;
    public float attackDistance = 2.0f;
    public float patrolThreshold = 1f;
    private GameObject hitbox;

    public float initialHealth = 100f;  // Initial health of the enemy
    private float currentHealth;
    private bool isDead = false;  // To track if the enemy is already dead
    private bool isTakingDamage = false;  // To track if the enemy is currently taking damage

    private Transform target;
    private Transform currentTarget;
    private NavMeshAgent navAgent;
    private Animator animator;  // Reference to the Animator component

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();  // Get the Animator component
        navAgent.speed = patrolSpeed;  // Set initial speed to patrol speed
        currentTarget = pointB;
        currentState = EnemyState.Patrol;
        currentHealth = initialHealth;
    }

    void Update()
    {
        if (!isDead)
        {
            Debug.Log(currentHealth);

            float distanceToPlayer = Vector3.Distance(target.position, transform.position);
            float distanceToA = Vector3.Distance(target.position, pointA.position);
            float distanceToB = Vector3.Distance(target.position, pointB.position);

            // Check if player is out of range of both points A and B
            if (distanceToA > maxSightDistance && distanceToB > maxSightDistance)
            {
                currentState = EnemyState.Patrol;
            }
            else if (distanceToPlayer < maxSightDistance && IsPlayerInFieldOfView() && currentState != EnemyState.Hurt)
            {
                currentState = EnemyState.Chase;
            }

            switch (currentState)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Hurt:
                    // No movement code during Hurt state
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
        animator.Play("Warrok Walk");

        if (Vector3.Distance(transform.position, pointA.position) < patrolThreshold && currentTarget == pointA)
        {
            currentTarget = pointB;
        }
        else if (Vector3.Distance(transform.position, pointB.position) < patrolThreshold && currentTarget == pointB)
        {
            currentTarget = pointA;
        }
    }


    void Chase()
    {
        navAgent.speed = patrolSpeed * 2;  // Double the patrol speed for chasing
        navAgent.isStopped = false;
        navAgent.SetDestination(target.position);

        animator.Play("Warrok Run");  // Play the Run animation

        // If the player moves to the other side, rotate to face the player
        if (Vector3.Dot(transform.forward, target.position - transform.position) < 0)
        {
            transform.Rotate(0, 180, 0);  // Rotate 180 degrees on the Y-axis
        }

        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            currentState = EnemyState.Attack;
        }
    }

    void Attack()
    {
        navAgent.isStopped = true;
        //hitbox.SetActive(true);

        animator.Play("Warrok Attack");  // Play the Attack animation
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the enemy collided with a sword and isn't already taking damage
        if (other.gameObject.CompareTag("Sword") && !isTakingDamage)
        {
            TakeDamage(10f);  // Example damage amount, you can adjust as needed
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Switch the state to Hurt when taking damage
        currentState = EnemyState.Hurt;

        // Play the damage animation or go to idle state for a short period of time
        StartCoroutine(DamageReaction());

        // Check if the enemy is dead
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    IEnumerator DamageReaction()
    {
        navAgent.isStopped = true;
        animator.Play("Warrok Hurt");
        
        // Wait for a short period of time, for example 2 seconds
        yield return new WaitForSeconds(1f);

        // Check the distance to the player after hurt animation/delay
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
        if (distanceToPlayer < maxSightDistance)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    void Die()
    {
        isDead = true;
        animator.Play("Warrok Dead");

        // Wait for the death animation to finish and then destroy the game object
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the duration of the death animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }
}
