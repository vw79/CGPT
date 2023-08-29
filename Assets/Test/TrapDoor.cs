using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;

    private void Start()
    {
        if (cameraManager)
        {
            cameraManager.SetMinBound(new Vector2(-2f, -0.46f));
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Sword"))
        {
            GameManager.instance.isTrapDoorDestroyed = true;
            if (cameraManager)
            {
                cameraManager.SetMinBound(new Vector2(-28.7f, -0.46f));
            }
            Destroy(gameObject);
        }
    }
}
