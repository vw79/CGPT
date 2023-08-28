using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints = new Transform[2];

    // Enemy stats
    private float movementSpeed;
    private float attackPower;

    private bool isDead = false;

    // References to outer objects
    private Transform player;
    private Transform currentTarget;
    private NavMeshAgent navMesh;
    private Animator animator;

    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        currentTarget = waypoints[0];

        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        GetStat();

        switch (GetComponent<EnemyStateController>().GetEnemyState())
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

    private void GetStat()
    {
        movementSpeed = GetComponent<CharacterStat>().GetMovementSpeed();
        attackPower = GetComponent<CharacterStat>().GetAttackDamage();
    }

    private void Hurt()
    {
        animator.Play("Warrok Hurt");
    }

    private void Attack()
    {
        navMesh.speed = 0;
        transform.LookAt(new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z));
        animator.Play("Warrok Attack");
    }

    private void Chase()
    {
        navMesh.speed = movementSpeed * 2;
        currentTarget = player;
        transform.LookAt(new Vector3(currentTarget.position.x,transform.position.y,currentTarget.position.z));
        animator.Play("Warrok Run");
    }

    private void Patrol()
    {
        navMesh.speed = movementSpeed;
        navMesh.SetDestination(currentTarget.position);
        animator.Play("Warrok Walk");

        // Change waypoint if enemy has reached current waypoint
        if(Vector3.Distance(transform.position, currentTarget.position) < 1f)
        {
            if(currentTarget == waypoints[0])
            {
                currentTarget = waypoints[1];
            }
            else
            {
                currentTarget = waypoints[0];
            }
        }
    }

    private void Dead()
    {
        isDead = true;
        animator.Play("Warrok Dead");
        GetComponent<Collider>().excludeLayers += LayerMask.GetMask("Player");
    }
}
