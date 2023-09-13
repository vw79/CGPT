using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Boss3Trap : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;

    private void Start()
    {
        if (cameraManager)
        {
            cameraManager.SetMinBound(new Vector2(-14.4f, -5.35f));
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Sword"))
        {
            GameManager.instance.isTrapDoorDestroyed = true;
            if (cameraManager)
            {
                cameraManager.SetMinBound(new Vector2(-41.9f, -5.35f));
            }
            Destroy(gameObject);
        }
    }
}
