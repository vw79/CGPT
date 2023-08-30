using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private CoinUI coinUI;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        coinUI.Setup(player);
    }

    private void Update()
    {
        coinUI.UpdateCoinUI();
    }


}
