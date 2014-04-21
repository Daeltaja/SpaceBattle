using UnityEngine;
using System.Collections;

public class PatrolState : State
{
	GameObject enemyGameObject;

	public override string Description()
	{
		return "Patrolling State";
	}
	
	public PatrolState(GameObject myGameObject, GameObject enemyGameObject) : base(myGameObject)
	{
		this.enemyGameObject = enemyGameObject;
	}
	
	public override void Enter()
	{
		myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
		myGameObject.GetComponent<SteeringBehaviours>().followPathEnabled = true;
		myGameObject.GetComponent<SteeringBehaviours>().maxSpeed -= 2f;
	}
	
	public override void Exit()
	{
		
	}
	
	public override void Update()
	{
		if(GameManager.warpedDiversion) //one ship should go alert, other stay patrolling, do this here!
		{
			//Debug.Log("Huh");
			GameObject gmGO = GameObject.Find ("GameManager");
			GameManager gm = gmGO.GetComponent<GameManager>();
		}
	}	
}
