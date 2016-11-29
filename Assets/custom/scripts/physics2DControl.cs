using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof(Rigidbody2D))]

public class physics2DControl : MonoBehaviour {
	
	[SerializeField] private LayerMask m_WhatIsGround;
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
	[SerializeField] private float m_MaxSpeed = 10f;
	[SerializeField] private float m_JumpForce = 400f;
	[SerializeField] private bool m_AirControl = false;

	const float groundedRadius = .2f;
	const float k_CeilingRadius = .01f;

	Transform groundCheck;
	Transform ceilingCheck;
	private Rigidbody2D myRigidBody2D;
	Animator anim;
	bool grounded;
	bool crouch;
	public string currGround;

	float hor;
	private bool m_Jump;
	private bool m_FacingRight = true;
	story2Dmanager gameManager;

	// Use this for initialization
	void Awake () {
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
		myRigidBody2D = GetComponent<Rigidbody2D>();	
		gameManager = GameObject.Find ("normalGround").GetComponent<story2Dmanager> ();
	}


	void Update()
	{
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders [i].gameObject != gameObject) {
				if (colliders [i].tag == "drone" && colliders[i].transform.parent.name != currGround) {
					colliders [i].GetComponentInParent<MoovingPlataform> ().absorbNewWeight ();
					//Debug.Log ("Hit drone: " + colliders [i].name);
				}
				currGround = colliders [i].transform.parent.name;
				grounded = true;
			}
		}
		anim.SetFloat("verticalSpeed", (-1)*myRigidBody2D.velocity.y);

		// If not crouching, check to see if the character can stand up
		if (!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}
		anim.SetBool("Ground", grounded);

		if (grounded || m_AirControl) {
			hor = CrossPlatformInputManager.GetAxis ("Horizontal");

			hor = (crouch ? hor * m_CrouchSpeed : hor);
			myRigidBody2D.velocity = new Vector2 (hor * m_MaxSpeed, myRigidBody2D.velocity.y);
			anim.SetFloat("Speed", Mathf.Abs(hor));

			if (hor > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (hor < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (grounded && m_Jump && anim.GetBool("Ground"))
		{
			// Add a vertical force to the player.
			grounded = false;
			anim.SetBool("Ground", false);
			myRigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
			m_Jump = false;
			currGround = "none";
		}

	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "elevator") {
			Debug.Log ("Entered elevator trigger. Success!!!");
		}
		else {
			Debug.Log ("Player entered other trigger: "+other.name);
			if (other.name == "killerBoundary") {
				gameManager.playerDie ();
			}
		}
	}
}
