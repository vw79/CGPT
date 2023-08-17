using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetScene; // Name of the scene to load
    public Vector3 spawnPosition; // Position to spawn the player in the target scene

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Store the spawn position in PlayerPrefs to retrieve it in the next scene
            PlayerPrefs.SetFloat("SpawnPositionX", spawnPosition.x);
            PlayerPrefs.SetFloat("SpawnPositionY", spawnPosition.y);
            PlayerPrefs.SetFloat("SpawnPositionZ", spawnPosition.z);
            SceneManager.LoadScene(targetScene);
        }
    }
}
