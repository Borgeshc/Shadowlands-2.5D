using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed;
    public float rotationSpeed;

    [Space, Header("Jump Variables")]
    public float jumpForce;
    public float distToGround;
    public LayerMask groundLayer;
    public float gravity;

    float horizontal;

    Rigidbody rb;
    Animator anim;

    bool canDoubleJump;
    bool jumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (horizontal == 0 && !anim.GetBool("IsIdle"))
            anim.SetBool("IsIdle", true);
        else if (horizontal != 0 && anim.GetBool("IsIdle"))
            anim.SetBool("IsIdle", false);

        if (horizontal > 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        else if(horizontal < 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,-180,0), rotationSpeed * Time.deltaTime);

        rb.velocity = new Vector3(horizontal * speed * Time.deltaTime, rb.velocity.y, 0);

        if(Input.GetKeyDown(KeyCode.Space))
            Jump();

        if(!jumping && IsGrounded())
        {
            anim.SetBool("DoubleJump", false);
            anim.SetBool("Jump", false);
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            anim.SetBool("Jump", true);
            jumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
        else if (jumping)
        {
            anim.SetBool("DoubleJump", true);
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * 1.2f, 0);
            jumping = false;
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, distToGround, groundLayer);

        if (isGrounded)
        {
            jumping = false;
        }

        return isGrounded;
    }
}
