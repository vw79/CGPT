using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum StatType { Attack, Health }
    public float attackPower = 10f;
    public float maxHealth = 100f;
    public float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth; // Initialize current health to max health
    }

    public void IncreaseAttack(float amount)
    {
        attackPower += amount;
    }

    public void IncreaseHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount; // Optionally increase current health as well
    }

    public void UpgradeAttack()
    {
        // Implement your logic for upgrading attack. For example:
        attackPower += 5f;
    }

    public void UpgradeHealth()
    {
        // Implement your logic for upgrading health. For example:
        maxHealth += 20f;
        currentHealth = maxHealth; // Reset current health to max after upgrade
    }

    // ... Other player-related methods ...
}
