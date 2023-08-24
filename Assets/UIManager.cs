using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private ShieldUI shieldUI;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private BuffInventoryUI buffInventoryUI;
    [SerializeField] private CoinUI coinUI;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        healthUI.Setup(player);
        shieldUI.Setup(player);
        buffInventoryUI.Setup(player);
        coinUI.Setup(player);
    }

    private void Update()
    {
        healthUI.UpdateHealthUI();
        shieldUI.UpdateShieldUI();
        buffInventoryUI.UpdateInventoryUI();
        coinUI.UpdateCoinUI();
    }
}
