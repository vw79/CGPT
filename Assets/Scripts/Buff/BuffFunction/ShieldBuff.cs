using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/ShieldBuff")]

public class ShieldBuff : SO_Buff
{
    [SerializeField] private float shieldAmount = 40f;

    public override void UseBuff(GameObject player)
    {
        player.GetComponent<HealthSystem>().AddShield(shieldAmount);
    }
}
