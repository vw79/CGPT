using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonShop : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject HUD;

    [Header("Shop Items")]
    [Header("Add Max Health")]
    [SerializeField] private int healthPrice = 100;
    [SerializeField] private float healthIncrease = 20f;

    [Header("Add Base Attack")]
    [SerializeField] private int attackPrice = 100;
    [SerializeField] private float attackIncrease = 5f;

    [Header("Add Max Shield")]
    [SerializeField] private int shieldPrice = 100;
    [SerializeField] private float shieldIncrease = 20f;

    private bool playerInTrigger = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInTrigger)
        {
            ShopUI.SetActive(true);
            HUD.SetActive(false);
        }
        else if((Input.GetKeyDown(KeyCode.Escape) && playerInTrigger) || !playerInTrigger)
        {
            QuitShop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    public void QuitShop()
    {
        ShopUI.SetActive(false);
        HUD.SetActive(true);
    }

    public void BuyHealth()
    {
        if(player.GetComponent<PlayerStat>().GetCoin() >= healthPrice)
        {
            player.GetComponent<PlayerStat>().LoseCoin(healthPrice);
            player.GetComponent<HealthSystem>().AddMaxHealth(healthIncrease);
        }
    }

    public void BuyAttack()
    {
        if (player.GetComponent<PlayerStat>().GetCoin() >= attackPrice)
        {
            player.GetComponent<PlayerStat>().LoseCoin(attackPrice);
            player.GetComponent<CharacterStat>().AddBaseAttack(attackIncrease);
        }
    }

    public void BuyShield()
    {
        if (player.GetComponent<PlayerStat>().GetCoin() >= shieldPrice)
        {
            player.GetComponent<PlayerStat>().LoseCoin(shieldPrice);
            player.GetComponent<HealthSystem>().AddMaxShield(shieldIncrease);
        }
    }
}
