using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    Animator animator;
    int isRunningHash;
    int isJumpingHash;
	int isGroundedHash;
	int isFallingHash;
	int isAttackHash;
	int isStopHash;
		
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("IsRunning");
        isJumpingHash = Animator.StringToHash("IsJumping");
		isGroundedHash = Animator.StringToHash("IsGrounded");
		isFallingHash = Animator.StringToHash("IsFalling");
		isAttackHash = Animator.StringToHash("IsAttack");
		isStopHash = Animator.StringToHash("IsStop");
    }

	// Update is called once per frame
	void Update()
	{
		bool isRunning = animator.GetBool(isRunningHash);
		bool isJumping = animator.GetBool(isJumpingHash);
		bool isGrounded = animator.GetBool(isGroundedHash);
		bool isFalling = animator.GetBool(isFallingHash);
		bool isAttack = animator.GetBool(isAttackHash);
		bool isStop = animator.GetBool(isStopHash);
		
		// Run
		bool forwardPressed = Input.GetKey("d") || Input.GetKey("a");
		
		// Jump
		bool jumpPressed = Input.GetKey(KeyCode.Space);
		
		//Dash
		bool stopPressed = Input.GetKey("s");
	
		if (!isRunning && !forwardPressed && !jumpPressed)
		{
			animator.SetBool(isRunningHash, false);
			animator.SetBool(isJumpingHash, false);
			animator.SetBool(isGroundedHash, true);
			animator.SetBool(isFallingHash, true);
		}
		
		if (isRunning && forwardPressed && jumpPressed)
		{
			animator.SetBool(isRunningHash, true);
			animator.SetBool(isJumpingHash, true);
			animator.SetBool(isGroundedHash, false);
			animator.SetBool(isFallingHash, false);
		}
		
		if (isRunning && !forwardPressed && !jumpPressed)
		{
			animator.SetBool(isRunningHash, false);
			animator.SetBool(isJumpingHash, false);
			animator.SetBool(isGroundedHash, true);
			animator.SetBool(isFallingHash, true);
		}

		if (isRunning && forwardPressed && !jumpPressed)
		{
			animator.SetBool(isRunningHash, true);
			animator.SetBool(isJumpingHash, false);
			animator.SetBool(isGroundedHash, true);
			animator.SetBool(isFallingHash, true);
		}
		
		if (!isRunning && forwardPressed && !jumpPressed)
		{
			animator.SetBool(isRunningHash, true);
			animator.SetBool(isJumpingHash, false);
			animator.SetBool(isGroundedHash, true);
			animator.SetBool(isFallingHash, true);
		}
		
		if (!isRunning && forwardPressed && jumpPressed)
		{
			animator.SetBool(isRunningHash, true);
			animator.SetBool(isJumpingHash, true);
			animator.SetBool(isGroundedHash, false);
			animator.SetBool(isFallingHash, false);
		}
		
		if (!isRunning && !forwardPressed && jumpPressed)
		{
			animator.SetBool(isRunningHash, false);
			animator.SetBool(isJumpingHash, true);
			animator.SetBool(isGroundedHash, false);
			animator.SetBool(isFallingHash, false);
		}

		if (isRunning && !forwardPressed && jumpPressed)
		{
			animator.SetBool(isRunningHash, true);
			animator.SetBool(isJumpingHash, true);
			animator.SetBool(isGroundedHash, false);
			animator.SetBool(isFallingHash, false);
		}
		
		if (isRunning && !forwardPressed && jumpPressed)
		{
			animator.SetBool(isRunningHash, true);
			animator.SetBool(isJumpingHash, true);
			animator.SetBool(isGroundedHash, false);
			animator.SetBool(isFallingHash, false);
		}
		
		//Attack
		bool attackPressed = Input.GetKey("j");
		
		if (attackPressed)
		{
			animator.SetBool(isAttackHash, true);
		}
		else
		{
			animator.SetBool(isAttackHash, false);
		}
		
		if (isJumping && isStop)
		{
			animator.SetBool(isStopHash, true);
		}
		
		if (!isStop)
		{
			animator.SetBool(isStopHash, false);
		}
		
	}
}