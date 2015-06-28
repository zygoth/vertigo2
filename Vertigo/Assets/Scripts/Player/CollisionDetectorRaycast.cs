using UnityEngine;
using System.Collections;

/*
 * This class represents a raycast used by a CollisionDetector object.
 * Its purpose is to communicate to that class the kind of raycast it should
 * use to determine collisions.
 */
public class CollisionDetectorRaycast
{
	// The angle at which the raycast should fire
	public float angle { get; set; }
	// The position of the raycast relative to the colliding object
	public Vector2 relativePosition { get; set; }
	// The length of the raycast.  Should be as long as the fastest speed of the object to prevent clipping.
	public float length { get; set; }

	public CollisionDetectorRaycast Rotate(float radians)
	{
		return new CollisionDetectorRaycast{angle = angleAdd(this.angle, radians),
											relativePosition = this.relativePosition.Rotate (radians),
											length = this.length};
	}

	/*
	 * Test method
	 */
	public void testRotate()
	{
		Debug.Log ("testing raycast rotate");
		CollisionDetectorRaycast testRaycast = new CollisionDetectorRaycast {angle = 0f, relativePosition = new Vector2(1f, -1f), length = 1f};
		CollisionDetectorRaycast rotatedRay = testRaycast.Rotate (Mathf.PI / 2f);
		Debug.Log ("rotated raycast angle: " + rotatedRay.angle);
		Debug.Log ("rotated raycast position: " + rotatedRay.relativePosition);
		Debug.Log ("rotated raycast length: " + rotatedRay.length);
	}

	/*
	 * Helper method
	 * Gets the distance between two numbers.
	 */
	private float numberDistance(float nr1, float nr2)
	{
		return Mathf.Abs(nr1 - nr2);
	}
	
	/*
	 * Helper method
	 * Adds two angles, keeping the result between -2pi and 2pi and rounding when very close
	 * to the borders so that 2pi and 0 will evaluate equal.
	 */
	private float angleAdd(float f1, float f2)
	{
		float answer = (f1 + f2) % (Mathf.PI * 2);
		if(numberDistance (Mathf.Abs(answer), Mathf.PI * 2) < .01)
		{
			return 0f;
		}
		
		return answer;
	}
}

/*
 * Helper method
 * Extension to the Vector2 class to allow for easy rotation.
 */
public static class Vector2Extension
{
	public static Vector2 Rotate(this Vector2 v, float radians)
	{
		return Quaternion.Euler(0, 0, Mathf.Rad2Deg * radians) * v;
	}
}