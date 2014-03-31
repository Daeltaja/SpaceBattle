using UnityEngine;
using System.Collections;

public class MoveShip : MonoBehaviour
{
	Vector3 dest, toDest;
	public Transform target;
	public float speed = 5.0f;

	void Update()
	{
	    dest = target.position;
	    toDest = dest - transform.position;
	    if (toDest.magnitude > 0.1f)
	    {
	        toDest.Normalize();
	        transform.position += toDest * speed * Time.deltaTime;
	        transform.forward = toDest;
	    }
	}
}