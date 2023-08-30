using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    private PlayerStat coin;
    [SerializeField] private TextMeshProUGUI coinText;

    public void Setup(GameObject player)
    {
        coin = player.GetComponent<PlayerStat>();
    }

    public void UpdateCoinUI()
    {
        coinText.text = coin.GetCoin().ToString();
    }
}
