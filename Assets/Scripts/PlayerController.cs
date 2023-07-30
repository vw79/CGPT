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
    private bool canRotate = true;

    private PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
		Debug.Log("Current Direction: " + _direction);
        ApplyRotation();
        ApplyMovement();
        ApplyGravity();
    }

/*========
Movement
========*/
    private void ApplyMovement()
    {
		//if (isAttacking) return;
        //_characterController.Move(_direction * speed * Time.deltaTime);
		Vector3 moveDirection = new Vector3(_direction.x, _velocity, _direction.z);
		_characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    private void ApplyRotation()
    {
		if (!canRotate) return;

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

		var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
		transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
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
		_input = context.ReadValue<Vector2>();

		if (context.canceled)
		{
			_direction = Vector3.zero;
			playerStateMachine.Idle();
		}
		else
		{
			_direction = new Vector3(_input.x, 0f, _input.y);  // Assuming you want to use _input.y for the Z-axis

			// Update _lastValidDirection when there's valid input
			if (_input != Vector2.zero)
			{
				_lastValidDirection = _direction;  // Use the current _direction as the last valid direction
			}

			playerStateMachine.Running();
		}
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
		if (!isAttacking)
		{
			isAttacking = true;
			canRotate = false;
			StartCoroutine(EndAttack());
			playerStateMachine.Attack();
		}
	}
	
	private IEnumerator EndAttack()
	{
		yield return new WaitForSeconds(2.5f);
		isAttacking = false;
		canRotate = true;
	}
}
