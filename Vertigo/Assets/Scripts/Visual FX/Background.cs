using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{
	public float MAXVELOCITY = 3f;
	public float ACCEL_RATE = 0.05f;
	public float SLOW_RATE = 0.95f;

	// Use this for initialization
	void Start () {
	
	}

	private void doGravityMovement()
	{
		GameObject character = GameObject.Find ("Character");
		CharacterControllerScript characterScript = (CharacterControllerScript) character.GetComponent ("CharacterControllerScript");

		float newXVelocity = rigidbody2D.velocity.x;
		float newYVelocity = rigidbody2D.velocity.y;

		switch(characterScript.gravity)
		{
		case CharacterControllerScript.gravityDirection.DOWN:
			newXVelocity = rigidbody2D.velocity.x * SLOW_RATE;
			newYVelocity -= ACCEL_RATE;
			break;
		case CharacterControllerScript.gravityDirection.LEFT:
			newXVelocity = rigidbody2D.velocity.x - ACCEL_RATE;
			newYVelocity *= SLOW_RATE;
			break;
		case CharacterControllerScript.gravityDirection.UP:
			newXVelocity = rigidbody2D.velocity.x * SLOW_RATE;
			newYVelocity += ACCEL_RATE;
			break;
		case CharacterControllerScript.gravityDirection.RIGHT:
			newXVelocity = rigidbody2D.velocity.x + ACCEL_RATE;
			newYVelocity *= SLOW_RATE;
			break;
		}

		newXVelocity = enforceBounds (-MAXVELOCITY, MAXVELOCITY, newXVelocity);
		newYVelocity = enforceBounds (-MAXVELOCITY, MAXVELOCITY, newYVelocity);

		rigidbody2D.velocity = new Vector2 (newXVelocity, newYVelocity);
	}

	private float enforceBounds(float min, float max, float value)
	{
		if(value < min)
		{
			return min;
		}

		if(value > max)
		{
			return max;
		}

		return value;
	}

	private void doWrapping()
	{
		GameObject camera = GameObject.Find ("Main Camera");
		CameraController cameraScript = (CameraController) camera.GetComponent ("CameraController");
		float newX = transform.position.x;
		float newY = transform.position.y;
		float newZ = transform.position.z;
		
		if(transform.position.x > camera.transform.position.x + 15)
		{
			newX = transform.position.x - 15;
		}
		
		if(transform.position.x < camera.transform.position.x - 15)
		{
			newX = transform.position.x + 15;
		}
		
		if(transform.position.y > camera.transform.position.y + 12)
		{
			newY = transform.position.y - 12;
		}
		
		if(transform.position.y < camera.transform.position.y - 12)
		{
			newY = transform.position.y + 12;
		}
		
		transform.position = new Vector3 (newX, newY, newZ);
	}
	
	// Update is called once per frame
	void Update () 
	{
		doGravityMovement ();
		doWrapping ();
	}
}
