using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference
    public int deathSceneIndex;  // The scene index you load when the player dies.
    [SerializeField] Animator transitionAnim;

    private GameObject player;
    private bool playerIsDead = false;
    private HealthSystem playerHealthScript;
    private CharacterStat playerCharacterStatScript;
    private PlayerStat playerStatScript;

    // Variables to store player stats
    private float storedAttackDamage = 20f;
    private float storedMovementSpeed = 12f;
    private float storedMaxHealth = 100f;
    private float storedMaxShield = 100f;
    private int storedCoins;
    private SO_Buff[] storedInventory = new SO_Buff[2];
    private bool storedHadClearLevel1;
    private bool storedHadClearLevel2;
    public bool isTrapDoorDestroyed = false;

    public bool PlayerIsDead { get => playerIsDead; set => playerIsDead = value; }

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
        transitionAnim.SetTrigger("Start");
        PlayerIsDead = false;
        // Fetch references to the player's scripts after scene load
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealthScript = player.GetComponent<HealthSystem>();
            playerCharacterStatScript = player.GetComponent<CharacterStat>();
            playerStatScript = player.GetComponent<PlayerStat>();
        }

        LoadPlayerData();

        // Check if we're in scene 1
        if (scene.buildIndex == 2 && GameManager.instance.isTrapDoorDestroyed)
        {
            GameObject trapDoor = GameObject.FindWithTag("TrapDoor");
            if (trapDoor != null)
            {
                Destroy(trapDoor);
            }
        }

        if (scene.buildIndex == 10)
        {
            GameObject player = GameObject.Find("Player");

            GameObject pointLightObject = GameObject.FindWithTag("PointLight");
            if (pointLightObject != null)
            {
                Light lightComponent = pointLightObject.GetComponent<Light>();
                if (lightComponent != null)
                {
                    lightComponent.intensity = 2.4f;
                    lightComponent.range = 6.3f;
                }
            }
            
        }
        else
        {
            GameObject pointLightObject = GameObject.FindWithTag("PointLight");
            if (pointLightObject != null)
            {
                Light lightComponent = pointLightObject.GetComponent<Light>();
                if (lightComponent != null)
                {
                    lightComponent.intensity = 13f;
                    lightComponent.range = 1.02f;
                }
            }
        }

        if (scene.buildIndex == 7 && storedHadClearLevel1 == true)
        {
            GameObject ballSpawn = GameObject.FindWithTag("groundDestroy");

            ballSpawn.SetActive(false);
        }
        else if (scene.buildIndex == 7 && storedHadClearLevel1 == false)
        {
            GameObject ballSpawn = GameObject.FindWithTag("groundDestroy");

            ballSpawn.SetActive(true);
        }

        if (scene.buildIndex == 2 && storedHadClearLevel2 == false)
        {
            GameObject boss3Enterance = GameObject.FindWithTag("Boss3Entrance");

            boss3Enterance.SetActive(false);      
        }
        else if (scene.buildIndex == 2 && storedHadClearLevel2 == true)
        {
            GameObject boss3Enterance = GameObject.FindWithTag("Boss3Entrance");

            boss3Enterance.SetActive(true);
        }
    }

    private void Update()
    {
        // Check if the player's health is below or equal to 0
        if (playerHealthScript != null && playerHealthScript.GetHealth() <= 0 && !PlayerIsDead)
        {
            PlayerIsDead = true;
            StartCoroutine(EndGameSequence());
        }
    }

    public void LoadScene(int sceneIndex)
    {
        StorePlayerData();

        transitionAnim.SetTrigger("End");
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator EndGameSequence()
    {
        // Here, you can show some UI message to the player or play some animation.
        // For this example, I'll just use a simple Debug.Log.
        Debug.Log("You Died!");

        // Wait for 3 seconds before loading the death scene.
        yield return new WaitForSeconds(3f);
        StorePlayerData();
        Destroy(player);
        SceneManager.LoadScene(deathSceneIndex);
    }

    private void StorePlayerData()
    {
        // Store player stats before transitioning
        if (playerCharacterStatScript != null)
        {
            storedAttackDamage = playerCharacterStatScript.GetBaseAttack();
            storedMovementSpeed = playerCharacterStatScript.GetBaseSpeed();
        }

        if (playerStatScript != null)
        {
            storedCoins = playerStatScript.GetCoin();
            storedInventory = playerStatScript.inventory;
            storedHadClearLevel1 = playerStatScript.hadClearLevel1;
            storedHadClearLevel2 = playerStatScript.hadClearLevel2;
        }

        if (playerHealthScript != null)
        {
            storedMaxHealth = playerHealthScript.GetMaxHealth();
            storedMaxShield = playerHealthScript.GetMaxShield();
        }
    }

    private void LoadPlayerData()
    {
          // Load player stats after transitioning
        if (playerCharacterStatScript != null)
        {
            playerCharacterStatScript.SetPlayerStat(storedAttackDamage, storedMovementSpeed);
        }

        if (playerStatScript != null)
        {
            playerStatScript.SetCoin(storedCoins);
            playerStatScript.inventory = storedInventory;
            playerStatScript.hadClearLevel1 = storedHadClearLevel1;
            playerStatScript.hadClearLevel2 = storedHadClearLevel2;
        }

        if (playerHealthScript != null)
        {
            playerHealthScript.SetMaxHealth(storedMaxHealth);
            playerHealthScript.SetMaxShield(storedMaxShield);
        }
    }
}