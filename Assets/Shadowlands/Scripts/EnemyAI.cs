using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float damage;
    public float attackFrequencyMin;
    public float attackFrequencyMax;

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
        if (health.isDead) return;
        transform.LookAt(new Vector3(target.transform.position.x, 0, 0));

        if(target != null)
        {
            agent.SetDestination(new Vector3(target.transform.position.x, 0, target.transform.position.z));

            if (Vector3.Distance(transform.position, target.transform.position) > agent.stoppingDistance)
                FollowTarget();
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

        if(attack != null)
            StopCoroutine(attack);
    }
}
