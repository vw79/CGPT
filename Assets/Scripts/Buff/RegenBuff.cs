using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenBuff : MonoBehaviour , IBuff
{
    private HealthSystem playerhealth;
    [SerializeField] private Sprite icon;

    [SerializeField] private float RegenAmount;
    [SerializeField] private int RegenTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerhealth = other.GetComponent<HealthSystem>();
            PlayerStat playerInventory = other.GetComponent<PlayerStat>();
            playerInventory.AddBuff(this);
            DisableExistance();
        }
        else if (other.CompareTag("Enemy"))
        {
            playerhealth = other.GetComponent<HealthSystem>();
            UseBuff();
        }
    }

    public void UseBuff()
    {
        playerhealth.GradHeal(RegenAmount, RegenTime);
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
