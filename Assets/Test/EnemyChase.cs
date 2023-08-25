using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Hurt,
}

public class EnemyChase : MonoBehaviour
{
    [Header("Animation State Names")]
    [SerializeField] private string walkAnimation = "Warrok Walk";
    [SerializeField] private string runAnimation = "Warrok Run";
    [SerializeField] private string attackAnimation = "Warrok Attack";
    [SerializeField] private string hurtAnimation = "Warrok Hurt";
    [SerializeField] private string deadAnimation = "Warrok Dead";

    [Header("Enemy Parameters")]
    [SerializeField] private float maxSightDistance = 5.0f;
    [SerializeField] private float attackDistance = 2.0f;
    public Transform pointA, pointB;
    private float movementSpeed;
    private float damage;

    private EnemyState currentState;

    private float fieldOfViewAngle = 90.0f;
    private float patrolThreshold = 1f;
    private bool isDead = false;  // To track if the enemy is already dead
    private bool isAttacking = false;

    [Header("")]
    [SerializeField] private GameObject hitbox;
    [SerializeField] private float deathAnimationDuration = 3.0f;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinAmount = 1;

    private Transform target;
    private Transform currentTarget;
    private NavMeshAgent navAgent;
    private Animator animator;  // Reference to the Animator component

    void Start()
    {
        GetStat();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();  // Get the Animator component
        navAgent.speed = movementSpeed;  // Set initial speed to patrol speed
        currentTarget = pointB;
        currentState = EnemyState.Patrol;
    }

    void Update()
    {
        if (!isDead)
        {
            GetStat();

            float distanceToPlayer = Vector3.Distance(target.position, transform.position);
            float distanceToA = Vector3.Distance(target.position, pointA.position);
            float distanceToB = Vector3.Distance(target.position, pointB.position);

            // Check if player is out of range of both points A and B
            if ((distanceToA > maxSightDistance && distanceToB > maxSightDistance))
            {
                currentState = EnemyState.Patrol;
            }
            else if ((distanceToPlayer > attackDistance) && (distanceToPlayer < maxSightDistance) && IsPlayerInFieldOfView() && (currentState != EnemyState.Hurt))
            {
                Debug.Log("Player in sight");
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
                    if (!isAttacking)
                    {
                        Attack();
                    }
                    break;
                case EnemyState.Hurt:
                    // No movement code during Hurt state
                    break;

            }
        }
    }

    private void GetStat()
    {
        movementSpeed = GetComponent<CharacterStat>().GetMovementSpeed();
        damage = GetComponent<CharacterStat>().GetAttackDamage();
        hitbox.GetComponent<HitboxManager>().SetUpDamage(damage);
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);
        return angle < fieldOfViewAngle * 0.5f;
    }

    void Patrol()
    {
        navAgent.speed = movementSpeed;  // Set speed to patrol speed
        navAgent.isStopped = false;

        // Make the enemy face the current target point before moving towards it
        transform.LookAt(currentTarget);

        navAgent.SetDestination(currentTarget.position);
        animator.Play(walkAnimation);

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
        //Debug.Log("Chasing");
        //Debug.Log(Vector3.Distance(transform.position, target.position));
        navAgent.speed = movementSpeed * 2;  // Double the patrol speed for chasing
        navAgent.isStopped = false;
        navAgent.SetDestination(target.position);

        animator.Play(runAnimation);  // Play the Run animation

        // If the player moves to the other side, rotate to face the player
        if (Vector3.Dot(transform.forward, target.position - transform.position) < 0)
        {
            transform.Rotate(0, 180, 0);  // Rotate 180 degrees on the Y-axis
        }

        if (Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            Debug.Log("Attack");
            currentState = EnemyState.Attack;
        }
    }

    void Attack()
    {
        Debug.Log("Attacking");
        navAgent.isStopped = true;
        hitbox.SetActive(true);

        animator.Play(attackAnimation);  // Play the Attack animation
        StartCoroutine(AttackAnim());
    }

    IEnumerator AttackAnim()
    {
        isAttacking = true;
        yield return new WaitForSeconds(2f);
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
        if (distanceToPlayer < maxSightDistance)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
        isAttacking = false;
    }

    public void TakeDamage()
    {
        // Switch the state to Hurt when taking damage
        currentState = EnemyState.Hurt;

        // Play the damage animation or go to idle state for a short period of time
        StartCoroutine(DamageReaction());
    }

    IEnumerator DamageReaction()
    {
        navAgent.isStopped = true;
        animator.Play(hurtAnimation);

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

    public void Die()
    {
        if (!isDead)
        {
            BlowCoin();
        }
        isDead = true;
        GetComponent<Collider>().excludeLayers += LayerMask.GetMask("Player");
        animator.Play(deadAnimation);

        // Wait for the death animation to finish and then destroy the game object
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the duration of the death animation + 1 second
        yield return new WaitForSeconds(deathAnimationDuration);
        Destroy(gameObject);
    }

    private void BlowCoin()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coin.GetComponent<Coin>().SetCoinAmount(coinAmount);
        coin.transform.position += Vector3.up;
        //coin.GetComponent<Rigidbody>().AddForce(new Vector3(10,10,10));
        //Random.Range(0, 1), Random.Range(8, 10), Random.Range(0, 1)
    }
}