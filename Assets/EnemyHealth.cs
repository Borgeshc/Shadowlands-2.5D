using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;

    [HideInInspector]
    public bool isDead;

    float health;
    Animator anim;
    Rigidbody rb;
    CapsuleCollider collision;

    private void Start()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        collision = GetComponent<CapsuleCollider>();
    }

    public void TookDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        TriggerHit();

        if (health <= 0)
            Died();
    }

    void TriggerHit()
    {
        int randomHit = Random.Range(0, 2);
        switch(randomHit)
        {
            case 0:
                anim.SetTrigger("Hit1");
                break;
            case 1:
                anim.SetTrigger("Hit2");
                break;
            case 2:
                anim.SetTrigger("Hit3");
                break;
        }
    }

    void Died()
    {
        isDead = true;
        rb.useGravity = false;
        collision.enabled = false;
        TriggerDeath();
    }

    void TriggerDeath()
    {
        int randomHit = Random.Range(0, 2);
        switch (randomHit)
        {
            case 0:
                anim.SetBool("Death1", true);
                break;
            case 1:
                anim.SetBool("Death2", true);
                break;
            case 2:
                anim.SetBool("Death3", true);
                break;
        }
    }
}
