using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2OpenBack : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OpenGate()
    {
        gate.SetActive(true);
        player.GetComponent<PlayerStat>().hadClearLevel2 = true;
    }
}
