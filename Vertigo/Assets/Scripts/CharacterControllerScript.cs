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
		float move = Input.GetAxis("Horizontal");
		
		anim.SetFloat("Speed", Mathf.Abs(move));

		if(Mathf.Abs(move) > 0)
		{
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}

		if(move > 0 &&!facingRight)
			Flip();
		else if (move < 0 && facingRight)
			Flip();
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
			rigidbody2D.AddForce (new Vector2(0, jumpForce));
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