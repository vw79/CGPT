using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenBuff : MonoBehaviour
{
    [SerializeField] private float RegenAmount;
    [SerializeField] private int RegenTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {   
            other.GetComponent<HealthSystem>().GradHeal(RegenAmount,RegenTime);
            Destroy(gameObject);
        }
    }
}
