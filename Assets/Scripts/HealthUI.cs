using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;
    private HealthSystem playerHealth;

    public void Setup(GameObject player)
    {
        HealthSystem health = player.GetComponent<HealthSystem>();
        playerHealth = health;
    }

    public void UpdateHealthUI()
    {
        healthBar.value = playerHealth.GetHealth() / playerHealth.GetMaxHealth();
        fill.color = gradient.Evaluate(healthBar.value);
    }
}
