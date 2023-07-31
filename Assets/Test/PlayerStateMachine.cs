using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
	Attacking,
	AttackingCombo1,
	AttackingCombo2,
	AttackingCombo3,
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
		Debug.Log("State changed to: " + currentState);

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
    public void Attack(int comboCount)
    {
        switch (comboCount)
		{
			case 1:
				ChangeState(PlayerState.AttackingCombo1);
				break;
			case 2:
				ChangeState(PlayerState.AttackingCombo2);
				break;
			case 3:
				ChangeState(PlayerState.AttackingCombo3);
				break;
		}
    }
	
    public PlayerState GetCurrentState()
    {
        return currentState;
    }
}
