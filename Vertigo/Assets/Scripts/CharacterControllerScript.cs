using UnityEngine;
using System.Collections;

public class CharacterControllerScript: MonoBehaviour
{
	// Maximum run speed
	public float MAXSPEED = 10f;
	public Vector2 gravityVector = new Vector2 (0f, -30f);
	bool facingRight = true;	
	Animator anim;
	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float JUMPFORCE = 800f;
	// Enum for gravity direction
	public enum gravityDirection {DOWN, LEFT, UP, RIGHT};
	// The current gravity direction
	public gravityDirection gravity = gravityDirection.DOWN;

	/*
	 * Called when the object is instantiated (I think).
	 */
	void Start()
	{
		anim = GetComponent<Animator>();
	}

	/*
	 * Called at fixed intervals during the game.  Use for things that must happen at steady intervals of time. 
	 */
	void FixedUpdate()
	{
		doGroundCheck ();

		doMovement ();

		rigidbody2D.AddForce (gravityVector);
	}

	/*
	 * Called every time a frame is rendered on screen, so may happen at variable rates.  Use for interrupt-based events.
	 */
	void Update()
	{
		doJumpCheck ();
		doWrapping ();
	}

	void doGroundCheck()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);
	}
	
	void doMovement()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		float movement;
		
		if(gravity == gravityDirection.DOWN || gravity == gravityDirection.UP)
		{
			movement = horizontal;
			vertical = 0f;
		}
		else
		{
			movement = vertical;
			horizontal = 0f;
		}
		
		anim.SetFloat("Speed", Mathf.Abs(movement));
		
		if(Mathf.Abs(movement) > 0)
		{
			if(horizontal == 0)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, vertical * MAXSPEED);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(horizontal * MAXSPEED, rigidbody2D.velocity.y);
			}
		}
		
		if(gravity == gravityDirection.DOWN || gravity == gravityDirection.RIGHT)
		{
			if(movement > 0 && !facingRight)
				Flip();
			else if (movement < 0 && facingRight)
				Flip();
		}
		
		if(gravity == gravityDirection.UP || gravity == gravityDirection.LEFT)
		{
			if(movement < 0 && !facingRight)
				Flip();
			else if (movement > 0 && facingRight)
				Flip();
		}
	}

	void doJumpCheck()
	{
		if (grounded && Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetBool ("Ground", false);

			Vector2 jumpVector = new Vector2();

			switch(gravity)
			{
				case gravityDirection.DOWN:
					jumpVector = (new Vector2(0, JUMPFORCE));
					break;
				case gravityDirection.RIGHT:
					jumpVector = (new Vector2(-JUMPFORCE, 0));
					break;
				case gravityDirection.UP:
					jumpVector = (new Vector2(0, -JUMPFORCE));
					break;
				case gravityDirection.LEFT:
					jumpVector = (new Vector2(JUMPFORCE, 0));
					break;
			}

			rigidbody2D.AddForce (jumpVector);
		}
	}

	/*
	 * Generally only called by outside GameObjects.
	 */
	public void hurt()
	{
		Debug.Log ("ouch!");
		// TODO: implement this, make it reduce health by one and cause a hurt animation
	}

	public void RotateLeft () 
	{
		transform.Rotate (0, 0, -90f);
		Debug.Log ("rotation" + transform.rotation.z);
		gravity = (gravityDirection)((((int)gravity) + 1) % 4);
		gravityVector = new Vector2 (gravityVector.y, -gravityVector.x);
	}

	public void switchGravity(gravityDirection newGravity)
	{
		while(gravity != newGravity)
		{
			RotateLeft ();
		}
	}

	void doWrapping()
	{
		GameObject camera = GameObject.Find ("Main Camera");
		CameraController cameraScript = (CameraController) camera.GetComponent ("CameraController");

		if (cameraScript.currentMode == CameraController.ScreenMode.HORIZONTAL)
		{
			if(transform.position.y < cameraScript.transform.position.y - 13)
			{
				transform.position = new Vector3 (transform.position.x, cameraScript.transform.position.y + 13, transform.position.z);
			}

			if(transform.position.y > cameraScript.transform.position.y + 13)
			{
				transform.position = new Vector3 (transform.position.x, cameraScript.transform.position.y - 13, transform.position.z);
			}
		}
		else
		if(cameraScript.currentMode == CameraController.ScreenMode.VERTICAL)
		{
			if(transform.position.x < cameraScript.transform.position.x - 17)
			{
				transform.position = new Vector3 (cameraScript.transform.position.x + 17, transform.position.y, transform.position.z);
			}
			
			if(transform.position.x > cameraScript.transform.position.x + 17)
			{
				transform.position = new Vector3 (cameraScript.transform.position.x - 17, transform.position.y, transform.position.z);
			}
		}
	}
		
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}