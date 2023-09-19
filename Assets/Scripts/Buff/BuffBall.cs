using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBall : MonoBehaviour
{
    public SO_Buff buff;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerStat playerInventory = other.GetComponent<PlayerStat>();
            if (!playerInventory.AddBuffIntoInventory(buff))
            {

            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("Enemy"))
        {
            buff.UseBuff(other.gameObject);
            if (!buff.isOneTimeUse)
            {
                StartCoroutine(buff.ResetBuff());
            }
            Destroy(gameObject);
        }
    }
}
