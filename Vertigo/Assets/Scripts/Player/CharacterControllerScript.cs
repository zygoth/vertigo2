using UnityEngine;
using System.Collections;

public class CharacterControllerScript: MonoBehaviour
{
	// used to create shots when firing
	public GameObject shotPrefab;
	private float SHOTSPEED = 15f;
	private float SHOTVERTICALDIST = .4f;
	private float SHOTHORIZONTALDIST = 1f;
	// Maximum run speed
	private float MAXSPEED = .28f;
	private float GRAVITYSPEED = .012f;
	private float COLLISIONBUFFER = .01f;
	private Vector2 gravityVector = new Vector2 (0f, -.012f);
	public int num_keys = 0;
	private float MAXFALLSPEED = .5f;
	bool facingRight = true;	
	bool hurtInvincibility = false;
	Animator anim;
	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	private float JUMPVELOCITY = .42f;
	private string levelToLoad;
	private Vector2 velocity = new Vector2(0,0);
	// Enum for gravity direction
	public enum gravityDirection {DOWN, LEFT, UP, RIGHT};
	// The current gravity direction
	public gravityDirection gravity = gravityDirection.DOWN;

	private CollisionDetector collisionDetector;


	/*
	 * Called when the object is instantiated (I think).
	 */
	void Start()
	{
		levelToLoad = Application.loadedLevelName;
		anim = GetComponent<Animator>();
		initializeCollisionDetector ();
	}

	void initializeCollisionDetector()
	{
		float RAYCASTLENGTH = 2f;
		float HALFPLAYERWIDTH = .7f;//.709f;
		float HALFPLAYERHEIGHT = .90f;//.99f;//1.02f;
		float HALFPLAYERFOOTWIDTH = .2f;

		collisionDetector = new CollisionDetector(whatIsGround,
		                                          new CollisionDetectorRaycast {angle = 0f, relativePosition = new Vector2(HALFPLAYERWIDTH, -HALFPLAYERHEIGHT), length = RAYCASTLENGTH},
		new CollisionDetectorRaycast {angle = 0f, relativePosition = new Vector2(HALFPLAYERWIDTH, HALFPLAYERHEIGHT), length = RAYCASTLENGTH},
		new CollisionDetectorRaycast {angle = Mathf.PI / 2f, relativePosition = new Vector2(HALFPLAYERWIDTH, HALFPLAYERHEIGHT), length = RAYCASTLENGTH},
		new CollisionDetectorRaycast {angle = Mathf.PI / 2f, relativePosition = new Vector2(-HALFPLAYERWIDTH, HALFPLAYERHEIGHT), length = RAYCASTLENGTH},
		new CollisionDetectorRaycast {angle = Mathf.PI, relativePosition = new Vector2(-HALFPLAYERWIDTH, HALFPLAYERHEIGHT), length = RAYCASTLENGTH},
		new CollisionDetectorRaycast {angle = Mathf.PI, relativePosition = new Vector2(-HALFPLAYERWIDTH, -HALFPLAYERHEIGHT), length = RAYCASTLENGTH},
		new CollisionDetectorRaycast {angle = 3f*Mathf.PI / 2f, relativePosition = new Vector2(HALFPLAYERWIDTH, -HALFPLAYERHEIGHT - .1f), length = RAYCASTLENGTH},
		new CollisionDetectorRaycast {angle = 3f*Mathf.PI / 2f, relativePosition = new Vector2(-HALFPLAYERWIDTH, -HALFPLAYERHEIGHT - .1f), length = RAYCASTLENGTH}); 

	}

	/*
	 * Called at fixed intervals during the game. Use for things that must happen at steady intervals of time (movement). 
	 */
	void FixedUpdate()
	{
		doGravity ();
		doWalking ();

		updatePosition ();
	}

	/*
	 * Called every time a frame is rendered on screen, so may happen at variable rates.  Use for interrupt-based events.
	 * This is the main high-level method of this class.
	 */
	void Update()
	{
		doWrapping ();
		doGroundCheck ();
		doShootCheck ();
		doJumpCheck ();





		if (Mathf.Round(transform.localEulerAngles.z) % 90f > 1) // TODO: this is a hack to fix the strange rotation bug.
		{
			Debug.Log ("craps + " + transform.localEulerAngles.z % 90);
			transform.localEulerAngles = new Vector3 (0, 0, (int)gravity * -90f);
		}
	}

	/*
	 * Acceleration due to gravity. Each frame, add a little to the velocity depending
	 * on which way gravity is currently going.
	 */
	private void doGravity()
	{
		switch(gravity)
		{
		case gravityDirection.DOWN:
			if(velocity.y > -MAXFALLSPEED)
				velocity += gravityVector;
			break;
		case gravityDirection.LEFT:
			if(velocity.x > -MAXFALLSPEED)
				velocity += gravityVector;
			break;
		case gravityDirection.RIGHT:
			if(velocity.x < MAXFALLSPEED)
				velocity += gravityVector;
			break;
		case gravityDirection.UP:
			if(velocity.y < MAXFALLSPEED)
				velocity += gravityVector;
			break;
		}
	}

	/*
	 * Updates the position of the object according to the current velocity vector.
	 * Updates x first, then y. This prevents sinking into corners & other problems.
	 */
	private void updatePosition()
	{
		float xvel = velocity.x;
		float yvel = velocity.y;

		// Get distance to any objects going right or left
		float rightDistance = collisionDetector.getMaxDistance (transform.position, 0);
		float leftDistance = collisionDetector.getMaxDistance (transform.position, Mathf.PI);

		// If the raycast said there's something close, we shouldn't go past it.
		if(xvel > 0)
			xvel = Mathf.Min (rightDistance - COLLISIONBUFFER, velocity.x);
		else
			xvel = Mathf.Max (-leftDistance + COLLISIONBUFFER, velocity.x);

		// Update the actual x position.
		rigidbody2D.transform.position += new Vector3 (xvel, 0, 0);

		// Get distance to any objects going up or down
		float upDistance = collisionDetector.getMaxDistance (transform.position, Mathf.PI / 2f);
		float downDistance = collisionDetector.getMaxDistance (transform.position, 3f*Mathf.PI / 2f);

		// If the raycast said there's something close, we shouldn't go past it.
		if(yvel > 0)
			yvel = Mathf.Min (upDistance - COLLISIONBUFFER, velocity.y);
		else
			yvel = Mathf.Max (-downDistance + COLLISIONBUFFER, velocity.y);

		// Update the actual y position.
		rigidbody2D.transform.position += new Vector3 (0, yvel, 0);

		// Panic: if we are somehow inside an obstacle, try to get out.
		if(rightDistance == 0f)
			transform.position += new Vector3(-.1f, 0f, 0f);
		if(leftDistance == 0f)
			transform.position += new Vector3(.1f, 0f, 0f);
		if(upDistance == 0f)
			transform.position += new Vector3(0f, -.1f, 0f);
		if(downDistance == 0f)
			transform.position += new Vector3(0f, .1f, 0f);

		// update velocity vector
		velocity = new Vector2 (xvel, yvel);
	}

	public void doShootCheck(bool fire = false)
	{
		if(Input.GetButtonDown("Fire1") || fire == true)
		{
			float shotXOffset = 0f;
			float shotYOffset = 0f;

			gravityDirection facingDirection = getFacingDirection();
			Vector2 shotVelocity = new Vector2(0f,0f);

			// calculate horizontal distance and velocity based on direction character is facing
			switch(facingDirection)
			{
			case gravityDirection.DOWN:
				shotYOffset = -SHOTHORIZONTALDIST;
				shotVelocity = new Vector2(0f, -SHOTSPEED);
				break;
			case gravityDirection.LEFT:
				shotXOffset = -SHOTHORIZONTALDIST;
				shotVelocity = new Vector2(-SHOTSPEED, 0f);
				break;
			case gravityDirection.RIGHT:
				shotXOffset = SHOTHORIZONTALDIST;
				shotVelocity = new Vector2(SHOTSPEED, 0f);
				break;
			case gravityDirection.UP:
				shotYOffset = SHOTHORIZONTALDIST;
				shotVelocity = new Vector2(0f, SHOTSPEED);
				break;
			}

			// calculate vertical distance based on current gravity
			switch(gravity)
			{
			case gravityDirection.DOWN:
				shotYOffset = -SHOTVERTICALDIST;
				break;
			case gravityDirection.LEFT:
				shotXOffset = -SHOTVERTICALDIST;
				break;
			case gravityDirection.RIGHT:
				shotXOffset = SHOTVERTICALDIST;
				break;
			case gravityDirection.UP:
				shotYOffset = SHOTVERTICALDIST;
				break;
			}

			GameObject shot = Instantiate (shotPrefab, new Vector3(transform.position.x + shotXOffset, transform.position.y + shotYOffset, 0f), Quaternion.identity) as GameObject;
			Shot script = (Shot)shot.GetComponent ("Shot");
			script.rigidbody2D.velocity = shotVelocity;
			SoundManager.playSound ("Shoot Sound");
		}

		if (Input.GetButton("Fire1") || fire == true)
		{
			anim.SetBool("Shooting", true);
		}
		else
		{
			anim.SetBool("Shooting", false);
		}
	}

	/*
	 * Coroutine, called to remove invincibility after 3 seconds when hit.
	 */
	private IEnumerator removeHurtInvincibility()
	{
		while(true)
		{
			yield return new WaitForSeconds(3.0f); // wait 3 seconds

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
			velocity = new Vector2(0, velocity.y);
		}

		if(isHorizontal (gravity))
		{
			velocity = new Vector2(velocity.x, 0);
		}
	}
	
	public void doWalking(float XAxis = 0f, float YAxis = 0f)
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		if(XAxis != 0) horizontal = XAxis;
		if(YAxis != 0) vertical = YAxis;
		
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
				velocity = new Vector2(velocity.x, vertical * MAXSPEED);
			}
			else
			{
				velocity = new Vector2(horizontal * MAXSPEED, velocity.y);
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

	/*
	 * Jumps if the button is pressed and the object is touching the ground.
	 */
	public void doJumpCheck(bool jump = false)
	{
		if (grounded && (Input.GetButtonDown("Jump") || jump == true))
		{
			anim.SetBool ("Ground", false);
			grounded = false;

			Vector2 jumpVector = new Vector2();

			switch(gravity)
			{
				case gravityDirection.DOWN:
					jumpVector = (new Vector2(0, JUMPVELOCITY));
					break;
				case gravityDirection.RIGHT:
					jumpVector = (new Vector2(-JUMPVELOCITY, 0));
					break;
				case gravityDirection.UP:
					jumpVector = (new Vector2(0, -JUMPVELOCITY));
					break;
				case gravityDirection.LEFT:
					jumpVector = (new Vector2(JUMPVELOCITY, 0));
					break;
			}

			velocity = jumpVector;
			SoundManager.playSound ("Jump Sound");
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
			health.adjustCurHealth(-0.25);
			if(health.curHealth <= 0){
				die ();
			}
			else{
				//set to partly transparent to indicate invincibility
				SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
				sr.color = new Color(1f,1f,1f,.6f);
				Invoke ("removeInvince", 3);//Remove incincibility in 3 seconds I think this fixed the hurt bugs
			}
		}
	}

	private void removeInvince(){
		hurtInvincibility = false;
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.color = new Color(1f,1f,1f,1f);
	}

	public void heal()
	{
		GameObject character = GameObject.Find ("Character");
		PlayerHealth health = (PlayerHealth) character.GetComponent ("PlayerHealth");
		health.adjustCurHealth(0.25);
	}

	/*
	 * Called when the character hits a level end block
	 */
	public void endLevel(string nextLevel)
	{
		levelToLoad = nextLevel;
		LoadLevelBlock.updateLoadLevelBlock (nextLevel);
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
	 * This is here for the restart command.
	 */
	public void loadLevel(string newLevel){
		Application.LoadLevel(newLevel);
	}

	/*
	 * Disables player input and cancels velocity & gravity.  
	 */
	public void disableMovement()
	{
		rigidbody2D.isKinematic = true;
		this.enabled = false;
	}

	/*
	 * Re-enables movement
	 */
	public void enableMovement()
	{
		rigidbody2D.isKinematic = false;
		this.enabled = true;
	}

	public void addKey(){
		num_keys += 1;
	}

	public void removeKey(){
		num_keys -= 1;
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
		float newRotation = 0;
		transform.localEulerAngles = new Vector3(0,0, (int)newGravity * -90f);
		gravity = newGravity;

		switch (newGravity)
		{
		case gravityDirection.DOWN:
			gravityVector.x = 0f;
			gravityVector.y = -GRAVITYSPEED;
			newRotation = 0f;
			break;
		case gravityDirection.LEFT:
			gravityVector.x = -GRAVITYSPEED;
			gravityVector.y = 0f;
			newRotation = Mathf.PI * 1.5f;
			break;
		case gravityDirection.UP:
			gravityVector.x = 0f;
			gravityVector.y = GRAVITYSPEED;
			newRotation = Mathf.PI * 1f;
			break;
		case gravityDirection.RIGHT:
			gravityVector.x = GRAVITYSPEED;
			gravityVector.y = 0f;
			newRotation = Mathf.PI * .5f;
			break;
		}

		collisionDetector.setRotation (newRotation);
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