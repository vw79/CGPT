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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = EnemyState.Chase;
    }

    private void Update()
    {
        if (!IsDead)
        {
            switch (enemyState)
            {
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Dead:
                    Dead();
                    break;
                default:
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

    private void Chase()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            enemyState = EnemyState.Attack;
        }
    }

    private void Attack()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;

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
    }

    public void OnDeath()
    {
        enemyState = EnemyState.Dead;
    }
}
