using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    [SerializeField] private Slider shieldBar;
    [SerializeField] private GameObject player;
    [SerializeField] private Image fill;
    private HealthSystem playerShield;

    void Start()
    {
        playerShield = player.GetComponent<HealthSystem>();
    }

    void Update()
    {
        shieldBar.value = playerShield.GetShield() / 100f;
    }
}
