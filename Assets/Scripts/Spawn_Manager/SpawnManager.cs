using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private GameObject fixedObject;
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnDelay;

    [SerializeField] bool stopSpawning = true;

    private void Start()
    {
        InvokeRepeating("spawning", spawnTime, spawnDelay);
    }

    private void spawning()
    {
        if (!stopSpawning)
        {
            Instantiate(spawnObject, fixedObject.transform.position, fixedObject.transform.rotation);
            stopSpawning = true;
            CancelInvoke("spawning");
        }
            
        if(stopSpawning)
        {
            CancelInvoke("spawning");
        }
    }
}

