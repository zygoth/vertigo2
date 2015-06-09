using UnityEngine;
using System.Collections;

/*
 * This class represents a raycast used by a CollisionDetector object.
 * Its purpose is to communicate to that class the kind of raycast it should
 * use to determine collisions.
 */
public class CollisionDetectorRaycast
{
	public float angle { get; set; }
	public Vector2 relativePosition { get; set; }
	public float length { get; set; }

	public CollisionDetectorRaycast Rotate(float radians)
	{
		return new CollisionDetectorRaycast{angle = (this.angle + radians) % (Mathf.PI * 2f),
											relativePosition = this.relativePosition.Rotate (radians),
											length = this.length};
	}

	public void testRotate()
	{
		Debug.Log ("testing raycast rotate");
		CollisionDetectorRaycast testRaycast = new CollisionDetectorRaycast {angle = 0f, relativePosition = new Vector2(1f, -1f), length = 1f};
		CollisionDetectorRaycast rotatedRay = testRaycast.Rotate (Mathf.PI / 2f);
		Debug.Log ("rotated raycast angle: " + rotatedRay.angle);
		Debug.Log ("rotated raycast position: " + rotatedRay.relativePosition);
		Debug.Log ("rotated raycast length: " + rotatedRay.length);
	}
}

public static class Vector2Extension
{
	public static Vector2 Rotate(this Vector2 v, float radians)
	{
		return Quaternion.Euler(0, 0, Mathf.Rad2Deg * radians) * v;
	}
}