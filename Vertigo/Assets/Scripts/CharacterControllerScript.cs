using UnityEngine;
using System.Collections;

public class CharacterControllerScript: MonoBehaviour
{
	public float maxSpeed = 10f;
	public Vector2 gravityVector = new Vector2 (0f, -30f);
	bool facingRight = true;
	
	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;
	public enum gravityDirection {DOWN, LEFT, UP, RIGHT};
	public gravityDirection gravity = gravityDirection.DOWN;
	
	void Start()
	{
		anim = GetComponent<Animator>();
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
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, vertical * maxSpeed);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(horizontal * maxSpeed, rigidbody2D.velocity.y);
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
	
	void FixedUpdate()
	{
		doGroundCheck ();

		doMovement ();

		rigidbody2D.AddForce (gravityVector);
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
					jumpVector = (new Vector2(0, jumpForce));
					break;
				case gravityDirection.RIGHT:
					jumpVector = (new Vector2(-jumpForce, 0));
					break;
				case gravityDirection.UP:
					jumpVector = (new Vector2(0, -jumpForce));
					break;
				case gravityDirection.LEFT:
					jumpVector = (new Vector2(jumpForce, 0));
					break;
			}

			rigidbody2D.AddForce (jumpVector);
		}
	}

	void RotateLeft () 
	{
		transform.Rotate (Vector3.forward * -90);
		gravity = (gravityDirection)((((int)gravity) + 1) % 4);
		gravityVector = new Vector2 (gravityVector.y, -gravityVector.x);
	}

	void doGravityCheck()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			RotateLeft();
		}
	}

	void Update()
	{
		doJumpCheck ();

		doGravityCheck ();
	}
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}