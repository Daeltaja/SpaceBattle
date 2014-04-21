using UnityEngine;
using System.Collections;

class Laser : MonoBehaviour
{
	public void Update()
	{
		float speed = 5.0f;

		transform.position += transform.forward * speed;
		Debug.DrawLine(transform.position, transform.position + transform.forward * 10.0f, Color.red);
	}
}
