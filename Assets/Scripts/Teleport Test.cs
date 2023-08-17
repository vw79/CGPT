using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionSetter : MonoBehaviour
{
    private void Start()
    {
        // If spawn position is set in PlayerPrefs, update player's position
        if (PlayerPrefs.HasKey("SpawnPositionX"))
        {
            float x = PlayerPrefs.GetFloat("SpawnPositionX");
            float y = PlayerPrefs.GetFloat("SpawnPositionY");
            float z = PlayerPrefs.GetFloat("SpawnPositionZ");
            transform.position = new Vector3(x, y, z);
        }
    }
}
