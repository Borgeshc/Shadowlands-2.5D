using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator anim;
    Damage damage;

    private void Start()
    {
        anim = GetComponent<Animator>();
        damage = GetComponentInChildren<Damage>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }
    }
    public void EnableCollider()
    {
        damage.EnableCollider();
    }

    public void DisableCollider()
    {
        damage.DisableCollider();
    }
}
