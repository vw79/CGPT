using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    private EnemyState enemyState;

    private Transform player;

    [SerializeField] private Transform[] waypoints = new Transform[2];
    [SerializeField] private float seekRadius;
    [SerializeField] private float attackRadius;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = EnemyState.Patrol;
    }

    private void Update()
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
        }
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
        if (Vector3.Distance(transform.position, player.position) > seekRadius)
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
        Debug.Log("Attack");
        if (Vector3.Distance(transform.position, player.position) > attackRadius)
        {
            enemyState = EnemyState.Chase;
        }
    }

    private void Hurt()
    {
        Debug.Log("Hurt");
    }
}
