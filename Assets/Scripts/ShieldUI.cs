using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    [SerializeField] private Slider shieldBar;
    private HealthSystem playerShield;

    public void Setup(GameObject player)
    {
        HealthSystem health = player.GetComponent<HealthSystem>();
        playerShield = health;
    }

    public void UpdateShieldUI()
    {
        shieldBar.value = playerShield.GetShield() / playerShield.GetMaxShield();
    }
}
