using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SpeedBuff")]
public class SpeedBuff : SO_Buff
{
    private CharacterStat playerstat;

    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedBuffDuration;

    public override void UseBuff(GameObject player)
    {
        playerstat = player.GetComponent<CharacterStat>();
        playerstat.SetMovementSpeed(speedMultiplier);
    }

    public override IEnumerator ResetBuff()
    {
        yield return new WaitForSeconds(speedBuffDuration);
        playerstat.SetMovementSpeed(1/speedMultiplier);
    }
}
