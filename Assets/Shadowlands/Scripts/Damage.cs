using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;

    BoxCollider collision;

    private void Start()
    {
        collision = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            print("Attacked " + other.name);
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
}
