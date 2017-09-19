using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float damage;
    public float attackFrequency;

    NavMeshAgent agent;
    EnemyHealth health;
    GameObject target;
    Animator anim;

    
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
            agent.SetDestination(new Vector3(target.transform.position.x, 0, 0));

            if (Vector3.Distance(transform.position, target.transform.position) > agent.stoppingDistance)
                FollowTarget();
            else
            {
                anim.SetBool("IsIdle", true);
            }
        }
	}

    void FollowTarget()
    {
        anim.SetBool("IsIdle", false);
    }
}
