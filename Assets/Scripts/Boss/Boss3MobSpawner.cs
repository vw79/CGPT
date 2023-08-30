using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss3MobSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private float spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnMobs());
    }

    private IEnumerator SpawnMobs()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(mobPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
        this.enabled = false;
    }
}
