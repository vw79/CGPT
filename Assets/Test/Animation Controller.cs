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

    public void OnAttackAnimationFinished()
    {
        GetComponent<PlayerCon>().HandleNextAttackInQueue();
    }

    private void Update()
    {
        PlayerState currentState = playerStateMachine.GetCurrentState();

        if (previousState != currentState)
        {
            SetAnimation(currentState);
            previousState = currentState;
        }

        // If the attack animation is finished, return player to Idle state
        if (previousState == PlayerState.AttackingCombo1 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            playerStateMachine.Idle();
        }
    }


    private void SetAnimation(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.SetFloat("Speed", Time.frameCount % 2 == 0 ? 0f : 0.25f);
                break;
            case PlayerState.Running:
                animator.SetFloat("Speed", 0.5f);
                break;
            case PlayerState.Jumping:
                animator.CrossFade("JumpUp", 0.1f);
                break;
            case PlayerState.Jumping2:
                animator.CrossFade("JumpUp2", 0.1f);
                break;
            case PlayerState.ForwardJumping:
                animator.CrossFade("JumpForward", 0.1f);
                break;
            case PlayerState.AttackingCombo1:
                animator.CrossFade("Combo1",0.1f);
                break;
            case PlayerState.AttackingCombo2:
                animator.CrossFade("Combo2", 0.1f);
                break;
            case PlayerState.AttackingCombo3:
                animator.CrossFade("Combo3", 0.1f);
                break;
            case PlayerState.Dead:
                animator.SetTrigger("IsDead");
                break;
            default:
                break;
        }
    }
}
