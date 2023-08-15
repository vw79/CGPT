using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    private float damage;

    public void SetUpDamage(float dmg)
    {
        damage = dmg;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Ignore self collider
        Collider collider = GetComponent<Collider>();
        Physics.IgnoreCollision(collider, transform.root.GetComponent<Collider>());
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
            Debug.Log(other);
            enemy.TakeDamage(20);
        }
    }

    private IEnumerator ResetHitbox()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
