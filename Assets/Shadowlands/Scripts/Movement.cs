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

    [Space, Header("Dodge Variables")]
    public float rollSpeed;
    public float dodgeCooldownTimer;
    public Collider collision;

    float horizontal;

    Rigidbody rb;
    Animator anim;

    float D_ButtonTimer = 1f;
    float D_ButtonCount;

    float A_ButtonTimer = 1f;
    float A_ButtonCount;

    bool roll;
    bool waitForRoll;
    bool isRolling;
    bool rollingRight;
    bool dodgeCooldown;

    bool canDoubleJump;
    bool jumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        print(IsGrounded());

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.D) && !dodgeCooldown)
        {
            if (D_ButtonTimer > 0 && D_ButtonCount == 1)
            {
                if (IsGrounded())
                    anim.SetTrigger("Roll");
                else
                    anim.SetTrigger("AirDodge");

                rb.useGravity = false;
                collision.enabled = false;

                if (!isRolling)
                {
                    isRolling = true;
                    rollingRight = true;
                    roll = true;
                    StartCoroutine(Rolling());
                }
            }
            else
            {
                D_ButtonTimer = .5f;
                D_ButtonCount += 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !dodgeCooldown)
        {
            if (A_ButtonTimer > 0 && A_ButtonCount == 1)
            {
                if (IsGrounded())
                    anim.SetTrigger("Roll");
                else
                    anim.SetTrigger("AirDodge");

                rb.useGravity = false;
                collision.enabled = false;

                if (!isRolling)
                {
                    isRolling = true;
                    rollingRight = false;
                    roll = true;
                    StartCoroutine(Rolling());
                }
            }
            else
            {
                A_ButtonTimer = .5f;
                A_ButtonCount += 1;
            }
        }
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

        if (roll && rollingRight)
        {
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            rb.velocity = (Vector3.right * rollSpeed);
        }
        else if (roll && !rollingRight)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
            rb.velocity = Vector3.zero;
            rb.velocity = (Vector3.left * rollSpeed);
        }

        if (D_ButtonTimer > 0)
            D_ButtonTimer -= 1 * Time.deltaTime;
        else
        {
            D_ButtonCount = 0;
        }

        if (A_ButtonTimer > 0)
            A_ButtonTimer -= 1 * Time.deltaTime;
        else
        {
            A_ButtonCount = 0;
        }

        if (!jumping && IsGrounded())
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

    IEnumerator Rolling()
    {
        yield return new WaitForSeconds(.5f);
        rb.useGravity = true;
        collision.enabled = true;
        isRolling = false;
        roll = false;

        if(!dodgeCooldown)
        {
            dodgeCooldown = true;
            StartCoroutine(DodgeCooldown());
        }
    }

    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(dodgeCooldownTimer);
        dodgeCooldown = false;
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
