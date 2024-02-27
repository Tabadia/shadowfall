using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    float OnCollisionEnter(Collision collision)
	{
		Vector3 normal = collision.contacts[0].normal;

		float collisionAngleTest1 = Vector3.Angle(Vector3.zero, -normal);

		return collisionAngleTest1;
    }
}
