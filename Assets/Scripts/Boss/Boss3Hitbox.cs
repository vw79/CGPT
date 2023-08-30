using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Hitbox : MonoBehaviour, IHitbox
{
    private float damage;
    private float hitboxDuration;
    [SerializeField] private Collider parentCollider;

    public void SetUpDamage(float dmg, float duration)
    {
        damage = dmg;
        hitboxDuration = duration;
    }

    void Start()
    {
        //Ignore every collider from parent
        Collider collider = GetComponent<Collider>();
        Collider[] colliders = parentCollider.GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            Physics.IgnoreCollision(collider, col);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ResetHitbox());
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem enemy;
        if (other.TryGetComponent<HealthSystem>(out enemy))
        {
            enemy.TakeDamage(damage + enemy.GetShield());
        }
    }

    private IEnumerator ResetHitbox()
    {
        yield return new WaitForSeconds(hitboxDuration);
        gameObject.SetActive(false);
    }
}
