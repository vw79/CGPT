using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/HealBuff")]
public class RegenBuff : SO_Buff
{
    [SerializeField] private float RegenAmount;
    [SerializeField] private int RegenTime;

    public override void UseBuff(GameObject player)
    {
        player.GetComponent<HealthSystem>().GradHeal(RegenAmount, RegenTime);
    }
}
