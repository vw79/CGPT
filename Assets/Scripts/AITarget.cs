using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITarget : MonoBehaviour
{
    public NavMeshAgent _enemy;
    public Transform _player;

    void Update()
    {
        _enemy.SetDestination(_player.position);
    }
}
