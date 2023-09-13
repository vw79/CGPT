using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class HealthSystem : MonoBehaviour
{
    //Give health system for the entity which is damagable

    [SerializeField] private float max_health = 100f;
    [SerializeField] private float max_shield = 100f;
    private float current_health;
    private float current_shield;

    public UnityEvent OnDeath;
    public UnityEvent OnHurt;

    private void Awake()
    {
        current_health = max_health;
        current_shield = 0f;
    }

    private void Update()
    {

    }

    public float GetHealth()
    {
        return current_health;
    }

    public float GetShield()
    {
        return current_shield;
    }

    public float GetMaxHealth()
    {
        return max_health;
    }

    public float GetMaxShield()
    {
          return max_shield;
    }

    public void TakeDamage(float damage)
    {
        // If shield can block whole damage
        if(current_shield >= damage)
        {
            current_shield -= damage;
        }
        // If shield can't block damage or shield is 0
        else
        {
            current_health -= damage - current_shield;
            current_shield = 0;
        }

        if (current_health <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // (xiu zhen added) return back to checkpoint
            //cannot do like this. this health system is for every entity
            OnDeath.Invoke();
            //Show End Scene when it die
            //SceneManager.LoadScene("EndScene");
        }
        else
        {
            OnHurt.Invoke();
        }
    }

    public void AddShield(float shieldPoint)
    {
        current_shield += shieldPoint;
    }

    public void Heal(float damage)
    {
        current_health += damage;
        if(current_health > max_health)
        {
            current_health = max_health;
        }
    }

    public void GradHeal(float hp, int seconds)
    {
        float hps = hp / seconds;
        Heal(hps);
        StartCoroutine(Regen(hps,seconds - 1));
    }

    public IEnumerator Regen(float hps, int seconds)
    {
        for(int i = 0; i < seconds; i++)
        {
            yield return new WaitForSeconds(1);
            Heal(hps);
        }
    }

    public void AddMaxHealth(float amount)
    {
        max_health += amount;
    }

    public void AddMaxShield(float amount)
    {
        max_shield += amount;
    }
}
