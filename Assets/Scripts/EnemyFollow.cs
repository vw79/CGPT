using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetedObject;  //targeted object location (eg. position, rotation,...)
    [SerializeField] private NavMeshAgent _enemy;   //follow target AI

    //speed
    private float speed;

    void Update()
    {
        AI_follow();
    }

    private void AI_follow()
    {
        _enemy.SetDestination(_targetedObject.position);
        
    }
}
