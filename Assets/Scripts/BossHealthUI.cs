using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;
    [SerializeField] private GameObject boss;
    private HealthSystem bossHealth;

    private void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    public void Update()
    {
        healthBar.value = bossHealth.GetHealth() / bossHealth.GetMaxHealth();
        fill.color = gradient.Evaluate(healthBar.value);
    }
}
