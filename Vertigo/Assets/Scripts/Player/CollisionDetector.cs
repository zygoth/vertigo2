using UnityEngine;
using System.Collections;

public class CollisionDetector
{
	private CollisionDetectorRaycast[] raycasts;
	private CollisionDetectorRaycast[] rotatedRaycasts;

	public float EPSILON = .1f;
	private LayerMask colliderMask;
	private float rotation = 0f;

	public CollisionDetector(LayerMask colliderMask, params CollisionDetectorRaycast[] r)
	{
		raycasts = r;
		rotatedRaycasts = (CollisionDetectorRaycast[])raycasts.Clone ();
		this.colliderMask = colliderMask;
	}

	/*
	 * Gets the maximum distance this object can move in a specified
	 * direction (*angle*).  Does this by taking any raycasts attached
	 * to this detector with a similar angle and taking the min of their
	 * results.  Returns -1 if there are no raycasts going that direction
	 * or none of the raycasts hit anything.
	 */
	public float getMaxDistance(Vector2 objectPosition, float angle)
	{
		float maxDistance = 100000f;
		foreach (CollisionDetectorRaycast ray in rotatedRaycasts)
		{
			if (numberDistance(ray.angle, angle) < EPSILON)
			{ 
				RaycastHit2D hit = Physics2D.Raycast (ray.relativePosition + objectPosition, new Vector2(Mathf.Cos (ray.angle), Mathf.Sin(ray.angle)), 
				                                      distance: ray.length, layerMask: colliderMask);

				if(hit.collider != null)
				{
					maxDistance = /*Mathf.Max (maxDistance, */(hit.point - (ray.relativePosition + objectPosition)).magnitude/*)*/;
				}
			}
		}

		return maxDistance;
	}

	/*
	 * Updated the rotation of raycasts associated with this CollisionDetector. This
	 * way the raycasts change their positions and angles relative to the object. These
	 * rotated raycasts are saved in an array for efficient access by methods like
	 * getMaxDistance.
	 */
	public void setRotation(float newRotation)
	{
		rotation = newRotation;

		for (int i = 0; i < raycasts.Length; i++)
		{
			rotatedRaycasts[i] = raycasts[i].Rotate (newRotation);
		}
	}

	private float numberDistance(float nr1, float nr2)
	{
		return Mathf.Abs(nr1 - nr2);
	}

	/*
	 * Adds two angles, keeping the result between -2pi and 2pi and rounding when very close
	 * to the borders so that 2pi and 0 will evaluate equal.
	 */
	private float angleAdd(float f1, float f2)
	{
		float answer = (f1 + f2) % (Mathf.PI * 2);
		if(numberDistance (Mathf.Abs(answer), Mathf.PI * 2) < .001)
		{
			return 0f;
		}

		return answer;
	}
}
