using UnityEngine;
using UnityEngine.Events;

public class CharacterMovementController : MonoBehaviour
{
	private Animator Animator { get; set; }
	private Rigidbody2D Rigidbody2D { get; set; }
	private Transform GroundCheck  { get; set; }
	private LayerMask WhatIsGround  { get; set; }
	private Vector3 velocity;
	private bool AirControl  { get; set; }
	public bool IsGrounded { get; private set; }
	private float GroundedRadius { get; set; }
	private float JumpForce  { get; set; }
	private float MovementSmoothing  { get; set; }
	private bool IsLookingRight { get; set; } = true;

	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;
	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		if (GameObject.Find("Player") is null)
		{
			Debug.LogError(
				"ERROR: <CharacterMovementController> - Player game object was not found in the game object " +
				"hierarchy."
				);
			Application.Quit(1);
		}
		
		if ((Animator = GameObject.Find("Player").GetComponent<Animator>()) is null)
		{
			Debug.LogError(
				"ERROR: <CharacterMovementController> - Player game object is missing Animator component."
				);
			Application.Quit(1);
		}
		
		if ((Rigidbody2D = GameObject.Find("Player").GetComponent<Rigidbody2D>()) is null)
		{
			Debug.LogError(
				"ERROR: <CharacterMovementController> - Player game object is missing Rigidbody2D component."
				);
			Application.Quit(1);
		}

		if (GameObject.Find("Player/GroundCheck") is null)
		{
			Debug.LogError(
				"ERROR: <CharacterMovementController> - Player/GroundCheck game object was not found in the " +
				"game object hierarchy."
				);
			Application.Quit(1);
		}
		GroundCheck = GameObject.Find("Player/GroundCheck").transform;
		
		WhatIsGround = LayerMask.GetMask("GameWorld");
		velocity = Vector3.zero;

		AirControl = true;
		IsGrounded = false;
		GroundedRadius = 0.2f;
		JumpForce = 850f;
		MovementSmoothing = 0.01f;
		IsLookingRight = true;

		if (OnLandEvent == null)
		{
			OnLandEvent = new UnityEvent();	
		}
	}

	private void FixedUpdate()
	{
		bool wasGrounded = IsGrounded;
		IsGrounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(
			GroundCheck.position, GroundedRadius, WhatIsGround
			);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				IsGrounded = true;
				if (!wasGrounded)
				{
					Animator.SetBool("IsJumping", false);
					OnLandEvent.Invoke();
				}
			}
		}
	}

	public void Move(float move, bool jump)
	{
		if (IsGrounded)
		{
			Animator.SetBool("IsGrounded", true);	
		}
		else
		{
			Animator.SetBool("IsGrounded", false);	
		}
		
		if (IsGrounded && move != 0f)
		{
			Animator.SetBool("IsWalking", true);	
		}
		else
		{
			Animator.SetBool("IsWalking", false);
		}
		
		if (!IsGrounded && Rigidbody2D.velocity.y < 0.001f)
		{
			Animator.SetBool("IsFalling", true);
		}
		else if (IsGrounded || Rigidbody2D.velocity.y >= 0.001f)
		{
			Animator.SetBool("IsFalling", false);
		}
		
		if (IsGrounded || AirControl)
		{

			Vector3 targetVelocity = new Vector2(move * 10f, Rigidbody2D.velocity.y);
			Rigidbody2D.velocity = Vector3.SmoothDamp(
					Rigidbody2D.velocity, targetVelocity, ref velocity, MovementSmoothing
					);

			if (move > 0 && !IsLookingRight)
			{
				Flip();
			}
			else if (move < 0 && IsLookingRight)
			{
				Flip();
			}
		}

		if (IsGrounded && jump)
		{
			Animator.SetBool("IsJumping", true);
			
			IsGrounded = false;
			Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0f);
			Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
		}
	}
	
	private void Flip()
	{
		IsLookingRight = !IsLookingRight;
		transform.Rotate(0f, 180f, 0f);
	}
}
