using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour
{
	State currentState;
	
	public void Update()
	{
		if (currentState != null)
		{
			Debug.Log("Current state: " + currentState.Description());
			currentState.Update();
		}
	}
	
	public void SwitchState(State newState)
	{
		if (currentState != null)
		{
			currentState.Exit();
		}
		
		currentState = newState;
		if (newState != null)
		{
			currentState.Enter();
		}
	}
}

