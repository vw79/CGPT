using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2EnemyStateController : MonoBehaviour
{
    public EnemyState enemyState;

    private Transform player;

    [SerializeField] private float seekRadius;
    [SerializeField] private float attackRadius;

    private bool IsAttacking = false;
    private bool IsDead = false;

    private float minLimitX;
    private float maxLimitX;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = EnemyState.Patrol;
        float[] waypointPosition = GetComponent<Boss2EnemyController>().GetWaypointPosition();

        minLimitX = Mathf.Min(waypointPosition);
        maxLimitX = Mathf.Max(waypointPosition);
    }

    private void Update()
    {
        if (!IsDead)
        {
            switch (enemyState)
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
                case EnemyState.Dead:
                    Dead();
                    break;
            }
        }
    }

    private void Dead()
    {
        IsDead = true;
    }

    public EnemyState GetEnemyState()
    {
        return enemyState;
    }

    private void Patrol()
    {
        Debug.Log("Patrol");
        if (Vector3.Distance(transform.position, player.position) < seekRadius && player.position.x >= minLimitX && player.position.x <= maxLimitX)
        {
            enemyState = EnemyState.Chase;
        }
    }

    private void Chase()
    {
        Debug.Log("Chase");
        if (Vector3.Distance(transform.position, player.position) >= seekRadius || player.position.x < minLimitX || player.position.x > maxLimitX)
        {
            enemyState = EnemyState.Patrol;
        }
        else if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            enemyState = EnemyState.Attack;
        }
    }

    private void Attack()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;

            //Insert any attack animation
            Debug.Log("Attack");


            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.83f);
        if (IsDead)
        {
            yield return null;
        }

        IsAttacking = false;

        if (Vector3.Distance(transform.position, player.position) > attackRadius)
        {
            enemyState = EnemyState.Chase;
        }
        else if (Vector3.Distance(transform.position, player.position) > seekRadius)
        {
            enemyState = EnemyState.Patrol;
        }
    }

    public void OnDeath()
    {
        enemyState = EnemyState.Dead;
    }
}
