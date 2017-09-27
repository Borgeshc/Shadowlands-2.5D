using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public static GameObject target;

    GameObject outlinedTarget;
    public LayerMask layermask;
    
    RaycastHit hit;

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, layermask))
        {
            if (hit.collider.tag == "Enemy")
            {
                if (hit.transform.GetComponent<TargetableObject>() != null)
                    hit.transform.GetComponent<TargetableObject>().Targeted();

                outlinedTarget = hit.transform.gameObject;

                if(Input.GetKeyDown(KeyCode.Mouse0))
                {
                    target = hit.transform.gameObject;
                    target.transform.GetComponent<TargetableObject>().Targeted();
                }
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && target)
            {
                target.GetComponent<TargetableObject>().NotTargeted();
                target = null;
            }

            if (!outlinedTarget) return;

            if (outlinedTarget.GetComponent<TargetableObject>() != null && outlinedTarget != target)
                outlinedTarget.GetComponent<TargetableObject>().NotTargeted();

            outlinedTarget = null;
        }
    }
}