using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Activation_Countdown : MonoBehaviour
{
    private Countdown_Timer countdown_timer;

    private void Start()
    {
        countdown_timer = FindObjectOfType<Countdown_Timer>();
    }

    private void Reset()
    {
        // its box collider's "istrigger" will be automatically set to true.
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("collided");
            countdown_timer.Countdown_Activation();
            
        }
    }
}
