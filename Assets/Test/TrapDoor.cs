using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    // This script will be attached to the Trapdoor object
    private void OnTriggerEnter(Collider collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.gameObject.tag == "Sword")
        {
            Debug.Log("Trapdoor hit by sword");
            // Destroy the trapdoor (this game object)
            Destroy(gameObject);
        }
    }
}


