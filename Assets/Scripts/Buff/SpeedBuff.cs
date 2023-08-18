using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour , IBuff
{
    private CharacterStat playerstat;
    [SerializeField] private Sprite icon;

    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedBuffDuration;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerstat = other.GetComponent<CharacterStat>();
            PlayerStat playerInventory = other.GetComponent<PlayerStat>();
            if (playerInventory.AddBuff(this))
            {
                DisableExistance();
            }
            else
            {
                UseBuff();
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            playerstat = other.GetComponent<CharacterStat>();
            UseBuff();
        }
    }

    public void UseBuff()
    {
        playerstat.SetMovementSpeed(speedMultiplier);
        StartCoroutine(ResetSpeed());

        //Disable every components that let the ball visible
        DisableExistance();
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(speedBuffDuration);
        playerstat.SetMovementSpeed(1/speedMultiplier);
        Destroy(gameObject);
    }

    private void DisableExistance()
    {
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    public Sprite GetIcon()
    {
        return icon;
    }
}
