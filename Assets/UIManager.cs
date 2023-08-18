using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private ShieldUI shieldUI;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private BuffInventoryUI buffInventoryUI;


    private void Start()
    {
        healthUI.Setup(player);
        shieldUI.Setup(player);
        buffInventoryUI.Setup(player);
    }

    private void Update()
    {
        healthUI.UpdateHealthUI();
        shieldUI.UpdateShieldUI();
        buffInventoryUI.UpdateInventoryUI();
    }
}
