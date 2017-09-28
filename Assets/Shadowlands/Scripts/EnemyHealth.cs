using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public GameObject hitEffect;

    public Image healthBar;
    public Text regularCombatText;
    public Text criticalCombatText;

    [HideInInspector]
    public bool isDead;

    bool takingDamage;
    float health;
    Animator anim;
    Rigidbody rb;
    CapsuleCollider collision;
    BoxCollider boxCollision;

    private void Start()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        collision = GetComponent<CapsuleCollider>();
        boxCollision = GetComponent<BoxCollider>();
        UpdateHealthBar();
    }

    public void TookDamage(float damage)
    {
        if (isDead) return;

        if(!takingDamage)
        {
            takingDamage = true;
            StartCoroutine(TriggerHit());

            if (CritChance())
            {
                StartCoroutine(FloatingCombatText((damage * 2), criticalCombatText));
                health -= (int)damage * 2;
            }
            else
            {
                StartCoroutine(FloatingCombatText(damage, regularCombatText));
                health -= (int)damage;
            }
        }

        UpdateHealthBar();

        if (health <= 0)
            Died();
    }

    bool CritChance()
    {
        int critRoll = Random.Range(0, 100);
        if (critRoll <= PlayerStats.critChance)
            return true;
        else
            return false;
    }

    IEnumerator TriggerHit()
    {
        hitEffect.SetActive(true);
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

        yield return new WaitForSeconds(.5f);
        hitEffect.SetActive(false);
        takingDamage = false;
    }

    IEnumerator FloatingCombatText(float damagedAmt, Text combatText)
    {
        yield return new WaitForSeconds(.2f);
        combatText.gameObject.SetActive(true);
        combatText.text = ((int)damagedAmt).ToString();

        yield return new WaitForSeconds(.2f);
        criticalCombatText.gameObject.SetActive(false);
        regularCombatText.gameObject.SetActive(false);
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    void Died()
    {
        isDead = true;
        rb.useGravity = false;
        collision.enabled = false;
        boxCollision.enabled = false;
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
