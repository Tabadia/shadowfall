using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
 	void OnCollisionEnter(Collision collision)
	{
		Vector3 normal = collision.contacts[0].normal;

		float collisionAngleTest1 = Vector3.Angle(Vector3.zero, -normal);

		Debug.Log(collisionAngleTest1);
    }
}
