using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public static bool canMove;
    public float speed;
    public float rotationSpeed;
    public LayerMask layermask;


    private Transform myTransform;              // this transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition
   // CharacterController cc;
    Rigidbody rb;
    RaycastHit hit;
    Animator anim;

    [HideInInspector]
    public float chargeSpeed;

    bool step;
    
    [HideInInspector]
    public float moveSpeed;                        


    void Start()
    {
        myTransform = transform;                            
        destinationPosition = myTransform.position;         
        canMove = true;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        moveSpeed = speed;
    }

    void FixedUpdate()
    {
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if (destinationDistance < .5f || Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = Vector3.zero;
            moveSpeed = 0;
        }
        else if (destinationDistance > .5f)
        {           
            moveSpeed = speed + chargeSpeed;
        }

        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            Plane playerPlane = new Plane(Vector3.up, myTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, layermask))
                {

                    if (hit.transform.tag == "Item")
                    {
                        return;
                    }
                    if (hit.transform.tag == "Enemy")
                    {

                        Vector3 tarPos = ray.GetPoint(hitdist);
                        destinationPosition = hit.point;
                        Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);
                        //	myTransform.rotation = Quaternion.Slerp (myTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                        myTransform.rotation = tarRot;
                    }
                }
                else
                {

                    Vector3 targetPoint = ray.GetPoint(hitdist);
                    destinationPosition = ray.GetPoint(hitdist);
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                    //	myTransform.rotation = Quaternion.Slerp (myTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    myTransform.rotation = targetRotation;
                }
            }
        }
        //// Moves the player if the mouse button is hold down
        else if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
        {
            Plane playerPlane = new Plane(Vector3.up, myTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                //myTransform.rotation = Quaternion.Slerp (myTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                myTransform.rotation = targetRotation;
            }
        }

        if(destinationDistance > 2f)
        {
            anim.SetBool("IsIdle", false);
            rb.velocity = (transform.forward * moveSpeed * Time.deltaTime);
        }
        else
            anim.SetBool("IsIdle", true);
    }
}

