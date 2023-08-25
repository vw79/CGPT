using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;

    // This script will be attached to the Trapdoor object
    private void OnTriggerEnter(Collider collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.gameObject.tag == "Sword")
        {
            cameraManager.SetMinBound(new Vector2(-28.7f, -0.46f));

            // Destroy the trapdoor (this game object)
            Destroy(gameObject);
        }
    }
}
