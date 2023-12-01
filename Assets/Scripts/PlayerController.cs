using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;
    private Rigidbody2D rb;
    private CapsuleCollider2D cCollider;
    private Vector2 inputAxis;
    private Vector2 velocity;

    private bool isGrounded;
    private float timeSinceJumpPressed;
    private float timeSinceLeftGround;
    private bool canJump;
    private bool canCoyoteJump;
    private bool jumpCut;

    public event Action<bool> OnGroundedChange;
    public event Action OnJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        CheckGroundStatus();
        ApplyJump();
        ApplyMovement();
        ApplyGravity();
    }

    void ProcessInput()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            canJump = true;
            timeSinceJumpPressed = Time.time;
        }
    }

    void CheckGroundStatus()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.CapsuleCast(cCollider.bounds.center,cCollider.size,
            cCollider.direction,0,Vector2.down,stats.GroundDistance,stats.PlayerLayer);

        if (isGrounded != wasGrounded)
        {
            OnGroundedChange?.Invoke(isGrounded);
            if (isGrounded)
            {
                canCoyoteJump = true;
                jumpCut = false;
            }
        }
        else
        {
            timeSinceLeftGround = Time.time;
        }
    }

    void ApplyMovement()
    {
        float targetSpeed = inputAxis.x * stats.maxSpeed;
        rb.velocity = new Vector2(targetSpeed,rb.velocity.y);
    }

    void ApplyJump()
    {
        if ((canJump || (canCoyoteJump && !isGrounded)
            && (Time.time < timeSinceLeftGround + stats.coyoteTime))
            && isGrounded)
        {
            velocity.y = stats.jumpPower;
            rb.velocity = new Vector2(rb.velocity.x, velocity.y);
            canJump = false;
            canCoyoteJump = false;
            jumpCut = true;
            OnJump?.Invoke();
        }
        else if (!Input.GetButton("Jump") &&
            rb.velocity.y>0 && jumpCut)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * stats.jumpMultiplier);
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            float gravityForce =
                (rb.velocity.y > 0 && !Input.GetButton("Jump")) ?
                      stats.fallAccleration : stats.inAirAcceleration;

            velocity.y = Mathf.MoveTowards(rb.velocity.y,
                -stats.MaxFallSpeed,
                gravityForce * Time.fixedDeltaTime);

            rb.velocity = new Vector2(rb.velocity.x,velocity.y);
        }
    }

    [Serializable]
    public class PlayerStats {
        public float maxSpeed = 10.0f;
        public float jumpPower = 15.0f;
        public float acceleration = 30f;
        public float groundDeceleration = 20f;
        public float airDeceleration = 5f;
        public float fallAccleration = 30f;
        public float inAirAcceleration = 15f;
        public float MaxFallSpeed = 20f;
        public float GroundForce = -0.5f;
        public float GroundDistance = 0.1f;
        public float coyoteTime = 0.2f;
        public float jumpBuffer = 0.1f;
        public float jumpMultiplier = 0.5f;
        public LayerMask PlayerLayer;
        public float HorizontalTreshold = 0.1f;
        public float VerticalTreshold = 0.1f;
    }
}
