using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStat : MonoBehaviour
{
    private bool hadClearLevel1;
    private bool hadClearLevel2;

    private int coin;

    [SerializeField] public IBuff[] inventory = new IBuff[2];

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (inventory[0] != null)
            {
                inventory[0].UseBuff();
                inventory[0] = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inventory[1] != null)
            {
                inventory[1].UseBuff();
                inventory[1] = null;
            }
        }
    }

    public bool AddBuff(IBuff buff)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = buff;
                return true;
            }
        }
        return false;
    }
}