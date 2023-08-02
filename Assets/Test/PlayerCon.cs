using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public CharacterController controller;
    private PlayerStateMachine playerStateMachine;
    private Animator animator;

    public float speed = 12f;

    public float gravity = -30f;
    private float verticalVelocity;
    public float terminalVelocity = -50f;

    private int jumpCount = 0;
    public int maxJumpCount = 2;

    public float firstJumpPower = 2f;
    public float secondJumpPower = 1.5f;

    public float dashSpeed = 50f; // Speed of the dash
    public float dashTime = 0.2f; // Duration of the dash
    private float dashEndTime; // Time when the current dash will end
    public float dashCooldown = 2f; // Time player has to wait after a dash to dash again
    private float nextDashTime; // Time when the player can next dash
    private bool isDashing = false;// To check if the player is currently dashing

    public bool IsAttacking { get; private set; } = false;
    

    public int comboC = 0; // To keep track of combo attacks
    private float comboResetTime = 0.8f; // Time after which the combo resets if no subsequent attack is made
    private float lastAttackTime; // Time the last attack was made

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input and move player
        MovePlayer();

        // Apply gravity
        ApplyGravity();

        HandleAttack();

        // Handle jumping
        HandleJump();

        HandleDash();
    }

    private void MovePlayer()
    {
        bool isJumpPressed = Input.GetButtonDown("Jump");

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 direction = new Vector3(-1, 0f, 0f);
            RotatePlayer(direction);
            controller.Move(direction * speed * Time.deltaTime);
            if (isJumpPressed)
                playerStateMachine.ForwardJump();
            else
                playerStateMachine.Running(); // Change state to running
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 direction = new Vector3(1, 0f, 0f);
            RotatePlayer(direction);
            controller.Move(direction * speed * Time.deltaTime);
            if (isJumpPressed)
                playerStateMachine.ForwardJump();
            else
                playerStateMachine.Running(); // Change state to running
        }
        else
        {
            if (isJumpPressed)
                playerStateMachine.Jump();
            else
                playerStateMachine.Idle(); // Change state to idle
        }
    }

    private void RotatePlayer(Vector3 direction)
    {
        if (IsAttacking) return; // Don't rotate if player is attacking

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }


    private void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            verticalVelocity = Mathf.Clamp(verticalVelocity, terminalVelocity, float.MaxValue);
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            jumpCount = 0;
        }

        Vector3 gravityVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(gravityVector * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            jumpCount++;

            if (jumpCount == 1)
            {
                verticalVelocity = Mathf.Sqrt(firstJumpPower * -2f * gravity);
                playerStateMachine.Jump(); // Change state to jumping for the first jump
            }
            else if (jumpCount == 2)
            {
                verticalVelocity = Mathf.Sqrt(secondJumpPower * -2f * gravity);
                playerStateMachine.Jump2(); // Change state to Jump2 for the second jump
            }
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // If enough time has passed since the last attack, reset combo
            if (Time.time - lastAttackTime > comboResetTime)
            {
                comboC = 0;
                Debug.Log("hi");
            }

            if (comboC < 3) // Assuming max 3 combo attacks
            {
                comboC++;
            }

            //Debug.Log("Combo: " + comboC);

            playerStateMachine.Attack(comboC);
            lastAttackTime = Time.time;

            // Set IsAttacking to true when player starts attacking
            IsAttacking = true;


            // Get the duration of the current attack animation
            float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(ResetAttackStatus(animationDuration));
        }
    }

    private IEnumerator ResetAttackStatus(float delay)
    {
        yield return new WaitForSeconds(delay);

        IsAttacking = false;
        Debug.Log("Attack ended");
    }



    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.time > nextDashTime)
        {
            StartDash();
        }

        if (isDashing)
        {
            if (Time.time > dashEndTime)
            {
                EndDash();
            }
            else
            {
                DashMovement();
            }
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashTime;
        nextDashTime = Time.time + dashCooldown;
    }

    private void DashMovement()
    {
        // Here, I'm assuming you dash in the direction the player is currently moving.
        // Adjust as needed based on your game's mechanics.
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 dashDirection = new Vector3(x, 0f, 0f).normalized;

        controller.Move(dashDirection * dashSpeed * Time.deltaTime);
    }

    private void EndDash()
    {
        isDashing = false;
    }

}
