using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
}

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState currentState = PlayerState.Idle;

    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;

        // Exit the current state (if needed)
        // No exit behavior for now, but can be added if needed in the future.

        // Enter the new state
        currentState = newState;
    }

    // Call this method when the player attacks
    public void Idle()
    {
        ChangeState(PlayerState.Idle);
    }
	
    // Call this method when the player attacks
    public void Running()
    {
        ChangeState(PlayerState.Running);
    }
	
    // Call this method when the player jumps
    public void Jump()
    {
        ChangeState(PlayerState.Jumping);
    }

    // Call this method when the player attacks
    public void Attack()
    {
        return;
    }
	
    public PlayerState GetCurrentState()
    {
        return currentState;
    }
}
