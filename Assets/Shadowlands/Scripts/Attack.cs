using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack Variables")]
    public float attackDistance;
    public float chargeDistance;
    public float chargeSpeed;
    public float damage;

    [Space, Header("Effects")]
    public TrailRenderer weaponTrail;
    public TrailRenderer distortTrail;
    public GameObject hitEffect;

    [Space, Header("Spells")]
    public GameObject fireball;
    public GameObject fireballSpawnPosition;

    public List<GameObject> enemiesInRange = new List<GameObject>();

   // bool attacking;
    bool globalCooldown;
    bool slamming;
    bool charging;

    RFX4_CameraShake cameraShake;
    Movement movement;
    Animator anim;
    Damage damageScript;
    Vector3 gravity;
    Rigidbody rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        damageScript = GetComponentInChildren<Damage>();
        cameraShake = GetComponent<RFX4_CameraShake>();
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if (TargetObject.target)
            {
                float distance = Vector3.Distance(transform.position, TargetObject.target.transform.position + (Vector3.one * 2f));
                if (distance >= attackDistance && distance <= chargeDistance && !charging)
                {
                    charging = true;
                    movement.chargeSpeed = chargeSpeed;
                    StartCoroutine(ResetSpeed());
                }
                else
                {
                    if (!globalCooldown)
                    {
                        globalCooldown = true;
                        StartCoroutine(WeaponAttack());
                    }

                }
            }
        }

        if (Input.GetMouseButton(1) && !globalCooldown)
        {
            globalCooldown = true;
            StartCoroutine(SpellAttack());
        }
    }


    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(.05f);
        rb.velocity = (Vector3.zero);
        movement.chargeSpeed = 0;
        charging = false;
    }

    IEnumerator CameraShake()
    {
        cameraShake.enabled = true;
        yield return new WaitForSeconds(1.5f);
        cameraShake.enabled = false;
    }

    IEnumerator WeaponAttack()
    {
       // Movement.canRotate = false;
       // Movement.canMove = false;
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
        //Movement.canRotate = true;
        // Movement.canMove = true;
        globalCooldown = false;
    }

    IEnumerator SpellAttack()
    {
        // Movement.canRotate = false;
        // Movement.canMove = false;
        //int randomSpell = Random.Range(0, 2);

        //switch (randomSpell)
        //{
        //    case 0:
        //        anim.SetTrigger("Spell1");
        //        break;
        //    case 1:
        //        anim.SetTrigger("Spell2");
        //        break;
        //    case 2:
        //        anim.SetTrigger("Spell3");
        //        break;
        //}

        anim.SetTrigger("Attack8");

        yield return new WaitForSeconds(1f);
        globalCooldown = false;
    }

    public void CastFireball()
    {
        GameObject fireballInstance = Instantiate(fireball, fireballSpawnPosition.transform.position, transform.rotation * Quaternion.Euler(0, -90, 0), transform) as GameObject;
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
        distortTrail.enabled = true;
    }

    public void StopTrail()
    {
        weaponTrail.enabled = false;
        distortTrail.enabled = false;
    }

    public IEnumerator SlamHitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(.3f);
        hitEffect.SetActive(false);
    }
}
