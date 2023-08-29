using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevel2Detection : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    private PlayerStat player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
    }

    private void Update()
    {
        if (player.hadClearLevel1)
        {
            meshRenderer.enabled = true;
            GetComponentInChildren<Collider>().excludeLayers = LayerMask.GetMask("Pickables");
        }
        else
        {
            meshRenderer.enabled = false;
            GetComponentInChildren<Collider>().excludeLayers = LayerMask.GetMask("Player");
        }
    }
}
