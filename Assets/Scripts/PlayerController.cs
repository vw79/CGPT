using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    //character controller
    private Vector2 _input;   //player key input
    private CharacterController _characterController;
    private Vector3 _direction;   //character facing direction
	
	
    //character movement
    [SerializeField] private float speed;

    //character view rotation
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    //gravity
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplyer = 3.0f;
    private float _velocity;   //character downfall velocity
    private bool IsGrounded() => _characterController.isGrounded;

    //character jump
    [SerializeField] private float jumpPower;
    private int _numberOfJumps;
    [SerializeField] private float maxNumberOfJumps = 2;

    //character dash
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;   //dashing duration time



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
        //character move in direction with a speed
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    private void ApplyRotation()
    {
        //character view angle rotation
        if (_input.sqrMagnitude == 0) return;   //if no input direction, then nothing to do.
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;   //get character upcoming facing direction angle.
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);   //smooth character rotation.
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);   //finally, character rotates in smooth angle we want.
    }

    private void ApplyGravity()
    {
        //character gravity
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;   //gravity = 1, which does not affect to slower character velocity if grounded
        }
        else
        {
            _velocity += _gravity * gravityMultiplyer * Time.deltaTime;   //gravity keep increasing while not grounded
        }
        
        _direction.y = _velocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        //character moving through input
        _input = context.ReadValue<Vector2>();   //get player input direction(x,y) values
        _direction = new Vector3(_input.x, 0f, 0f);   //change character direction(x) based on above input value.
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
        if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

        _numberOfJumps++;
        _velocity += jumpPower / _numberOfJumps;    //every time jump, jump power is decreased
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

        while(Time.time < startTime + dashTime)
        {
            _characterController.Move(_direction * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

