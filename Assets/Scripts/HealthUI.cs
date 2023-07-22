using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject player;
    [SerializeField] private Image fill;
    public Gradient gradient;
    private HealthSystem playerHealth;

    void Start()
    {
        playerHealth = player.GetComponent<HealthSystem>();
    }

    void Update()
    {
        healthBar.value = playerHealth.GetHealth()/100f;
        fill.color = gradient.Evaluate(healthBar.value);
    }
}
