using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    public CharacterController controller;
    private PlayerStateMachine playerStateMachine;
    public HealthSystem healthSystem;
    public GameObject hitbox;

    private float lastDirection = 1f; // 1 for right, -1 for left
    public float speed = 12f;

    public float gravity = -30f;
    private float verticalVelocity;
    public float terminalVelocity = -50f;

    private int jumpCount = 0;
    public int maxJumpCount = 2;

    public float firstJumpPower = 2f;
    public float secondJumpPower = 1.5f;

    public float damage = 20f;

    public float dashSpeed = 50f; // Speed of the dash
    public float dashTime = 0.2f; // Duration of the dash
    private float dashEndTime; // Time when the current dash will end
    public float dashCooldown = 2f; // Time player has to wait after a dash to dash again
    private float nextDashTime; // Time when the player can next dash
    private bool isDashing = false; // To check if the player is currently dashing

    private Queue<int> attackQueue = new Queue<int>(); // To store the attack sequence
    public float attackRate = 3f; // Time player has to wait after an attack to attack again
    private float nextAttackTime; // Time when the player can next attack
    private bool isAttacking = false; // To check if the player is currently attacking
    private int attackComboCount = 0; // To track the combo sequence
    public float comboResetTime = 2f; // Time after which the combo sequence resets
    private float lastAttackTime; // Time of the last attack

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Update()
    {
        ApplyGravity();
        MovePlayer();
        HandleJump();
        HandleDash();
        HandleAttack();
        CheckDeath();
        hitbox.GetComponent<HitboxManager>().SetUpDamage(damage);
    }

    private void MovePlayer()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(x, 0f, 0f);
        RotatePlayer(direction);

        controller.Move(direction * speed * Time.deltaTime);

        if (direction.x != 0)
        {
            ResetComboCounter();
            if (Input.GetButtonDown("Jump"))
                playerStateMachine.ForwardJump();
            else
                playerStateMachine.Running();
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
                playerStateMachine.Jump();
            else
                playerStateMachine.Idle();
        }
    }

    private void RotatePlayer(Vector3 direction)
    {
        if (direction.x < 0)
        {
            lastDirection = -1f;
        }
        else if (direction.x > 0)
        {
            lastDirection = 1f;
        }

        float targetAngle = lastDirection == 1f ? 90f : -90f;
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
            ResetComboCounter();
            jumpCount++;
            float jumpPower = jumpCount == 1 ? firstJumpPower : secondJumpPower;
            verticalVelocity = Mathf.Sqrt(jumpPower * -2f * gravity);

            if (jumpCount == 1)
                playerStateMachine.Jump();
            else
                playerStateMachine.Jump2();
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.time > nextDashTime)
        {
            ResetComboCounter();
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
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 dashDirection = new Vector3(x, 0f, 0f).normalized;
        controller.Move(dashDirection * dashSpeed * Time.deltaTime);
    }

    private void EndDash()
    {
        isDashing = false;
    }

    public void AddSpeed()
    {
        speed *= 5f; // Increase speed by 50% as an example.
        StartCoroutine(ResetSpeedAfterDuration(2f)); // Reset speed after 5 seconds.
    }

    private IEnumerator ResetSpeedAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed /= 5f; // Return speed to original value.
    }

    private void CheckDeath()
    {
        if (healthSystem.GetHealth() <= 0)
        {
            playerStateMachine.Death(); 
        }
    }

    private void HandleAttack()
    {
        if (Time.time > nextAttackTime && Input.GetKeyDown(KeyCode.J))
        {
            //Debug.Log(attackComboCount);
            // Check if we need to reset the combo based on the last attack time
            if (Time.time - lastAttackTime > comboResetTime || attackComboCount == 3)
            {
                ResetComboCounter();
            }
            

            // Enqueue the attack if we're below the maximum combo count
            if (attackComboCount < 3)  // Assuming a 3-attack combo
            {
                attackComboCount++;
                attackQueue.Enqueue(attackComboCount);
            }

            // If not currently attacking, handle the next attack in the queue
            /*
            if (!isAttacking)
            {
                HandleNextAttackInQueue();
            }
            */
            HandleNextAttackInQueue();

            lastAttackTime = Time.time;

            // Start the combo reset timer
            // (Eon) Deleted bc this can be replaced by the code above
            //StartCoroutine(ComboResetTimer());
        }
    }

    private IEnumerator ComboResetTimer()
    {
        yield return new WaitForSeconds(comboResetTime);
        if (Time.time - lastAttackTime >= comboResetTime)
        {
            ResetComboCounter();
        }
    }

    private void ResetComboCounter()
    {
        attackComboCount = 0;
        attackQueue.Clear();
    }

    public void HandleNextAttackInQueue()
    {
        if (attackQueue.Count > 0)
        {
            hitbox.SetActive(true);
            int nextAttack = attackQueue.Dequeue();

            switch (nextAttack)
            {
                case 1:
                    playerStateMachine.Attack1();
                    break;
                case 2:
                    playerStateMachine.Attack2();
                    break;
                case 3:
                    playerStateMachine.Attack3();
                    break;
            }

            isAttacking = true;

            nextAttackTime = Time.time + 1f / attackRate;
        }
        else
        {
            isAttacking = false;
        }
    }
}