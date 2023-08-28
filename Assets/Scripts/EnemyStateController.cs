using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class EnemyStateController : MonoBehaviour
{
    public EnemyState enemyState;

    private Transform player;

    
    [SerializeField] private float seekRadius;
    [SerializeField] private float attackRadius;

    private bool IsAttacking = false;
    private bool IsDead = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = EnemyState.Patrol;
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
                case EnemyState.Hurt:
                    Hurt();
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
        if (Vector3.Distance(transform.position, player.position) < seekRadius)
        {
            enemyState = EnemyState.Chase;
        }
    }

    private void Chase()
    {
        Debug.Log("Chase");
        if (Vector3.Distance(transform.position, player.position) >= seekRadius)
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
        if(!IsAttacking)
        {
            IsAttacking = true;

            //Insert any attack animation
            Debug.Log("Attack");

            
            StartCoroutine(ResetAttack());
        }
    }

    private void Hurt()
    {
        Debug.Log("Hurt");

        StartCoroutine(ResetHurt());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(2);
        if(IsDead)
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

    private IEnumerator ResetHurt()
    {
        yield return new WaitForSeconds(0.5f);
        if(Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            enemyState = EnemyState.Attack;
        }
        else if(Vector3.Distance(transform.position, player.position) < seekRadius)
        {
            enemyState = EnemyState.Chase;
        }
        else
        {
            enemyState = EnemyState.Patrol;
        }
    }

    public void OnHurt()
    {
        enemyState = EnemyState.Hurt;
    }

    public void OnDeath()
    {
        enemyState = EnemyState.Dead;
    }
}
