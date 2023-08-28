using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStat : MonoBehaviour
{
    public bool hadClearLevel1;
    public bool hadClearLevel2;

    private int coin;

    [SerializeField] public SO_Buff[] inventory = new SO_Buff[2];

    #region Buff Inventory
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (inventory[0] != null)
            {
                inventory[0].UseBuff(gameObject);
                if (!inventory[0].isOneTimeUse)
                {
                    StartCoroutine(inventory[0].ResetBuff());
                }
                inventory[0] = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inventory[1] != null)
            {
                inventory[1].UseBuff(gameObject);
                if (!inventory[1].isOneTimeUse)
                {
                    StartCoroutine(inventory[1].ResetBuff());
                }
                inventory[1] = null;
            }
        }
    }

    public bool AddBuffIntoInventory(SO_Buff buff)
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
    #endregion

    #region Coin System
    public int GetCoin()
    {
        return coin;
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public void LoseCoin(int amount)
    {
        coin -= amount;
    }
    #endregion

}