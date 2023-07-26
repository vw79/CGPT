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

    [SerializeField] private float speed;

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

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyRotation();
        ApplyMovement();
        ApplyGravity();
    }

    private void ApplyMovement()
    {
        // Character move in direction with a speed
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    private void ApplyRotation()
    {
        // Character view angle rotation
        if (_input.sqrMagnitude == 0) return;
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyGravity()
    {
        // Character gravity
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        // Character moving through input
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0f, 0f);
    }

	public void Jump(InputAction.CallbackContext context)
	{
		if (!context.started) return;
		if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
		if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

		// Set the jump power based on whether it's the first jump or the second jump
		float currentJumpPower = _numberOfJumps == 0 ? jumpPower : secondJumpPower;
		_numberOfJumps++;
		_velocity += currentJumpPower / _numberOfJumps;    //every time jump, jump power is decreased
	}

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        _numberOfJumps = 0;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        StartCoroutine(StartDashing());
    }

    private IEnumerator StartDashing()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            _characterController.Move(_direction * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void AddSpeed()
    {
        speed = 50f;
        Invoke("ResetSpeed", 3);
    }

    public void ResetSpeed()
    {
        speed = 10f;
    }
}
