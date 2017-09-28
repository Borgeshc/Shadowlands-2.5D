using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float damage;
    public float attackFrequencyMin;
    public float attackFrequencyMax;
    public float lookRadius;

    bool attacking;

    NavMeshAgent agent;
    EnemyHealth health;
    GameObject target;
    Animator anim;
    Coroutine attack;

	private void Start ()
    {
        health = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
	}
	
	void FixedUpdate ()
    {
        if (health.isDead || !target) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= lookRadius)
        {
            FaceTarget();

            if (distance > agent.stoppingDistance)
            {
                FollowTarget();
            }
            else
            {
                anim.SetBool("IsIdle", true);
                if(!attacking)
                {
                    attacking = true;
                    attack = StartCoroutine(Attack());
                }
            }
        }
	}

    void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
    }

    IEnumerator Attack()
    {
        int randomAttack = Random.Range(0, 2);

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
        }

        float attackFrequency = Random.Range(attackFrequencyMin, attackFrequencyMax);
        yield return new WaitForSeconds(attackFrequency);
        attacking = false;
    }

    void FollowTarget()
    {
        anim.SetBool("IsIdle", false);
        agent.SetDestination(new Vector3(target.transform.position.x, 0, target.transform.position.z));

        if (attack != null)
            StopCoroutine(attack);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
