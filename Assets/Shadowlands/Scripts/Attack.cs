using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public TrailRenderer weaponTrail;
    public GameObject fireball;
    public GameObject fireballSpawnPosition;

    public List<GameObject> enemiesInRange = new List<GameObject>();
    Animator anim;
    Damage damageScript;
    bool attacking;
    bool casting;

    private void Start()
    {
        anim = GetComponent<Animator>();
        damageScript = GetComponentInChildren<Damage>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && !attacking)
        {
            attacking = true;
            StartCoroutine(WeaponAttack());
        }

        if (Input.GetMouseButtonDown(1) && !casting)
        {
            casting = true;
            StartCoroutine(SpellAttack());
        }
    }

    IEnumerator WeaponAttack()
    {
        int randomAttack = Random.Range(0, 7);

        switch(randomAttack)
        {
            case 0:
                anim.SetTrigger("Attack1");
                break;
            case 1:
                anim.SetTrigger("Attack2");
                break;
            case 2:
                anim.SetTrigger("Attack3");
                break;
            case 3:
                anim.SetTrigger("Attack4");
                break;
            case 4:
                anim.SetTrigger("Attack5");
                break;
            case 5:
                anim.SetTrigger("Attack6");
                break;
            case 6:
                anim.SetTrigger("Attack7");
                break;
            case 7:
                anim.SetTrigger("Attack8");
                break;
        }

        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    IEnumerator SpellAttack()
    {
        Movement.canMove = false;
        int randomSpell = Random.Range(0, 2);

        switch (randomSpell)
        {
            case 0:
                anim.SetTrigger("Spell1");
                break;
            case 1:
                anim.SetTrigger("Spell2");
                break;
            case 2:
                anim.SetTrigger("Spell3");
                break;
        }

        yield return new WaitForSeconds(.5f);
        Movement.canMove = true;
        yield return new WaitForSeconds(.5f);
        casting = false;
    }

    public void CastFireball()
    {
        GameObject fireballInstance = Instantiate(fireball, fireballSpawnPosition.transform.position, transform.rotation, transform) as GameObject;
        fireballInstance.transform.parent = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy") && !enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Enemy") && enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public void DealDamage()
    {
        if (enemiesInRange.Count <= 0) return;

        for(int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i].GetComponent<EnemyHealth>() != null)
            {
                EnemyHealth health = enemiesInRange[i].GetComponent<EnemyHealth>();
                if (!health.isDead)
                {
                    health.TookDamage(damage);
                }
                else
                    enemiesInRange.Remove(enemiesInRange[i]);
            }
            else
                enemiesInRange.Remove(enemiesInRange[i]);
        }
    }

    public void StartTrail()
    {
        weaponTrail.enabled = true;
    }

    public void StopTrail()
    {
        weaponTrail.enabled = false;
    }
}
