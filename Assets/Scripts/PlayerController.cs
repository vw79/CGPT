using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
	private Vector3 _lastValidDirection;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float boostedSpeed = 50f;
    [SerializeField] private float boostedSpeedDuration = 3f;
	
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;
	
    private bool IsGrounded() => _characterController.isGrounded;

    [SerializeField] private float jumpPower;
    private int _numberOfJumps;
    [SerializeField] private float maxNumberOfJumps = 2;
	[SerializeField] private float secondJumpPower;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
	private bool canDash = true;
    [SerializeField] private float dashCooldown = 2.0f;

	private bool isAttacking = false;
    [SerializeField] private InputAction attackAction;
	public int comboCount = 0;
	private float comboResetTime = 1f;  // Time after which the combo count resets if no subsequent attacks are made.
	private float lastAttackTime;
	[SerializeField] private float comboWindow = 1f; // The time within which next attack should be registered for combo.
	private bool inComboWindow = false;

    private PlayerStateMachine playerStateMachine;
	
	private Vector3 _pendingDirection = Vector3.zero;
	private bool _directionChanged = false;
	private Vector3 _lastDirectionBeforeAttack = Vector3.zero;
	private Vector3 _intendedDirectionAfterAttack = Vector3.zero;

	
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        ApplyGravity();
		if (!isAttacking)
		{
			ApplyRotation();
			ApplyMovement();
		}
		
		if (Time.time - lastAttackTime > comboResetTime && comboCount > 0)
		{
			comboCount = 0;
		}
    }

/*========
Movement
========*/
    private void ApplyMovement()
	
    {
		Vector3 moveDirection = new Vector3(_direction.x, _velocity, _direction.z);
		_characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    private void ApplyRotation()
	{
		if (isAttacking) return;  // Skip rotation if attacking
		float targetAngle;

		// Update target angle only when there's input
		if (_input.sqrMagnitude != 0)
		{
			targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
		}
		else
		{
			targetAngle = transform.eulerAngles.y;  // Use the current rotation angle
		}

		if (_directionChanged)
		{
			transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
			_directionChanged = false;
		}
		else
		{
			var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
			transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
		}
	}


    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
		{
			_velocity = -1.0f;
		}
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
    }

    public void Move(InputAction.CallbackContext context)
	{
		
		Vector2 newInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            _direction = Vector3.zero;
			_intendedDirectionAfterAttack = Vector3.zero;
            playerStateMachine.Idle();
            return;
        }

        if (isAttacking)
		{
			// Store the intended direction but don't apply it
			_intendedDirectionAfterAttack = new Vector3(newInput.x, 0f, newInput.y);
			_direction = new Vector3(_input.x, 0f, _input.y);
			return;
		}

		if (_input != newInput)
		{
			_directionChanged = true;
		}

		_input = newInput;


		_direction = new Vector3(_input.x, 0f, _input.y);  // Assuming you want to use _input.y for the Z-axis

		// Update _lastValidDirection when there's valid input
		if (_input != Vector2.zero)
		{
			_lastValidDirection = _direction;  // Use the current _direction as the last valid direction
		}
		else
		{
			_direction = Vector3.zero;  // Reset direction when there's no current input
		}

		playerStateMachine.Running();
		//Debug.Log("Move function called with direction: " + _direction);
	}


/*========
Jump
========*/
	public void Jump(InputAction.CallbackContext context)
	{
		if (!context.started) return;
		if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
		if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

		float currentJumpPower = _numberOfJumps == 0 ? jumpPower : secondJumpPower;
		_numberOfJumps++;

		_velocity += currentJumpPower;
        playerStateMachine.Jump();
	}

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);
        _numberOfJumps = 0;
    }

/*========
Dash
========*/
    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.started || !canDash) return;
        StartCoroutine(StartDashing());
    }

    private IEnumerator StartDashing()
    {
        canDash = false;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            _characterController.Move(_direction * dashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void AddSpeed()
    {
        speed = boostedSpeed;
        Invoke("ResetSpeed", boostedSpeedDuration);
    }

    public void ResetSpeed()
    {
        speed = 10f;
    }

/*========
Attack
========*/
	public void Attack(InputAction.CallbackContext context)
	{
		if (!context.started) return;
		
		if (!isAttacking || comboCount < 3)
		{	
			isAttacking = true;
			_lastDirectionBeforeAttack = _direction;  // Store the current direction
			
			comboCount++;
			if (comboCount > 3) // If combo count exceeds the max combos, reset.
			{
				comboCount = 1;
			}

			lastAttackTime = Time.time;
			playerStateMachine.Attack(comboCount);

			if (!inComboWindow)
			{
				StartCoroutine(ComboTimer());
			}

			// Start the EndAttack coroutine after a delay.
			// Here, I'm using 1 second as an example, but you should replace this with the duration of your attack animation.
			StartCoroutine(EndAttack()); // Replace 1.0f with your attack animation duration
			//Debug.Log("Move function called with direction: " + _direction);
		}
	}
	
	private IEnumerator EndAttack()
	{
		// Adjust this value to control how long the player must wait before initiating the next attack.
		yield return new WaitForSeconds(1.0f); // Replace with your desired attack duration or a bit less.

		if (_intendedDirectionAfterAttack != Vector3.zero) 
		{
			_direction = _intendedDirectionAfterAttack; // Apply the intended direction
			_intendedDirectionAfterAttack = Vector3.zero;  // Reset the intended direction
			playerStateMachine.Running();
        }
		else if (_direction == Vector3.zero)
		{
			playerStateMachine.Idle();  // Set state to Idle if no movement input
		}

		isAttacking = false;  // Reset the flag when the attack ends
		//Debug.Log("Move function called with direction: " + _direction);
	}
	
	private IEnumerator ComboTimer()
	{
		inComboWindow = true;
		yield return new WaitForSeconds(comboWindow);
		comboCount = 0;
		inComboWindow = false;
	}
}
