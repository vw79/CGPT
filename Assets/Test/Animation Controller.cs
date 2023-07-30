using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerStateMachine playerStateMachine;
    private PlayerState previousState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
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
                animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                break;
            case PlayerState.Running:
                animator.SetFloat("Speed", 1);
                break;
            case PlayerState.Jumping:
                break;
            default:
                break;
        }
    }
}
