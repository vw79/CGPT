using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    private float damage;
    [SerializeField] private Collider parentCollider;

    public void SetUpDamage(float dmg)
    {
        damage = dmg;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Ignore every collider from parent
        Collider collider = GetComponent<Collider>();
        Collider[] colliders = parentCollider.GetComponents<Collider>();
        foreach(Collider col in colliders)
        {
            Physics.IgnoreCollision(collider, col);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(ResetHitbox());
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem enemy;
        if(other.TryGetComponent<HealthSystem>(out enemy))
        {
            enemy.TakeDamage(damage);
        }
    }

    private IEnumerator ResetHitbox()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
