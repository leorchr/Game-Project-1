using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour

{
    private Rigidbody2D rb;
    private Vector3 velocity;

    [Header("Movement")]
    public float speed;
    private Vector2 movementInput;
    private Vector2 smoothedMovementInput;
    private Vector2 movementInputSmoothVelocity;
    public float smoothTime = 0.1f;

    [Header("Jump")]
    public float jumpForce = 2f;
    public float jumpForceHolding = 0.5f;
    private bool isGrounded = false;
    private bool isHolding = false;

    [Header("WallJump")]
    public bool leftHitbox = false;
    public bool rightHitbox = false;
    //public float wallJumpForce;
    //private bool hasJumped;

    [Header("Electricity")]
    [SerializeField]
    private GameObject electric;
    [HideInInspector]
    public bool ElectricityActive;
    private float lastPressed;
    public float interationCooldown = 1f;
    public float interationDuration = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        electric.SetActive(false);
        ElectricityActive = false;
    }
    private void FixedUpdate()
    {
        SmoothedMovement();
        AddJumpForce();
        WallJump();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movementInput.x = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!leftHitbox && !rightHitbox)
            {
                if (rb.velocity.y == 0 && isGrounded)
                {
                    isHolding = true;
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }
            }
            else
            {
                if (leftHitbox)
                {
                    /*if (hasJumped)
                    {
                        rb.velocity = new Vector2(smoothedMovementInput.x * speed + wallJumpForce, jumpForce);
                        hasJumped = false;
                    }*/
                    //rb.gravityScale = 8;
                    //rb.AddForce(new Vector2(wallJumpForce, jumpForce), ForceMode2D.Impulse);
                }
                else
                {
                    //rb.gravityScale = 8;
                    //rb.velocity = new Vector2(smoothedMovementInput.x * speed + wallJumpForce, jumpForce);

                    //rb.AddForce(new Vector2(wallJumpForce, jumpForce), ForceMode2D.Impulse);
                }
            }
        }

        if(context.canceled && !context.started)
        {
            isHolding = false;
        }
        if (context.performed && !context.started)
        {
            isHolding = false;
        }
    }

    private void AddJumpForce()     // let the player jump higher if is holding the jump button
    {
        if (isHolding)
        {
            rb.AddForce(new Vector2(0, jumpForceHolding), ForceMode2D.Impulse);
        }
    }

    public void Electric(InputAction.CallbackContext context)           // interaction
    {
        if (context.started && lastPressed + interationCooldown <= Time.time)
        {
            electric.SetActive(true);
            ElectricityActive = true;
            Invoke("StopElectricity", interationDuration);
        }
        else if(Time.time < interationCooldown)
        {
            electric.SetActive(true);
            ElectricityActive = false;
            Invoke("StopElectricity", interationDuration);
        }
    }

    private void StopElectricity()
    {
        electric.SetActive(false);
        ElectricityActive = false;
        lastPressed = Time.time;
    }

    private void SmoothedMovement()
    {
        smoothedMovementInput = Vector2.SmoothDamp(
            smoothedMovementInput,
            movementInput,
            ref movementInputSmoothVelocity,

            smoothTime);

        if(!leftHitbox && !rightHitbox)
        {
            rb.velocity = new Vector2(smoothedMovementInput.x * speed, rb.velocity.y);
        }

        //velocity = rb.velocity;
        //velocity.x = smoothedMovementInput.x * speed;
        //rb.velocity = velocity;

    }

    private void WallJump()
    {
        if(leftHitbox || rightHitbox)
        {
            //rb.gravityScale = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}