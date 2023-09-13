using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
    What I wish to do:
    This script can be a system for every entity
    When entity wants to change the value or use the value to calculation
    They should assign from this script
*/

public class CharacterStat : MonoBehaviour
{
    [SerializeField] private float base_attack;
    public float attack_modifier = 1f;     //Buff or debuff for attack

    [SerializeField] private float movement_speed;
    public float movement_modifier = 1f;   //Buff or debuff for movement

    public float GetAttackDamage()
    {
        return base_attack * attack_modifier;
    }

    public void SetAttackModifier(float modifier)
    {
        attack_modifier *= modifier;
    }

    public void AddBaseAttack(float amount)
    {
        base_attack += amount;
    }

    public float GetMovementSpeed()
    {
        return movement_speed * movement_modifier;
    }

    public void SetMovementSpeed(float modifier)
    {
        movement_speed *= modifier;
    }

    public void AddMovementSpeed(float amount)
    {
        movement_speed += amount;
    }
}
