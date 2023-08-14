using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// PRECONDITION
// trap object must have "boxcollider" property to use this script.
[RequireComponent(typeof(BoxCollider))]

public class TrapObject_2: MonoBehaviour
{
    // PRECONDITION
    private void Reset()
    {
        // its box collider's "istrigger" will be automatically set to true.
        GetComponent<BoxCollider>().isTrigger = true;
    }

    [SerializeField] private float trap_DamageMade = 50f;
    private HealthSystem playerHealth;

[SerializeField] bool groundDestroy = true;



// FUNCTION
private void OnTriggerEnter(Collider other)
{
    // Check if the other collider is not a trigger
    if (!other.isTrigger)
    {
        // Prevent the other collider from passing through
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    // if player touched the trap object, ...
    if (other.tag == "Player")
    {
        Debug.Log($"{name} Triggered");

        playerHealth = other.GetComponent<HealthSystem>();
        playerHealth.TakeDamage(trap_DamageMade);

        // player return back to the checkpoint
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    if (other.tag == "groundDestroy" && groundDestroy == true)
    {
        Destroy(gameObject);
    }
}
}
