using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

// PRECONDITION
// trap object must have "boxcollider" property to use this script.
[RequireComponent(typeof(BoxCollider))]

public class TrapObject : MonoBehaviour
{
    // PRECONDITION
    private void Reset()
    {
        // its box collider's "istrigger" will be automatically set to true.
        GetComponent<BoxCollider>().isTrigger = true;
    }

    [SerializeField] private float trap_DamageMade = 50f;
    private HealthSystem playerHealth;


    // FUNCTION
    private void OnTriggerEnter(Collider other)
    {
        // if player touched the trap object, ...
        if(other.tag == "Player")
        {
            Debug.Log($"{name} Triggered");

            playerHealth = other.GetComponent<HealthSystem>();
            playerHealth.TakeDamage(trap_DamageMade);

            // player return back to the checkpoint
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
