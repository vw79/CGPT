using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/ProShieldBuff")]

public class ProShieldBuff : SO_Buff
{
    [SerializeField] private float shieldAmount = 40f;

    public override void UseBuff(GameObject player)
    {
        player.GetComponent<HealthSystem>().AddShield(shieldAmount);
    }
}
