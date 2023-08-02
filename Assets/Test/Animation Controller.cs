using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private PlayerCon playerController;
    private Animator animator;
    private PlayerStateMachine playerStateMachine;
    private PlayerState previousState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
		playerController = GetComponent<PlayerCon>();
    }

    private void Update()
    {
        PlayerState currentState = playerStateMachine.GetCurrentState();

        if (previousState != currentState)
        {
            SetAnimation(currentState);
            previousState = currentState;
        }
    }
		
    private void SetAnimation(PlayerState state)
    {
        switch (state)
		{
			case PlayerState.Idle:
                float randomSpeed = Random.Range(0f, 0.25f);
                animator.SetFloat("Speed", randomSpeed);
                break;
			case PlayerState.Running:
                animator.SetFloat("Speed", 0.5f);
                break;
			case PlayerState.Jumping:
                animator.Play("JumpUp");
                break;
            case PlayerState.Jumping2:
                animator.Play("JumpUp2");
                break;
            case PlayerState.ForwardJumping:
                animator.Play("JumpForward");
                break;
            case PlayerState.AttackingCombo1:
                animator.SetInteger("ComboCount", playerController.comboC);
                animator.SetBool("Attack",true);
				break;
            case PlayerState.AttackingCombo2:
                animator.SetInteger("ComboCount", playerController.comboC);
                animator.SetTrigger("Attack");
                break;
            case PlayerState.AttackingCombo3:
				animator.SetInteger("ComboCount", playerController.comboC);
				animator.SetTrigger("Attack");
				break;
			default:
				break;
		}
    }
}
