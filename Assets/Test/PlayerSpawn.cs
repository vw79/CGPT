using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public Transform spawnPoint;  // Set this to Point A/Spawn in each scene

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
        if (PlayerSpawnState.NextSpawn == PlayerSpawnState.SpawnType.AtSpawn)
        {
            // Find the "Spawn" prefab in the current scene and set the player's position to its location
            Transform spawnPoint = GameObject.FindWithTag("Spawn").transform;
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.position;
            }
        }
    }
}



