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
            // Find the GameObject named "Block" and destroy it
            GameObject block = GameObject.Find("Block");
            if (block)
            {
                Destroy(block);
            }

            // Destroy the trapdoor (this game object)
            Destroy(gameObject);
        }
    }
}
