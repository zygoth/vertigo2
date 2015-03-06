using UnityEngine;
using System.Collections;

public class CharacterControllerScript: MonoBehaviour
{
	// used to create shots when firing
	public GameObject shotPrefab;
	private float SHOTSPEED = 10f;
	// Maximum run speed
	public float MAXSPEED = 10f;
	public Vector2 gravityVector = new Vector2 (0f, -30f);
	public bool key = false;
	private float MAXFALLSPEED = 50;
	bool facingRight = true;	
	bool hurtInvincibility = false;
	Animator anim;
	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float JUMPFORCE = 800f;
	private string levelToLoad;
	// Enum for gravity direction
	public enum gravityDirection {DOWN, LEFT, UP, RIGHT};
	// The current gravity direction
	public gravityDirection gravity = gravityDirection.DOWN;
	/*
	 * Called when the object is instantiated (I think).
	 */
	void Start()
	{
		levelToLoad = Application.loadedLevelName;
		anim = GetComponent<Animator>();
	}

	/*
	 * Called at fixed intervals during the game.  Use for things that must happen at steady intervals of time. 
	 */
	void FixedUpdate()
	{
		if(isVertical (gravity) && Mathf.Abs (rigidbody2D.velocity.y) < MAXFALLSPEED || 
		   isHorizontal(gravity) && Mathf.Abs (rigidbody2D.velocity.x) < MAXFALLSPEED)
		{
			rigidbody2D.AddForce (gravityVector);
		}
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
		doShootCheck ();
		
		if (Mathf.Round(transform.localEulerAngles.z) % 90f > 1) // TODO: this is a hack to fix the strange rotation bug.
		{
			Debug.Log ("craps + " + transform.localEulerAngles.z % 90);
			transform.localEulerAngles = new Vector3 (0, 0, (int)gravity * -90f);
		}
	}

	private void doShootCheck()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			//TODO: finish this
			gravityDirection facingDirection = getFacingDirection();
			Vector3 shotPosition = transform.position;
			Vector2 shotVelocity = new Vector2(0f,0f);

			switch(facingDirection)
			{
			case gravityDirection.DOWN:
				shotPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
				shotVelocity = new Vector2(0f, -SHOTSPEED);
				break;
			case gravityDirection.LEFT:
				shotPosition = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
				shotVelocity = new Vector2(-SHOTSPEED, 0f);
				break;
			case gravityDirection.RIGHT:
				shotPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
				shotVelocity = new Vector2(SHOTSPEED, 0f);
				break;
			case gravityDirection.UP:
				shotPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
				shotVelocity = new Vector2(0f, SHOTSPEED);
				break;
			}

			GameObject shot = Instantiate (shotPrefab, shotPosition, Quaternion.identity) as GameObject;
			Shot script = (Shot)shot.GetComponent ("Shot");
			script.rigidbody2D.velocity = shotVelocity;
		}
	}

	/*
	 * Coroutine, called to remove invincibility after 3 seconds when hit.
	 */
	private IEnumerator removeHurtInvincibility()
	{
		while(true)
		{
			yield return new WaitForSeconds(3.0f); // wait half a second
			// do things
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

	bool isHorizontal(gravityDirection direction)
	{
		if(direction == gravityDirection.LEFT || direction == gravityDirection.RIGHT)
		{
			return true;
		}

		return false;
	}

	bool isVertical(gravityDirection direction)
	{
		if(direction == gravityDirection.UP || direction == gravityDirection.DOWN)
		{
			return true;
		}
		
		return false;
	}

	/*
	 * Returns the direction the character is facing.  Needed by Shots to know which way they should
	 * fly.
	 */
	public gravityDirection getFacingDirection()
	{
		if (gravity == gravityDirection.DOWN)
		{
			if (transform.localScale.x < 0) 
			{
				return gravityDirection.LEFT;
			}
			else
			{
				return gravityDirection.RIGHT;
			}
		}
		else if (gravity == gravityDirection.RIGHT)
		{
			if (transform.localScale.x < 0) 
			{
				return gravityDirection.DOWN;
			}
			else
			{
				return gravityDirection.UP;
			}
		}
		else if (gravity == gravityDirection.LEFT)
		{
			if (transform.localScale.x > 0) 
			{
				return gravityDirection.DOWN;
			}
			else
			{
				return gravityDirection.UP;
			}
		}
		else if (gravity == gravityDirection.UP)
		{
			if (transform.localScale.x > 0) 
			{
				return gravityDirection.LEFT;
			}
			else
			{
				return gravityDirection.RIGHT;
			}
		}

		throw new System.InvalidOperationException("Bad news in the getFacingDirection function");
	}

	/*
	 * Since our collider is made of frictionless material, we have to stop momentum
	 * when we are not holding down a key.
	 */
	void stopSliding()
	{
		if(isVertical (gravity))
		{
			rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
		}

		if(isHorizontal (gravity))
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
		}
	}
	
	void doMovement()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		float movement;

		stopSliding ();
		
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

		// Set the animation to face the correct direction (could possibly be moved into the state machine?)
		
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
		if (grounded && Input.GetButtonDown("Jump"))
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
			anim.SetTrigger ("HurtTrigger"); // cause hurt animation
			hurtInvincibility = true;

			//Adjust health. Currently just subtracts 25 health
			GameObject character = GameObject.Find ("Character");
			PlayerHealth health = (PlayerHealth) character.GetComponent ("PlayerHealth");
			health.adjustCurHealth(-25);
			//hurtInvincibilityTimer.Start ();
			StartCoroutine("removeHurtInvincibility");
			// set to partly transparent to indicate invincibility
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
			sr.color = new Color(1f,1f,1f,.6f);
		}
	}

	/*
	 * Called when the character hits a level end block
	 */
	public void endLevel(string nextLevel)
	{
		levelToLoad = nextLevel;
		die ();
	}

	/*
	 * Called when the player should die.  This can happen either by losing all health
	 * or by ending the level by hitting a level end block.
	 */
	private void die()
	{
		anim.SetTrigger ("DieTrigger");
		disableMovement ();
	}

	/*
	 * Loads the next level.
	 * Should only be called by the event at the end of the dying animation.
	 */
	public void loadNextLevel()
	{
		Application.LoadLevel (levelToLoad);
	}

	/*
	 * Disables player input and cancels velocity & gravity.  
	 */
	private void disableMovement()
	{
		rigidbody2D.isKinematic = true;
		this.enabled = false;
	}

	public void setKey(bool hasKey){
		key = hasKey;
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

		//if (cameraScript.currentMode == CameraController.ScreenMode.HORIZONTAL)
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
		//else
		//if(cameraScript.currentMode == CameraController.ScreenMode.VERTICAL)
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