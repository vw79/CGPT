using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/HealBuff")]
public class HealBuff : SO_Buff
{
    [SerializeField] private float healAmount = 40f;

    public override void UseBuff(GameObject player)
    {
        player.GetComponent<HealthSystem>().Heal(healAmount);
    }
}
