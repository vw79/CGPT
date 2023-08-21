using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneIndex;  // Set the index of the target scene in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSpawnState.NextSpawn = PlayerSpawnState.SpawnType.AtSpawn;
            SceneManager.LoadScene(sceneIndex);
        }
    }
}