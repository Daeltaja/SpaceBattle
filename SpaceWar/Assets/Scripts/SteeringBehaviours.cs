using UnityEngine;
using System.Collections;

public class SteeringBehaviours : MonoBehaviour {

	//behaviour flags
	public bool seekEnabled;
	public bool fleeEnabled;
	public bool arriveEnabled;
	public bool pursuitEnabled;
	public bool evadeEnabled;
	public bool offsetPursuitEnabled;
	public bool followPathEnabled;
    public bool obstacleAvoidEnabled;

	//integrator 
	public float mass;
	public float maxSpeed;
	public Vector3 velocity;
	public Vector3 force;

	//behaviour targets
	public Vector3 seekPos;
	public Vector3 fleePos;
	public Vector3 arrivePos;
	public Vector3 offsetPursuitOffset;
	public GameObject seekTarget;
	public GameObject pursuitTarget;
	public GameObject evadeTarget;
	public GameObject offsetPursuitTarget;
	
	public SteeringBehaviours() //constructor
	{
		force = Vector3.zero;
		velocity = Vector3.zero;
		mass = 1.0f;
		maxSpeed = 5.0f;
	}

	public void DisableAll() //disabler
	{
		seekEnabled = false;
		fleeEnabled = false;
		pursuitEnabled = false;
		evadeEnabled = false;
		offsetPursuitEnabled = false;
		arriveEnabled = false;
		followPathEnabled = false;
        obstacleAvoidEnabled = false;
	}

	#region Steering Behaviours
	Vector3 Seek(Vector3 targetPos)
	{
		Vector3 desiredPos = targetPos - transform.position;
		desiredPos.Normalize();
		desiredPos *= maxSpeed;
		return (desiredPos - velocity);
	}

	Vector3 Flee(Vector3 targetPos)
	{
		Vector3 desiredPos = transform.position - targetPos;
		desiredPos.Normalize();
		desiredPos *= maxSpeed;
		return (desiredPos - velocity);
	}

	Vector3 Arrive(Vector3 targetPos)
	{
		Vector3 toTarget = targetPos - transform.position;
		float distance = toTarget.magnitude;
		float slowingDistance = 2.0f;
		if(distance == 0)
		{
			return Vector3.zero;
		}
		const float decelTweak = 0.0f;
		float rampedSpeed = maxSpeed * (distance / (slowingDistance * decelTweak));
		float clampedSpeed = Mathf.Min(rampedSpeed, maxSpeed);
		Vector3 desiredPos = clampedSpeed * (toTarget / distance);
		return (desiredPos - velocity);
	}

	Vector3 Pursue()
	{
		Vector3 toTarget = pursuitTarget.transform.position - transform.position;
		float distance = toTarget.magnitude;
		float time = distance / maxSpeed;
		Vector3 desiredPos = pursuitTarget.transform.position + (time * pursuitTarget.GetComponent<SteeringBehaviours>().velocity);
		return Seek (desiredPos);
	}

	Vector3 Evade()
	{
		float lookAhead = maxSpeed;
		
		Vector3 targetPos = evadeTarget.transform.position + (lookAhead * evadeTarget.GetComponent<SteeringBehaviours>().velocity);
		return Flee(targetPos);
	}
	
	Vector3 OffsetPursuit(Vector3 offset)
	{
		Vector3 target = Vector3.zero;
		target = offsetPursuitTarget.transform.TransformPoint(offset);
		
		float distance = (target - transform.position).magnitude;
		float lookAhead = distance / maxSpeed;

		target = target + (lookAhead * offsetPursuitTarget.GetComponent<SteeringBehaviours>().velocity);
		return Arrive(target);
	}

	Vector3 FollowPath()
	{
		Vector3 newPos = transform.position + Random.insideUnitSphere * 10f;
		return Seek(newPos);
	}
	#endregion

	#region Update
	void Update () 
	{
		if (seekEnabled)
		{
			force += Seek(seekPos);
		}
		if (fleeEnabled)
		{
			force += Flee(fleePos);
		}
		if (arriveEnabled)
		{
			force += Arrive(arrivePos);
		}
		if (pursuitEnabled)
		{
			force += Pursue();
		}
		if(evadeEnabled)
		{
			force += Evade();
		}
		if (offsetPursuitEnabled)
		{
			force += OffsetPursuit(offsetPursuitOffset);
		}
		if (followPathEnabled)
		{
			force += FollowPath();
		}
		//Force Integrator
		Vector3 accel = force / mass; //new vector for acceleration, which is the forceAcc divided by the mass of object
		velocity = velocity + accel * Time.deltaTime; 
		transform.position = transform.position + velocity * Time.deltaTime;
		force = Vector3.zero; //reset forceAcc to zero so each cycle
		if(velocity.magnitude > float.Epsilon) //if there is any velocity past smallest possible number, normalize its forward direction 
		{
			transform.forward = Vector3.Normalize(velocity);
		}
		velocity *= 0.99f;
	}
	#endregion
}
