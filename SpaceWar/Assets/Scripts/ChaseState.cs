﻿using UnityEngine;
using System.Collections;

public class ChaseState : State
{
	GameObject enemyGameObject;
	float shootTime = 0.25f;
	
	public override string Description()
	{
		return "Chase State";
	}
	
	public ChaseState(GameObject myGameObject, GameObject enemyGameObject) : base(myGameObject)
	{
		this.enemyGameObject = enemyGameObject;
	}
	
	public override void Enter()
	{
		myGameObject.GetComponent<SteeringBehaviours>().DisableAll();
		myGameObject.GetComponent<SteeringBehaviours>().offsetPursuitEnabled = true;
		myGameObject.GetComponent<SteeringBehaviours>().offsetPursuitTarget = enemyGameObject;
		myGameObject.GetComponent<SteeringBehaviours>().maxSpeed += 2f;
	}
	
	public override void Exit()
	{
		
	}
	
	public override void Update()
	{
		shootTime += Time.deltaTime;
		float fov = Mathf.PI / 4.0f;
		float angle;
		Vector3 toEnemy = (enemyGameObject.transform.position - myGameObject.transform.position);
		toEnemy.Normalize();
		angle = (float) Mathf.Acos(Vector3.Dot(toEnemy, myGameObject.transform.forward));

		if (angle < fov)
		{
			if (shootTime > 0.25f)
			{
				GameObject laser = new GameObject();
				laser.AddComponent<Laser>();
				laser.transform.position = myGameObject.transform.position;
				laser.transform.forward = myGameObject.transform.forward;
				shootTime = 0.0f;
			}
		}
	}
}
