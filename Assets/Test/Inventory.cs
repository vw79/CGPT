using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class SkillOrb
    {
        public string orbName;
        public string description;
        public enum OrbType { AttackBoost, HealthBoost } // Example types of orbs
        public OrbType type;
        public float boostAmount; // The amount by which the orb boosts a stat

        // ... add any other properties or methods related to skill orbs
    }

    private SkillOrb[] skillOrbSlots = new SkillOrb[2];
    public int currencyCount { get; private set; } = 0;

    public PlayerStats playerStats; // Reference to the player's stats

    public bool AddSkillOrb(SkillOrb newOrb)
    {
        for (int i = 0; i < skillOrbSlots.Length; i++)
        {
            if (skillOrbSlots[i] == null) // If there's an empty slot
            {
                skillOrbSlots[i] = newOrb;
                return true; // Successfully added
            }
        }
        return false; // Inventory full
    }

    public void RemoveSkillOrb(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < skillOrbSlots.Length)
        {
            skillOrbSlots[slotIndex] = null; // Clear the slot
        }
    }

    public void UseOrb(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < skillOrbSlots.Length && skillOrbSlots[slotIndex] != null)
        {
            SkillOrb orb = skillOrbSlots[slotIndex];
            switch (orb.type)
            {
                case SkillOrb.OrbType.AttackBoost:
                    playerStats.IncreaseAttack(orb.boostAmount);
                    break;
                case SkillOrb.OrbType.HealthBoost:
                    playerStats.IncreaseHealth(orb.boostAmount);
                    break;
            }
            RemoveSkillOrb(slotIndex); // Remove the orb after use (optional)
        }
    }

    public void AddCurrency(int amount)
    {
        currencyCount += amount;
    }

    public bool UpgradeStats(PlayerStats.StatType statType, int upgradeCost)
    {
        if (currencyCount >= upgradeCost)
        {
            switch (statType)
            {
                case PlayerStats.StatType.Attack:
                    playerStats.UpgradeAttack();
                    break;
                case PlayerStats.StatType.Health:
                    playerStats.UpgradeHealth();
                    break;
            }
            SpendCurrency(upgradeCost);
            return true; // Successfully upgraded
        }
        return false; // Not enough currency
    }

    public bool SpendCurrency(int amount)
    {
        if (currencyCount >= amount)
        {
            currencyCount -= amount;
            return true; // Successfully spent
        }
        return false; // Not enough currency
    }

    // Suggestion: Implement a UI update mechanism that listens 
    // to inventory changes and updates the inventory UI accordingly.
}
