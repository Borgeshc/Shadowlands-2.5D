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
    CharacterController cc;
    RaycastHit hit;

    [HideInInspector]
    public float chargeSpeed;

    bool step;
    
    [HideInInspector]
    public float moveSpeed;                        // The Speed the character will move


    void Start()
    {
        myTransform = transform;                            // sets myTransform to this GameObject.transform
        destinationPosition = myTransform.position;         // prevents myTransform reset
        canMove = true;
        cc = GetComponent<CharacterController>();
        moveSpeed = speed;
    }

    void FixedUpdate()
    {
        // keep track of the distance between this gameObject and destinationPosition
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if (destinationDistance < .5f || Input.GetKey(KeyCode.LeftShift))
        {       // To prevent shakin behavior when near destination
            moveSpeed = 0;
        }
        else if (destinationDistance > .5f)
        {           // To Reset Speed to default
            moveSpeed = speed + chargeSpeed;
        }


        // Moves the Player if the Left Mouse Button was clicked
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

        if(destinationDistance > 1.5f)
        {
            cc.SimpleMove(transform.forward * moveSpeed * Time.deltaTime);
        }
    }
}

