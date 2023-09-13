using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3TriggerWin : MonoBehaviour
{
    [SerializeField] private GameObject winCube;

    public void UnlockCube()
    {
        winCube.SetActive(true);
    }
}
