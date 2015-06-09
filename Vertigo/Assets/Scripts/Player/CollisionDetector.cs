using UnityEngine;
using System.Collections;

public class CollisionDetector
{
	public CollisionDetectorRaycast[] raycasts;
	public float EPSILON = .1f;
	private LayerMask colliderMask;
	public float rotation = 0f;

	public CollisionDetector(LayerMask colliderMask, params CollisionDetectorRaycast[] r)
	{
		raycasts = r;
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
		float maxDistance = -1f;
		foreach (CollisionDetectorRaycast ray in raycasts)
		{
			if (numberDistance(ray.angle, angle) < EPSILON)
			{ 
				CollisionDetectorRaycast rotatedRaycast = ray.Rotate (0);

				RaycastHit2D hit = Physics2D.Raycast (ray.relativePosition + objectPosition, new Vector2(Mathf.Cos (ray.angle), Mathf.Sin(ray.angle)), 
				                                      distance: ray.length, layerMask: colliderMask);

				if(hit.collider != null)
				{
					maxDistance = Mathf.Max (maxDistance, (hit.point - (ray.relativePosition + objectPosition)).magnitude);
				}
			}
		}

		return maxDistance;
	}

	private float numberDistance(float nr1, float nr2)
	{
		return Mathf.Abs(nr1 - nr2);
	}
}
