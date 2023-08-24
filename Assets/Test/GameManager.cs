using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference
    public int deathSceneIndex;  // The scene index you load when the player dies.

    private GameObject player;
    private HealthSystem playerHealthScript;
    private CharacterStat playerCharacterStatScript;
    private PlayerStat playerStatScript;

    // Variables to store player stats
    private float storedAttackModifier = 1f;
    private float storedMovementSpeedModifier = 1f;
    private int storedCoins;
    private IBuff[] storedInventory = new IBuff[2];
    private bool storedHadClearLevel1;
    private bool storedHadClearLevel2;

    private void Awake()
    {
        // Implement Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fetch references to the player's scripts after scene load
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealthScript = player.GetComponent<HealthSystem>();
            playerCharacterStatScript = player.GetComponent<CharacterStat>();
            playerStatScript = player.GetComponent<PlayerStat>();
        }
    }

    private void Update()
    {
        // Check if the player's health is below or equal to 0
        if (playerHealthScript != null && playerHealthScript.GetHealth() <= 0)
        {
            StartCoroutine(EndGameSequence());
        }
    }

    public void LoadScene(int sceneIndex)
    {
        // Store player stats before transitioning
        if (playerCharacterStatScript != null)
        {
            storedAttackModifier = playerCharacterStatScript.attack_modifier;
            storedMovementSpeedModifier = playerCharacterStatScript.movement_modifier;
        }

        if (playerStatScript != null)
        {
            storedCoins = playerStatScript.GetCoin();
            storedInventory = playerStatScript.inventory;
            storedHadClearLevel1 = playerStatScript.hadClearLevel1;
            storedHadClearLevel2 = playerStatScript.hadClearLevel2;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator EndGameSequence()
    {
        // Here, you can show some UI message to the player or play some animation.
        // For this example, I'll just use a simple Debug.Log.
        Debug.Log("You Died!");

        // Wait for 3 seconds before loading the death scene.
        yield return new WaitForSeconds(3f);

        Destroy(player);
        SceneManager.LoadScene(deathSceneIndex);
    }
}