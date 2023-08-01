using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    //Give health system for the entity which is damagable

    [SerializeField] private float max_health;
    private float current_health;

    private void Awake()
    {
        current_health = max_health;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(40f);
        }
    }

    public float GetHealth()
    {
        return current_health;
    }

    public void TakeDamage(float damage)
    {
        current_health -= damage;
        if(current_health <= 0)
        {
            gameObject.SetActive(false);
        }
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
}
