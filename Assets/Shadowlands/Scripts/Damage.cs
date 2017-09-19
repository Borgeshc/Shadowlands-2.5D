using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;
    public TrailRenderer trail;

    BoxCollider collision;

    private void Start()
    {
        collision = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TookDamage(damage);
        }
    }

    public void EnableCollider()
    {
        collision.enabled = true;
    }

    public void DisableCollider()
    {
        collision.enabled = false;
    }
    
    public void StartTrail()
    {
        trail.enabled = true;
    }

    public void StopTrail()
    {
        trail.enabled = false;
    }
}
