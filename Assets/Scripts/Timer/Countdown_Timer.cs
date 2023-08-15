using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown_Timer : MonoBehaviour
{

    float currentTime = 0f;
    [SerializeField] float startingTime = 10f;

    [SerializeField] Text text_countdownTimer;

    private bool isStart = false;

    /* isEnded is used to completely stop the timer, 
     * or else the timer text will keep shaking something like that at the end 0.00 */
    private bool isEnded = false;

    

    void Start()
    {
        text_countdownTimer.enabled = false;

        currentTime = startingTime;

        text_countdownTimer.color = Color.white;
    }


    void Update()
    {
        if (isStart && !isEnded)
        {
            

            currentTime -= 1 * Time.deltaTime;
            text_countdownTimer.text = currentTime.ToString("0.00");

            if (currentTime <= 10)
            {
                text_countdownTimer.color = Color.red;
            }

            if (currentTime <= 0)
            {
                currentTime = 0;
                isEnded = true;     // here as i meantioned above
            }
        }
    }

    public void Countdown_Activation()
    {
        isStart = true;
        text_countdownTimer.enabled = true;
    }
}
