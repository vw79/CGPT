using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AttackBuff")]
public class AttackBuff : SO_Buff
{
    private CharacterStat playerstat;

    [SerializeField] private float attackMultiplier;
    [SerializeField] private float attackBuffDuration;

    public override void UseBuff(GameObject player)
    {
        playerstat = player.GetComponent<CharacterStat>();
        playerstat.SetAttackModifier(attackMultiplier);
    }

    public override IEnumerator ResetBuff()
    {
        yield return new WaitForSeconds(attackBuffDuration);
        playerstat.SetAttackModifier(1 / attackMultiplier);
    }
}
