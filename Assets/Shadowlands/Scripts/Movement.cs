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
    public TrailRenderer[] bodyDistortTrail;

    public static bool canMove;
    public static bool canRotate;
    public static bool isJumping;

    float horizontal;
    float vertical;

    Rigidbody rb;
    Animator anim;
    Vector3 targetRotation;
    Vector3 movement;

    float D_ButtonTimer = 1f;
    float D_ButtonCount;

    float A_ButtonTimer = 1f;
    float A_ButtonCount;

    float W_ButtonTimer = 1f;
    float W_ButtonCount;

    float S_ButtonTimer = 1f;
    float S_ButtonCount;

    bool roll;
    bool waitForRoll;
    bool isRolling;
    bool rollingRight;
    bool rollingForward;
    bool rollingLeft;
    bool rollingBack;
    bool dodgeCooldown;
    bool isGrounded;
    bool canDoubleJump;
    bool jumping;

    private void Start()
    {
        canMove = true;
        canRotate = true;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        isGrounded = IsGrounded();

        if(isJumping == true && isGrounded)
        {
            anim.SetBool("DoubleJump", false);
            anim.SetBool("Jump", false);
        }

        if (IsGrounded())
        {
            jumping = false;
            isJumping = false;
            canDoubleJump = true;
        }
        else
            isJumping = true;

        if (Input.GetKeyDown(KeyCode.D) && !dodgeCooldown)
        {
            if (D_ButtonTimer > 0 && D_ButtonCount == 1)
            {
                if (isGrounded)
                    anim.SetTrigger("Roll");
                else
                    anim.SetTrigger("AirDodge");

                rb.useGravity = false;

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
                if (isGrounded)
                    anim.SetTrigger("Roll");
                else
                    anim.SetTrigger("AirDodge");

                rb.useGravity = false;

                if (!isRolling)
                {
                    isRolling = true;
                    rollingLeft = true;
                    StartCoroutine(Rolling());
                }
            }
            else
            {
                A_ButtonTimer = .5f;
                A_ButtonCount += 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && !dodgeCooldown)
        {
            if (W_ButtonTimer > 0 && W_ButtonCount == 1)
            {
                if (isGrounded)
                    anim.SetTrigger("Roll");
                else
                    anim.SetTrigger("AirDodge");

                rb.useGravity = false;

                if (!isRolling)
                {
                    isRolling = true;
                    rollingForward = true;
                    StartCoroutine(Rolling());
                }
            }
            else
            {
                W_ButtonTimer = .5f;
                W_ButtonCount += 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && !dodgeCooldown)
        {
            if (S_ButtonTimer > 0 && S_ButtonCount == 1)
            {
                if (isGrounded)
                    anim.SetTrigger("Roll");
                else
                    anim.SetTrigger("AirDodge");

                rb.useGravity = false;

                if (!isRolling)
                {
                    isRolling = true;
                    rollingBack = true;
                    roll = true;
                    StartCoroutine(Rolling());
                }
            }
            else
            {
                S_ButtonTimer = .5f;
                S_ButtonCount += 1;
            }
        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0, vertical);

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        if (movement != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(movement).eulerAngles;
            anim.SetBool("IsIdle", false);
        }
        else
        {
            anim.SetBool("IsIdle", true);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x,
                     Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * rotationSpeed);

        if (!canMove) return;

        rb.velocity = new Vector3(movement.normalized.x * speed * Time.deltaTime, rb.velocity.y, movement.normalized.z * speed * Time.deltaTime);

        if (roll && rollingRight)
        {
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            rb.velocity = (Vector3.right * rollSpeed);
        }
        else if (roll && rollingLeft)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
            rb.velocity = Vector3.zero;
            rb.velocity = (Vector3.left * rollSpeed);
        }
        else if (roll && rollingForward)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            rb.velocity = Vector3.zero;
            rb.velocity = (Vector3.forward * rollSpeed);
        }
        else if (roll && rollingBack)
        {
            transform.rotation = Quaternion.Euler(0, -270, 0);
            rb.velocity = Vector3.zero;
            rb.velocity = (Vector3.back * rollSpeed);
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

        if (W_ButtonTimer > 0)
            W_ButtonTimer -= 1 * Time.deltaTime;
        else
        {
            W_ButtonCount = 0;
        }

        if (S_ButtonTimer > 0)
            S_ButtonTimer -= 1 * Time.deltaTime;
        else
        {
            S_ButtonCount = 0;
        }
    }

    void Jump()
    {
        anim.SetBool("Jump", true);

        if (isGrounded)
        {
            jumping = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            canDoubleJump = true;
        }
        else if (!isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
            anim.SetBool("DoubleJump", true);
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * 1.5f, 0);
            jumping = false;
        }
    }

    IEnumerator Rolling()
    {
        for(int i = 0; i < bodyDistortTrail.Length; i++)
            bodyDistortTrail[i].enabled = true;

        yield return new WaitForSeconds(.1f);
        roll = true;
        yield return new WaitForSeconds(.5f);
        rb.useGravity = true;
        isRolling = false;
        roll = false;

        rollingBack = false;
        rollingForward = false;
        rollingLeft = false;
        rollingRight = false;

        for (int i = 0; i < bodyDistortTrail.Length; i++)
            bodyDistortTrail[i].enabled = false;

        if (!dodgeCooldown)
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

        return isGrounded;
    }
}
