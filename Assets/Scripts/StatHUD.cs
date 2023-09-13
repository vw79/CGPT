using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatHUD : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject HUD;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI speedText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

    private void Update()
    {
        healthText.text = player.GetComponent<HealthSystem>().GetHealth() + " / " + player.GetComponent<HealthSystem>().GetMaxHealth();
        shieldText.text = player.GetComponent<HealthSystem>().GetShield() + " / " + player.GetComponent<HealthSystem>().GetMaxShield();
        attackText.text = player.GetComponent<CharacterStat>().GetAttackDamage().ToString();
        speedText.text = player.GetComponent<CharacterStat>().GetMovementSpeed().ToString();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HUD.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            HUD.SetActive(false);
        }
    }
}
