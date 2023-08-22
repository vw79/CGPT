using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuff : MonoBehaviour , IBuff
{
    private HealthSystem playerhealth;
    [SerializeField] private Sprite icon;

    [SerializeField] private float healAmount = 8f;
    private float HealB = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerhealth = other.GetComponent<HealthSystem>();
            PlayerStat playerInventory = other.GetComponent<PlayerStat>();
            if (playerInventory.AddBuff(this))
            {
                DisableExistance();
            }
            else
            {
                UseBuff();
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            playerhealth = other.GetComponent<HealthSystem>();
            UseBuff();
        }
    }

    public void UseBuff()
    {
        playerhealth.Heal(healAmount);
        Destroy(gameObject);
    }

    private void DisableExistance()
    {
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    public Sprite GetIcon()
    {
        return icon;
    }
}
