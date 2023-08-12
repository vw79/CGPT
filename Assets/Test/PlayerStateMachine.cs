using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Jumping2,
    ForwardJumping,
    Attacking,
    AttackingCombo1,
    AttackingCombo2,
    AttackingCombo3,
    Dead,
}

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState currentState = PlayerState.Idle;

    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    public void Idle()
    {
        ChangeState(PlayerState.Idle);
    }

    public void Running()
    {
        ChangeState(PlayerState.Running);
    }

    public void Jump()
    {
        ChangeState(PlayerState.Jumping);
    }

    public void Jump2()
    {
        ChangeState(PlayerState.Jumping2);
    }

    public void ForwardJump()
    {
        ChangeState(PlayerState.ForwardJumping);
    }

    public void Attack1()
    {
        ChangeState(PlayerState.AttackingCombo1);
    }

    public void Attack2()
    {
        ChangeState(PlayerState.AttackingCombo2);
    }

    public void Attack3()
    {
        ChangeState(PlayerState.AttackingCombo3);
    }

    public void Death()
    {
        ChangeState(PlayerState.Dead);
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }
}
