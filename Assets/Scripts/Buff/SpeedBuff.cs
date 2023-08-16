using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    private CharacterStat playerstat;

    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedBuffDuration;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerstat = other.GetComponent<CharacterStat>();
            playerstat.SetMovementSpeed(speedMultiplier);
            StartCoroutine(ResetSpeed());

            //Disable every components that let the ball visible
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(speedBuffDuration);
        playerstat.SetMovementSpeed(1/speedMultiplier);
        Destroy(gameObject);
    }
}
