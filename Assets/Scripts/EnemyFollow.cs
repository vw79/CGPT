using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetedObject;  //targeted object location (eg. position, rotation,...)
    [SerializeField] private NavMeshAgent _enemy;   //follow target AI
    [SerializeField] private float detectionRange = 10;
    [SerializeField] private float attackRange = 5;

    //speed
    private float speed;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _targetedObject.position);
        if(distance >= detectionRange)
        {
            _enemy.isStopped = true;
        }
        else if(distance < detectionRange && distance >= attackRange)
        {
            _enemy.isStopped = false;
            AI_follow();
        }
        else if(distance < attackRange)
        {
            _enemy.isStopped = true;
        }
    }

    private void AI_follow()
    {
        _enemy.SetDestination(_targetedObject.position);
    }
}
