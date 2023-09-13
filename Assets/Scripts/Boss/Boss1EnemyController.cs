using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1EnemyController : MonoBehaviour
{
    [Header("Animation State Names")]
    [SerializeField] private string runAnimation = "Boss 1 Run";
    [SerializeField] private string attackAnimation = "Boss 1 Attack";
    [SerializeField] private string deadAnimation = "Boss 1 Dead";

    [SerializeField] private float attackActiveTime;
    [SerializeField] private float hitboxDuration;
    [SerializeField] private float attackInterval;

    // Enemy stats
    private float movementSpeed;
    private float attackPower;

    private bool isDead = false;
    private bool isAttacking = false;

    // References to outer objects
    private Transform player;
    private Transform currentTarget;
    private NavMeshAgent navMesh;
    private Animator animator;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinAmount = 1;
    [SerializeField] private GameObject enemy;
    [SerializeField] private AudioSource AttackAudio;
    [SerializeField] private AudioSource DeathAudio;

    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentTarget = player;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        GetStat();

        switch (GetComponent<Boss1EnemyStateController>().GetEnemyState())
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

    private void GetStat()
    {
        movementSpeed = GetComponent<CharacterStat>().GetMovementSpeed();
        attackPower = GetComponent<CharacterStat>().GetAttackDamage();
    }

    private void Attack()
    {
        navMesh.speed = 0;
        transform.LookAt(new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z));
        animator.Play(attackAnimation);
        if (!isAttacking)
        {
            StartCoroutine(AttackActivate());
        }
    }

    private IEnumerator AttackActivate()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackActiveTime);
        hitbox.GetComponent<IHitbox>().SetUpDamage(attackPower, hitboxDuration);
        AttackAudio.Play();
        hitbox.SetActive(true);
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackInterval);
        isAttacking = false;
    }

    private void Chase()
    {
        navMesh.speed = movementSpeed * 3;
        currentTarget = player;
        navMesh.SetDestination(currentTarget.position);
        transform.LookAt(new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z));
        animator.Play(runAnimation);
    }

    private void Dead()
    {
        isDead = true;
        navMesh.speed = 0;
        BlowCoin();
        DeathAudio.Play();
        animator.Play(deadAnimation);
        GetComponent<Collider>().excludeLayers += LayerMask.GetMask("Player");

        StartCoroutine(DestroyAfterAnimation());
    }


    private void BlowCoin()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coin.GetComponent<Coin>().SetCoinAmount(coinAmount);
        coin.transform.position += Vector3.up;
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(2.9f);
        Destroy(enemy);
    }
}
