using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class Boss3Entrance : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private GameObject PopUpUI;

    private bool playerInTrigger = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInTrigger)
        {
            SceneManager.LoadScene(10);
        }
      

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerInTrigger = true;
            PopUpUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            PopUpUI.SetActive(false);
        }
    }
}
