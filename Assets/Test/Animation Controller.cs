using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private PlayerController playerController;
    private Animator animator;
    private PlayerStateMachine playerStateMachine;
    private PlayerState previousState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
		playerController = GetComponent<PlayerController>();
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
	
	private IEnumerator SmoothSetSpeed(float targetValue, float duration)
	{
		float startValue = animator.GetFloat("Speed");
		float elapsedTime = 0;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
			animator.SetFloat("Speed", newValue);
			yield return null;
		}

		animator.SetFloat("Speed", targetValue);
	}
	
	private IEnumerator PlayComboSequence(params string[] comboNames)
	{
		foreach (var combo in comboNames)
		{
			animator.SetTrigger(combo);
			yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for the animation length before starting the next
		}
	}
	
    private void SetAnimation(PlayerState state)
    {
        switch (state)
		{
			case PlayerState.Idle:
				StopAllCoroutines();
				float oscillation = Mathf.Sin(Time.time) * 0.5f + 0.5f;
				StartCoroutine(SmoothSetSpeed(oscillation * 0.5f, 0.1f));
				break;
			case PlayerState.Running:
				StopAllCoroutines();
				StartCoroutine(SmoothSetSpeed(1, 0.1f));
                break;
			case PlayerState.Jumping:
                break;
			case PlayerState.AttackingCombo1:
                StopAllCoroutines();
                animator.SetInteger("ComboCount", playerController.comboCount);
                animator.SetBool("Attack",true);
				break;
            case PlayerState.AttackingCombo2:
                StopAllCoroutines();
                animator.SetInteger("ComboCount", playerController.comboCount);
                animator.SetTrigger("Attack");
                break;
            case PlayerState.AttackingCombo3:
				StopAllCoroutines();
				animator.SetInteger("ComboCount", playerController.comboCount);
				animator.SetTrigger("Attack");
				break;
			default:
				break;
		}
    }
}
