using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffInventoryUI : MonoBehaviour
{
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    private Image[] images;
    private PlayerStat inventory;


    public void Setup(GameObject player)
    {
        images = new Image[] { image1, image2 };
        inventory = player.GetComponent<PlayerStat>();
    }

    public void UpdateInventoryUI()
    {
        SO_Buff[] buffs = inventory.inventory;
        for (int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i] != null)
            {
                images[i].sprite = buffs[i].GetIcon();
                images[i].color = new Color(255, 255, 255, 1);
            }
            else
            {
                images[i].sprite = null;
                images[i].color = new Color(255, 255, 255, 0);
            }
        }
    }
}
