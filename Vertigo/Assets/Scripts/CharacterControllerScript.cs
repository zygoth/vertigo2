using UnityEngine;
using System.Collections;

public class CharacterControllerScript: MonoBehaviour
{
	// Maximum run speed
	public float MAXSPEED = 10f;
	public Vector2 gravityVector = new Vector2 (0f, -30f);
	bool facingRight = true;	
	bool hurtInvincibility = false;
	Timer hurtInvincibilityTimer = new Timer ();
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
		rigidbody2D.AddForce (gravityVector);
	}

	/*
	 * Called every time a frame is rendered on screen, so may happen at variable rates.  Use for interrupt-based events.
	 */
	void Update()
	{
		doJumpCheck ();
		doWrapping ();
		doMovement ();
		doGroundCheck ();

		if(hurtInvincibilityTimer.GetElapsedTimeSecs() > 3)
		{
			hurtInvincibilityTimer.Start ();
			hurtInvincibilityTimer.Stop ();
			hurtInvincibility = false;
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
			sr.color = new Color(1f,1f,1f,1f);
		}
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

		if (!hurtInvincibility)
		{
			Debug.Log ("ouch!");
			anim.SetTrigger ("HurtTrigger");
			hurtInvincibility = true;
			//Adjust health. Currently just subtracts 25 health
			GameObject character = GameObject.Find ("Character");
			PlayerHealth health = (PlayerHealth) character.GetComponent ("PlayerHealth");
			health.adjustCurHealth(-25);
			hurtInvincibilityTimer.Start ();
			// set to partly transparent to indicate invincibility
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
			sr.color = new Color(1f,1f,1f,.6f);
		}
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
		/*float amount = -90 * (newGravity - gravity);
		Debug.Log ("rotate " + amount);
		transform.Rotate (0, 0, amount);*/ //this bit's broken somehow
		transform.localEulerAngles = new Vector3(0,0, (int)newGravity * -90f);
		print (transform.localEulerAngles.z);
		gravity = newGravity;
		switch (newGravity)
		{
		case gravityDirection.DOWN:
			gravityVector.x = 0f;
			gravityVector.y = -30f; //FIXME: magic numbers
			break;
		case gravityDirection.LEFT:
			gravityVector.x = -30f;
			gravityVector.y = 0f;
			break;
		case gravityDirection.UP:
			gravityVector.x = 0f;
			gravityVector.y = 30f;
			break;
		case gravityDirection.RIGHT:
			gravityVector.x = 30f;
			gravityVector.y = 0f;
			break;
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